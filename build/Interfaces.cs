using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace Buildz;

interface IHazConfiguration : INukeBuild
{
    [Parameter]
    Configuration Configuration => TryGetValue(() => Configuration) ??
                                   (IsLocalBuild ? Configuration.Debug : Configuration.Release);
}

interface IHazSolution : INukeBuild
{
    [Solution(GenerateProjects = true)]
    [Required]
    public Solution Solution => TryGetValue(() => Solution);
}

interface IClean : IHazSolution
{
    Target Clean => _ => _
        .Executes(() =>
        {
            DotNetClean(_ => _
                .SetProject(Solution));
        });
}

interface IRestore : IClean
{
    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(_ => _
                .SetProjectFile(Solution));
        });
}

interface ICompile : IRestore, IHazConfiguration
{
    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(_ => _
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration));
        });
}
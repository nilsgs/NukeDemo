using Nuke.Common;
using Nuke.Common.CI.GitHubActions;

namespace Buildz;


[GitHubActions(
    "continuous",
    GitHubActionsImage.UbuntuLatest,
    On = new[] { GitHubActionsTrigger.Push },
    InvokedTargets = new[] { nameof(ICompile.Compile) })]
class Build : NukeBuild, ICompile
{
    public static int Main() => Execute<Build>(x => ((ICompile)x).Compile);
}
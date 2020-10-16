using System.Threading.Tasks;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.Npm;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>(x => x.Default);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion] readonly GitVersion GitVersion;

    AbsolutePath BackendDirectory => RootDirectory / "src/server";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath FrontendDirectory => RootDirectory / "src/client";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    [PathExecutable] readonly Tool Npm;
    [PathExecutable] readonly Tool Git;

    Target Default => _ => _
        .DependsOn(CompileBackend)
        .DependsOn(CompileFrontend)
        .Executes(() =>
        {
            // do nothing
        });

    Target FullClean => _ => _
        .Executes(() =>
        {
            Git("clean -xf");
            BackendDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            FrontendDirectory.GlobDirectories("**/node_modules", "**/dist").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target FastClean => _ => _
        .Executes(() =>
        {
            BackendDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .DependsOn(IsLocalBuild ? FastClean : FullClean)
        .Executes(() =>
        {
            DotNetRestore(s => s.SetProjectFile(Solution));
        });

    Target CompileBackend => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore());
        });

    Target CompileFrontend => _ => _
        .After(CompileBackend)
        .Executes(() =>
        {
            Npm("i", workingDirectory: FrontendDirectory);
            Npm("run build", workingDirectory: FrontendDirectory);
        });
}

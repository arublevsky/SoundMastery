using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Default);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;

    AbsolutePath BackendDirectory => RootDirectory / "src/server";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath FrontendDirectory => RootDirectory / "src/client";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath DockerDirectory => RootDirectory / "tools/docker";

    [PathExecutable("npm")] readonly Tool Npm;
    [PathExecutable("git")] readonly Tool Git;
    [PathExecutable("docker-compose")] readonly Tool DockerCompose;

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
                .EnableNoRestore());
        });

    Target CompileFrontend => _ => _
        .After(CompileBackend)
        .Executes(() =>
        {
            Npm("i", workingDirectory: FrontendDirectory);
            Npm("run build", workingDirectory: FrontendDirectory);
        });

    Target Deploy => _ => _
        .Executes(() =>
        {
            DockerCompose($"--env-file ./../../config/.env.dev up --build -d", workingDirectory: DockerDirectory);
        });
}

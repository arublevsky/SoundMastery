using System;
using System.IO;
using System.Linq;
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

    [Solution] readonly Solution Solution = null!;

    static AbsolutePath BackendDirectory => RootDirectory / "src/server";
    static AbsolutePath TestsDirectory => RootDirectory / "tests";
    static AbsolutePath FrontendDirectory => RootDirectory / "src/client";
    static AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    static AbsolutePath DockerDirectory => RootDirectory / "tools/docker";
    static AbsolutePath ConfigDirectory => RootDirectory / ".config";

    [PathExecutable("npm")] readonly Tool Npm = null!;
    [PathExecutable("git")] readonly Tool Git = null!;
    [PathExecutable("docker-compose")] readonly Tool DockerCompose = null!;
    [PathExecutable("docker")] readonly Tool Docker = null!;

    Target Default => _ => _
        .DependsOn(CompileBackend)
        .DependsOn(CompileFrontend)
        .DependsOn(TestBackend)
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

    Target TestBackend => _ => _
        .DependsOn(CompileBackend)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Solution.GetProject("SoundMastery.Tests"))
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .EnableNoRestore()
                .SetLogger("trx")
                .SetLogOutput(true)
                .SetArgumentConfigurator(arguments => arguments.Add("/p:CollectCoverage={0}", true)
                    .Add("/p:CoverletOutput={0}/", ArtifactsDirectory / "coverage")
                    .Add("/p:UseSourceLink={0}", "true")
                    .Add("/p:CoverletOutputFormat={0}", "cobertura"))
                .SetResultsDirectory(ArtifactsDirectory / "tests"));
        });

    Target CompileFrontend => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            Npm("i --legacy-peer-deps", workingDirectory: FrontendDirectory);
            Npm("run build", workingDirectory: FrontendDirectory);
        });

    Target BuildDockerImages => _ => _
        .Executes(() =>
        {
            SetAppVersionInternal();
            DockerCompose($"-f {DockerComposePath} --env-file {DotEnvPath} build");
        });

    Target DeployDocker => _ => _
        .DependsOn(BuildDockerImages)
        .Executes(() =>
        {
            DockerCompose($"-f {DockerComposePath} --env-file {DotEnvPath} up -d");
        });

    Target PublishImages => _ => _
        .DependsOn(BuildDockerImages)
        .Executes(() =>
        {
            if (IsLocalBuild)
            {
                return;
            }

            DockerCompose($"-f {DockerComposePath} push");
        });

    Target SetAppVersion => _ => _.Executes(SetAppVersionInternal);

    static string Env => IsLocalBuild ? "dev" : "ci";

    static string DotEnvPath => Path.Combine(ConfigDirectory, $"{Env}.env");

    static string DockerComposePath => Path.Combine(DockerDirectory, $"docker-compose.{Env}.yml");

    void SetAppVersionInternal()
    {
        var version = GetVersionInternal();
        Environment.SetEnvironmentVariable("APP_VERSION", version);
        File.AppendAllLines(DotEnvPath, new[] { Environment.NewLine, $"APP_VERSION={version}" });
    }

    string GetVersionInternal()
    {
        DotNet("tool restore", workingDirectory: RootDirectory);

        var version = DotNet("minver", logOutput: false, logInvocation: false)
            .First(x => x.Type != OutputType.Err)
            .Text;

        return IsTaggedCommit() ? version : $"{version}-{GetCommitSha().Substring(0, 5)}";
    }

    bool IsTaggedCommit()
    {
        var isTagged = false;
        try
        {
            var sha = GetCommitSha();
            var tag = Git($"describe --exact-match {sha}", logOutput: false, logInvocation: false).Single().Text;
            isTagged = !string.IsNullOrEmpty(tag);
        }
        catch
        {
            // no tag, ignore
        }

        return isTagged;
    }

    string GetCommitSha()
    {
        // github does merge into target branch during checkout
        // so to have a deterministic version for the pull request, the 2nd parent of merge commit is taken
        var commitPath = IsLocalBuild ? "HEAD" : "HEAD^2";
        return Git($"rev-parse {commitPath}").Single().Text;
    }
}

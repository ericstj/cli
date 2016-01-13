using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.DotNet.Cli.Utils;
using Microsoft.DotNet.ProjectModel.Graph;

namespace Microsoft.DotNet.Tools.Restore
{
    public static class Dnx
    {
        public static int RunRestore(IEnumerable<string> args, bool quiet)
        {
            var result = RunNuGet("restore", args, quiet)
                .ForwardStdErr()
                .ForwardStdOut()
                .Execute();

            return result.ExitCode;
        }

        public static int RunPackageInstall(LibraryRange dependency, string projectPath, IEnumerable<string> args)
        {
            var result = RunDnx(new List<string> { "install", dependency.Name, dependency.VersionRange.OriginalString, projectPath }.Concat(args))
                .ForwardStdErr()
                .ForwardStdOut()
                .Execute();

            return result.ExitCode;
        }

        private static Command RunDnx(IEnumerable<string> dnxArgs)
        {
            return Command.Create("dotnet-dnx", dnxArgs);
        }

        private static Command RunNuGet(string command, IEnumerable<string> args, bool quiet)
        {
            var allArgs = args.ToList();
            allArgs.Insert(0, "restore");
            if (quiet)
            {
                allArgs.Insert(0, "Warning");
                allArgs.Insert(0, "--verbosity");
            }
            allArgs.Insert(0, Path.Combine(AppContext.BaseDirectory, "NuGet.CommandLine.XPlat.dll"));
            return Command.Create(CoreHost.HostExePath, allArgs);
        }
    }
}
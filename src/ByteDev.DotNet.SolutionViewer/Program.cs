using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ByteDev.Cmd;
using ByteDev.Cmd.Arguments;

namespace ByteDev.DotNet.SolutionViewer
{
    internal class Program
    {
        private static readonly Output Output = new Output();

        private static ProgramArgs _programArgs;
        private static List<CmdAllowedArg> _cmdAllowedArgs;

        private static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();

            Output.WriteAppHeader();

            _cmdAllowedArgs = CmdAllowedArgsFactory.Create();

            try
            {
                var cmdArgInfo = new CmdArgInfo(args, _cmdAllowedArgs);

                _programArgs = new ProgramArgs(cmdArgInfo);

                var slnPaths = GetSlnPaths();

                if (slnPaths.Count == 0)
                {
                    HandleError($"{_programArgs.Path} and its sub directories contain no solution files.");
                }

                Output.WriteLine($"{slnPaths.Count} solutions found.");
                Output.WriteBlankLines(1);

                WriteSlnDetails(slnPaths);
            }
            catch (Exception ex)
            {
                HandleError(ex.Message);
            }

            sw.Stop();

            Output.WriteBlankLines(1);
            Output.WriteLine($"Time taken: {sw.Elapsed.TotalSeconds} seconds.");
            Output.WriteBlankLines(1);
        }
        
        private static void WriteSlnDetails(IEnumerable<string> slnPaths)
        {
            var options = CreateWriteSlnProjectsOptions();

            foreach (var slnPath in slnPaths)
            {
                var slnFileInfo = new FileInfo(slnPath);

                Output.WriteSlnHeader(slnFileInfo);
                Output.WriteSlnProjects(slnFileInfo, options);
            }
        }

        private static IList<string> GetSlnPaths()
        {
            if (IsSlnFile(_programArgs.Path))
                return new List<string> { _programArgs.Path };

            var slnPaths = Directory.EnumerateFiles(_programArgs.Path, "*.sln", SearchOption.AllDirectories)?.ToList();

            if (slnPaths == null || slnPaths.Count == 0)
                return new List<string>();

            RemoveIgnoreSlns(slnPaths);

            return slnPaths;
        }

        private static void RemoveIgnoreSlns(List<string> slnPaths)
        {
            foreach (string slnFile in _programArgs.IgnoreSlnFiles)
            {
                slnPaths.RemoveAll(p => p.EndsWith(slnFile));
            }
        }

        private static bool IsSlnFile(string filePath)
        {
            return filePath.ToLower().EndsWith(".sln");
        }

        private static WriteSlnProjectsOptions CreateWriteSlnProjectsOptions()
        {
            return new WriteSlnProjectsOptions
            {
                DisplayProjectType = true,
                DisplayPackageReferences = _programArgs.DisplayPackageReferences,
                DisplayProjectReferencePaths = _programArgs.DisplayProjectReferencePaths,
                DisplayProjectReferenceNames = _programArgs.DisplayProjectReferenceNames
            };
        }

        private static void HandleError(string message)
        {
            Output.WriteLine(message, new OutputColor(ConsoleColor.Red, ConsoleColor.Black));
            Output.WriteLine(_cmdAllowedArgs.HelpText());
            Environment.Exit(0);
        }
    }
}

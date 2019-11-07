using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ByteDev.Cmd;
using ByteDev.Cmd.Arguments;

namespace ByteDev.DotNet.SolutionViewer
{
    internal class Program
    {
        private static readonly Output Output = new Output();

        private static CmdArgInfo _cmdArgInfo;
        private static IList<CmdAllowedArg> _cmdAllowedArgs;

        private static void Main(string[] args)
        {
            Output.WriteAppHeader();

            _cmdAllowedArgs = CmdAllowedArgsFactory.Create();

            try
            {
                _cmdArgInfo = new CmdArgInfo(args, _cmdAllowedArgs);
                
                var path = _cmdArgInfo.Arguments.Single(a => a.ShortName == 'p');
                var useTable = _cmdArgInfo.Arguments.SingleOrDefault(a => a.ShortName == 't');
                
                var slnPaths = GetSlnPaths(path.Value);

                Output.WriteLine($"{slnPaths.Count} solutions found.");
                Output.WriteBlankLines();

                if (useTable != null)
                    WriteSlnDetailsAsTable(slnPaths);
                else
                    WriteSlnDetails(slnPaths);
            }
            catch (Exception ex)
            {
                HandleError(ex.Message);
            }
        }

        private static void WriteSlnDetailsAsTable(IList<string> slnPaths)
        {
            var options = new WriteSlnProjectsOptions { WriteProjectType = true };

            foreach (var slnPath in slnPaths)
            {
                var slnFileInfo = new FileInfo(slnPath);

                Output.WriteSlnHeader(slnFileInfo);
                Output.WriteSlnProjectsInTable(slnFileInfo, options);
            }
        }

        private static void WriteSlnDetails(IEnumerable<string> slnPaths)
        {
            var options = new WriteSlnProjectsOptions {WriteProjectType = true};

            foreach (var slnPath in slnPaths)
            {
                var slnFileInfo = new FileInfo(slnPath);

                Output.WriteSlnHeader(slnFileInfo);
                Output.WriteSlnProjects(slnFileInfo, options);
            }
        }

        private static IList<string> GetSlnPaths(string basePath)
        {
            if(string.IsNullOrEmpty(basePath))
                throw new ArgumentException("No base path supplied as argument.");

            var slnPaths = Directory.EnumerateFiles(basePath, "*.sln", SearchOption.AllDirectories)?.ToList();

            if (slnPaths == null || slnPaths.Count == 0)
                HandleError($"{basePath} and its sub directories contain no solution files.");

            return slnPaths;
        }

        private static void HandleError(string message)
        {
            Output.WriteLine(message, new OutputColor(ConsoleColor.Red, ConsoleColor.Black));
            Output.WriteLine(_cmdAllowedArgs.HelpText());
            Environment.Exit(0);
        }
    }
}

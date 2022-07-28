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

        private static CmdArgInfo _cmdArgInfo;
        private static IList<CmdAllowedArg> _cmdAllowedArgs;

        private static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();

            Output.WriteAppHeader();

            _cmdAllowedArgs = CmdAllowedArgsFactory.Create();

            try
            {
                _cmdArgInfo = new CmdArgInfo(args, _cmdAllowedArgs);

                string path = _cmdArgInfo.GetPath();
                bool useTable = _cmdArgInfo.GetUseTable();
                List<string> slnsToIgnore = _cmdArgInfo.GetSlnsToIgnore();

                var slnPaths = GetSlnPaths(path, slnsToIgnore);

                if (slnPaths.Count == 0)
                {
                    HandleError($"{path} and its sub directories contain no solution files.");
                }

                Output.WriteLine($"{slnPaths.Count} solutions found.");
                Output.WriteBlankLines();

                if (useTable)
                    WriteSlnDetailsAsTable(slnPaths);
                else
                    WriteSlnDetails(slnPaths);
            }
            catch (Exception ex)
            {
                HandleError(ex.Message);
            }

            sw.Stop();

            Output.WriteBlankLines();
            Output.WriteLine($"Time taken: {sw.Elapsed.TotalSeconds} seconds.");
            Output.WriteBlankLines();
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

        private static IList<string> GetSlnPaths(string basePath, List<string> slnsToIgnore)
        {
            if(string.IsNullOrEmpty(basePath))
                throw new ArgumentException("No base or .sln file path supplied as argument.");

            if (IsSlnFile(basePath))
                return new List<string> { basePath };

            List<string> slnPaths = Directory.EnumerateFiles(basePath, "*.sln", SearchOption.AllDirectories)?.ToList();

            if (slnPaths == null || slnPaths.Count == 0)
                return new List<string>();

            if (slnsToIgnore == null)
                return slnPaths;

            var list = new List<string>();

            foreach (var slnPath in slnPaths)
            {
                var shouldIgnore = false;

                foreach (var slnToIgnore in slnsToIgnore)
                {
                    if (slnPath.EndsWith(slnToIgnore))
                    {
                        shouldIgnore = true;
                        break;
                    }
                }

                if (!shouldIgnore)
                    list.Add(slnPath);
            }
        
            return list;
        }

        private static bool IsSlnFile(string filePath)
        {
            return filePath.ToLower().EndsWith(".sln");
        }

        private static void HandleError(string message)
        {
            Output.WriteLine(message, new OutputColor(ConsoleColor.Red, ConsoleColor.Black));
            Output.WriteLine(_cmdAllowedArgs.HelpText());
            Environment.Exit(0);
        }
    }
}

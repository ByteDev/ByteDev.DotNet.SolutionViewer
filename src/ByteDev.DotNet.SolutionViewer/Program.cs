using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ByteDev.Cmd;

namespace ByteDev.DotNet.SolutionViewer
{
    internal class Program
    {
        private static readonly Output Output = new Output();

        private static void Main(string[] args)
        {
            Output.WriteHeader();

            if (args == null || args.Length == 0)
            {
                HandleError("No base path supplied as argument.");
                return;
            }

            var slnPaths = GetSlnPaths(args.First());

            if (slnPaths == null || slnPaths.Count == 0)
            {
                HandleError($"{args.First()} and its sub directories contain no solution files.");
                return;
            }

            Output.WriteLine($"{slnPaths.Count} solutions found.");
            Output.WriteBlankLines();

            foreach (var slnPath in slnPaths)
            {
                Output.WriteSlnDetails(slnPath);
            }
        }

        private static IList<string> GetSlnPaths(string basePath)
        {
            try
            {
                return Directory.EnumerateFiles(basePath, "*.sln", SearchOption.AllDirectories)?.ToList();
            }
            catch (DirectoryNotFoundException)
            {
                HandleError($"Directory '{basePath}' does not exist.");
                return null;
            }
        }

        private static void HandleError(string message)
        {
            Output.WriteLine(message, new OutputColor(ConsoleColor.Red));
            Environment.Exit(0);
        }
    }
}

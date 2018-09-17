using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using ByteDev.Cmd;
using ByteDev.DotNet.Project;
using ByteDev.DotNet.Solution;

namespace ByteDev.DotNet.SolutionViewer
{
    internal class Program
    {
        private static readonly Output Output = new Output();

        private static void Main(string[] args)
        {
            OutputHeader();

            if (args == null || args.Length == 0)
            {
                HandleError("No base path supplied as argument.");
            }

            var slnPaths = GetSlnPaths(args.First());

            if (slnPaths == null || slnPaths.Count == 0)
            {
                HandleError($"{args.First()} and its sub directories contain no solution files.");
            }

            Output.WriteLine($"{slnPaths.Count} solutions found.");
            Output.WriteBlankLines();

            foreach (var slnPath in slnPaths)
            {
                WriteSlnDetails(slnPath);
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

        private static void OutputHeader()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            Output.WriteBlankLines();

            Output.Write(new MessageBox($" SolutionViewer {fvi.FileVersion} ")
            {
                TextColor = new OutputColor(ConsoleColor.White, ConsoleColor.Blue),
                BorderColor = new OutputColor(ConsoleColor.White, ConsoleColor.Blue)
            });

            Output.WriteBlankLines();
        }

        private static void WriteSlnDetails(string slnFilePath)
        {
            var slnFile = new FileInfo(slnFilePath);

            Output.WriteLine(slnFile.Name, new OutputColor(ConsoleColor.White, ConsoleColor.Blue));
            Output.WriteBlankLines();
            Output.WriteLine($"Path: {slnFilePath}");
            Output.WriteBlankLines();

            var slnText = File.ReadAllText(slnFile.FullName);

            var dotNetSolution = new DotNetSolution(slnText);

            foreach (var slnProject in dotNetSolution.Projects.Where(p => !p.IsSolutionFolder).OrderBy(p => p.Name))
            {
                var basePath = Path.GetDirectoryName(slnFilePath);

                try
                {
                    var dotNetProject = CreateDotNetProject(basePath, slnProject.Path);

                    Output.WriteAlignToSides(slnProject.Name, dotNetProject.ProjectTargets.Single().Description);
                }
                catch (InvalidDotNetProjectException)
                {
                    Output.WriteAlignToSides(slnProject.Name, "(Unknown)", new OutputColor(ConsoleColor.Yellow));
                }
            }

            Output.WriteLine();
        }

        private static DotNetProject CreateDotNetProject(string basePath, string projectPath)
        {
            var projXml = XDocument.Load(Path.Combine(basePath, projectPath));
            return new DotNetProject(projXml);
        }

        private static void HandleError(string message)
        {
            Output.WriteLine(message, new OutputColor(ConsoleColor.Red));
            Environment.Exit(0);
        }
    }
}

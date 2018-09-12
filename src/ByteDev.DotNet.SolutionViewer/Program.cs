using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ByteDev.Cmd;
using ByteDev.DotNet.Project;
using ByteDev.DotNet.Solution;

namespace ByteDev.DotNet.SolutionViewer
{
    class Program
    {
        private static readonly Output Output = new Output();

        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Output.WriteLine("Error - no paths supplied", new OutputColor(ConsoleColor.Red));
                return;
            }

            foreach (var basePath in args)
            {
                Output.WriteLine($"Path: {basePath}");
                Output.WriteLine();

                var slnFiles = GetSolutionFiles(basePath);

                foreach (var slnFile in slnFiles)
                {
                    WriteSlnDetails(basePath, slnFile);
                }
            }
        }

        private static void WriteSlnDetails(string basePath, FileInfo slnFile)
        {
            Output.WriteLine(slnFile.Name, new OutputColor(ConsoleColor.White, ConsoleColor.Blue));

            var slnText = File.ReadAllText(slnFile.FullName);

            var dotNetSolution = new DotNetSolution(slnText);

            foreach (var slnProject in dotNetSolution.Projects.Where(p => !p.IsSolutionFolder).OrderBy(p => p.Name))
            {
                var dotNetProject = CreateDotNetProject(slnProject, basePath);

                if (dotNetProject == null)
                    Output.WriteAlignToSides(slnProject.Name, "(Unknown)", new OutputColor(ConsoleColor.Red));
                else
                    Output.WriteAlignToSides(slnProject.Name, dotNetProject.ProjectTargets.Single().Description);
            }

            Output.WriteLine();
        }

        private static DotNetProject CreateDotNetProject(DotNetSolutionProject project, string basePath)
        {
            try
            {
                var projXml = XDocument.Load(Path.Combine(basePath, project.Path));
                return new DotNetProject(projXml);
            }
            catch (InvalidDotNetProjectException)
            {
                return null;
            }
        }

        private static IEnumerable<FileInfo> GetSolutionFiles(string basePath)
        {
            var directory = new DirectoryInfo(basePath);

            return directory.GetFiles("*.sln");
        }
    }
}

using System;
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

            var slnPaths = Directory.EnumerateFiles(args.First(), "*.sln", SearchOption.AllDirectories);

            if (slnPaths == null || !slnPaths.Any())
            {
                Output.WriteLine($"{args.First()} and its sub directories contain no solution files.", new OutputColor(ConsoleColor.Red));
                return;
            }

            foreach (var slnPath in slnPaths)
            {
                Output.WriteLine($"Path: {slnPath}");
                Output.WriteLine();

                WriteSlnDetails(slnPath);
            }
        }

        private static void WriteSlnDetails(string slnFilePath)
        {
            var slnFile = new FileInfo(slnFilePath);

            Output.WriteLine(slnFile.Name, new OutputColor(ConsoleColor.White, ConsoleColor.Blue));

            var slnText = File.ReadAllText(slnFile.FullName);

            var dotNetSolution = new DotNetSolution(slnText);

            foreach (var slnProject in dotNetSolution.Projects.Where(p => !p.IsSolutionFolder).OrderBy(p => p.Name))
            {
                // HACK: to get round bug in DotNetSolution
                if (Path.GetExtension(slnProject.Path) == ".deployproj")
                {
                    Output.WriteAlignToSides(slnProject.Name, "(Unknown)", new OutputColor(ConsoleColor.Red));
                    continue;
                }

                var basePath = Path.GetDirectoryName(slnFilePath);
                var dotNetProject = CreateDotNetProject(basePath, slnProject);

                if (dotNetProject == null)
                    Output.WriteAlignToSides(slnProject.Name, "(Unknown)", new OutputColor(ConsoleColor.Red));
                else
                    Output.WriteAlignToSides(slnProject.Name, dotNetProject.ProjectTargets.Single().Description);
            }

            Output.WriteLine();
        }

        private static DotNetProject CreateDotNetProject(string basePath, DotNetSolutionProject project)
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
    }
}

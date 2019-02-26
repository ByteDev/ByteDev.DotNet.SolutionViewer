using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using ByteDev.Cmd;
using ByteDev.DotNet.Project;
using ByteDev.DotNet.Solution;

namespace ByteDev.DotNet.SolutionViewer
{
    internal static class OutputExtensions
    {
        private static readonly Guid SolutionFolderId = new Guid("2150E333-8FDC-42A3-9474-1A3956D46DE8");

        public static void WriteHeader(this Output source)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            source.WriteBlankLines();

            source.Write(new MessageBox($" SolutionViewer {fvi.FileVersion} ")
            {
                TextColor = new OutputColor(ConsoleColor.White, ConsoleColor.Blue),
                BorderColor = new OutputColor(ConsoleColor.White, ConsoleColor.Blue)
            });

            source.WriteBlankLines();
        }

        public static void WriteSlnDetails(this Output source, string slnFilePath)
        {
            var slnFile = new FileInfo(slnFilePath);

            source.WriteLine(slnFile.Name, new OutputColor(ConsoleColor.White, ConsoleColor.Blue));
            source.WriteBlankLines();
            source.WriteLine($"Path: {slnFilePath}");
            source.WriteBlankLines();

            var dotNetSolution = DotNetSolution.Load(slnFile.FullName);

            var slnProjects = dotNetSolution.Projects
                .Where(p => p.Type.Id != SolutionFolderId)
                .OrderBy(p => p.Name);

            foreach (var slnProject in slnProjects)
            {
                var basePath = Path.GetDirectoryName(slnFilePath);

                try
                {
                    var dotNetProject = DotNetProject.Load(Path.Combine(basePath, slnProject.Path));

                    source.WriteAlignToSides(slnProject.Name, dotNetProject.ProjectTargets.Single().Description);
                }
                catch (InvalidDotNetProjectException)
                {
                    source.WriteAlignToSides(slnProject.Name, "(Unknown)", new OutputColor(ConsoleColor.Yellow));
                }
            }

            source.WriteLine();
        }
    }
}
using System;
using System.Collections.Generic;
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
        public static void WriteAppHeader(this Output source)
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

        public static void WriteSlnHeader(this Output source, FileInfo slnFileInfo)
        {
            source.WriteLine(slnFileInfo.Name, new OutputColor(ConsoleColor.White, ConsoleColor.Blue));
            source.WriteBlankLines();
            source.WriteLine($"Path: {slnFileInfo.FullName}");
            source.WriteBlankLines();
        }

        public static void WriteSlnProjects(this Output source, FileInfo slnFileInfo, WriteSlnProjectsOptions options)
        {
            foreach (var slnProject in GetDotNetSolutionProjects(slnFileInfo))
            {
                var basePath = Path.GetDirectoryName(slnFileInfo.FullName);

                try
                {
                    var dotNetProject = DotNetProject.Load(Path.Combine(basePath, slnProject.Path));

                    source.WriteAlignToSides(CreateProjectNameText(slnProject, options), CreateProjectTargetText(dotNetProject));
                }
                catch (InvalidDotNetProjectException)
                {
                    source.WriteAlignToSides(slnProject.Name, "(Unknown)", new OutputColor(ConsoleColor.Yellow));
                }
            }

            source.WriteLine();
        }

        private static string CreateProjectNameText(DotNetSolutionProject slnProject, WriteSlnProjectsOptions options)
        {
            var projectText = slnProject.Name;

            if (options.WriteProjectType)
            {
                projectText += $" ({slnProject.Type.Description.Replace("(", string.Empty).Replace(")", string.Empty)})";       // Removed () cos (Unknown)
            }

            return projectText;
        }

        private static string CreateProjectTargetText(DotNetProject project)
        {
            return string.Join(',', project.ProjectTargets.Select(t => t.Description));
        }

        private static IEnumerable<DotNetSolutionProject> GetDotNetSolutionProjects(FileInfo slnFileInfo)
        {
            var dotNetSolution = DotNetSolution.Load(slnFileInfo.FullName);

            var slnProjects = dotNetSolution.Projects
                .Where(p => !p.Type.IsSolutionFolder)
                .OrderBy(p => p.Name);

            return slnProjects;
        }
    }

    internal class WriteSlnProjectsOptions
    {
        public bool WriteProjectType { get; set; }
    }
}
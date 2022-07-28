using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ByteDev.Cmd;
using ByteDev.DotNet.Project;
using ByteDev.DotNet.SolutionViewer.ModelExtensions;
using ByteDev.Strings;

namespace ByteDev.DotNet.SolutionViewer
{
    internal static class OutputExtensions
    {
        public static void WriteAppHeader(this Output source)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            source.WriteBlankLines(1);

            source.Write(new MessageBox($" SolutionViewer {fvi.FileVersion} ")
            {
                TextColor = new OutputColor(ConsoleColor.White, ConsoleColor.Blue),
                BorderColor = new OutputColor(ConsoleColor.White, ConsoleColor.Blue)
            });

            source.WriteBlankLines(1);
        }

        public static void WriteSlnHeader(this Output source, FileInfo slnFileInfo)
        {
            var projects = Io.GetDotNetSolutionProjects(slnFileInfo);

            source.WriteLine($"{slnFileInfo.Name} ({projects.Count()} projects)", new OutputColor(ConsoleColor.White, ConsoleColor.Blue));
            source.WriteBlankLines(1);
            source.WriteLine($"Path: {slnFileInfo.FullName}");
            source.WriteBlankLines(1);
        }

        public static void WriteSlnProjects(this Output source, FileInfo slnFileInfo, WriteSlnProjectsOptions options)
        {
            foreach (var slnProject in Io.GetDotNetSolutionProjects(slnFileInfo))
            {
                var basePath = Path.GetDirectoryName(slnFileInfo.FullName);
                var projFilePath = Path.Combine(basePath, slnProject.Path);

                try
                {
                    var dotNetProject = DotNetProject.Load(projFilePath);

                    string left = slnProject.ToDescriptionString(dotNetProject, options);
                    string right = dotNetProject.ToProjectTargetsString();

                    source.WriteAlignToSides(left, right, new OutputColor(ConsoleColor.Gray));
                    
                    string refText = GetReferenceText(dotNetProject, options);

                    if (!refText.IsNullOrEmpty())
                        source.WriteAlignLeft(refText);
                }
                catch (InvalidDotNetProjectException)
                {
                    source.WriteAlignToSides(slnProject.Name, "(Unknown)", new OutputColor(ConsoleColor.Yellow));
                }
                catch (Exception ex)
                {
                    source.WriteAlignToSides(slnProject.Name, $"ERROR: {ex.Message}", new OutputColor(ConsoleColor.Red));
                }
            }

            source.WriteLine();
        }

        private static string GetReferenceText(DotNetProject dotNetProject, WriteSlnProjectsOptions options)
        {
            var projectText = new StringBuilder();

            if (options.DisplayProjectReferencePaths || options.DisplayProjectReferenceNames)
            {
                foreach (var projectReference in dotNetProject.ProjectReferences)
                {
                    projectText.AppendIfNotEmpty(Environment.NewLine);

                    if (options.DisplayProjectReferenceNames)
                        projectText.Append($"-> { new FileInfo(projectReference.FilePath).Name }");
                    else
                        projectText.Append($"-> {projectReference.FilePath}");
                }
            }

            if (options.DisplayPackageReferences)
            {
                foreach (var packageReference in dotNetProject.PackageReferences)
                {
                    projectText.AppendIfNotEmpty(Environment.NewLine);
                    projectText.Append($"--> {packageReference.Name} {packageReference.Version}");
                }
            }

            return projectText.ToString();
        }
    }
}
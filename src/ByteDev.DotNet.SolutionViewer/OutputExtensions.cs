using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using ByteDev.Cmd;
using ByteDev.DotNet.Project;
using ByteDev.DotNet.SolutionViewer.ModelExtensions;

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
            foreach (var slnProject in Io.GetDotNetSolutionProjects(slnFileInfo))
            {
                var basePath = Path.GetDirectoryName(slnFileInfo.FullName);

                try
                {
                    var dotNetProject = DotNetProject.Load(Path.Combine(basePath, slnProject.Path));

                    source.WriteAlignToSides(slnProject.ToText(options), dotNetProject.ToText());
                }
                catch (InvalidDotNetProjectException)
                {
                    source.WriteAlignToSides(slnProject.Name, "(Unknown)", new OutputColor(ConsoleColor.Yellow));
                }
            }

            source.WriteLine();
        }
        
        public static void WriteSlnProjectsInTable(this Output source, FileInfo slnFileInfo, WriteSlnProjectsOptions options)
        {
            var slnProjects = Io.GetDotNetSolutionProjects(slnFileInfo);

            var count = slnProjects.Count();

            if (count < 1)
                return;

            var table = new Table(2, count)
            {
                ValueColor = new OutputColor(ConsoleColor.White, ConsoleColor.Blue)
            };

            var rowNumber = 0;

            foreach (var slnProject in slnProjects)
            {
                var basePath = Path.GetDirectoryName(slnFileInfo.FullName);

                try
                {
                    var dotNetProject = DotNetProject.Load(Path.Combine(basePath, slnProject.Path));
                    
                    table.UpdateRow(rowNumber, new []{ slnProject.ToText(options), dotNetProject.ToText() });
                }
                catch (InvalidDotNetProjectException)
                {
                    table.UpdateRow(rowNumber, new[] { slnProject.ToText(options), "(Unknown)" });
                }

                rowNumber++;
            }

            source.Write(table);
            source.WriteLine();
        }
    }
}
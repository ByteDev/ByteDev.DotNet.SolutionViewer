using System.Text;
using ByteDev.DotNet.Project;
using ByteDev.DotNet.Solution;

namespace ByteDev.DotNet.SolutionViewer.ModelExtensions
{
    public static class DotNetSolutionProjectExtensions
    {
        public static string ToDescriptionString(this DotNetSolutionProject source, DotNetProject dotNetProject, WriteSlnProjectsOptions options)
        {
            var projectText = new StringBuilder(source.Name);

            if (options.DisplayProjectType)
            {
                projectText.Append($" ({source.Type.Description.Replace("(", string.Empty).Replace(")", string.Empty)})");
            }

            return projectText.ToString();
        }
    }
}
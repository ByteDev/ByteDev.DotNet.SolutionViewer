using ByteDev.DotNet.Solution;

namespace ByteDev.DotNet.SolutionViewer.ModelExtensions
{
    public static class DotNetSolutionProjectExtensions
    {
        public static string ToText(this DotNetSolutionProject source, WriteSlnProjectsOptions options)
        {
            var projectText = source.Name;

            if (options.WriteProjectType)
            {
                projectText += $" ({source.Type.Description.Replace("(", string.Empty).Replace(")", string.Empty)})";       // Removed () cos (Unknown)
            }

            return projectText;
        }
    }
}
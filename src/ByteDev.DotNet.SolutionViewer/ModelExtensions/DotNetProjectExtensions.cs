using System.Linq;
using ByteDev.DotNet.Project;

namespace ByteDev.DotNet.SolutionViewer.ModelExtensions
{
    public static class DotNetProjectExtensions
    {
        public static string ToText(this DotNetProject source)
        {
            return string.Join(',', source.ProjectTargets.Select(t => t.Description));
        }
    }
}
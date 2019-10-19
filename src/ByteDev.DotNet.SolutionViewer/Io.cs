using System.Collections.Generic;
using System.IO;
using System.Linq;
using ByteDev.DotNet.Solution;

namespace ByteDev.DotNet.SolutionViewer
{
    public static class Io
    {
        public static IEnumerable<DotNetSolutionProject> GetDotNetSolutionProjects(FileInfo slnFileInfo)
        {
            var dotNetSolution = DotNetSolution.Load(slnFileInfo.FullName);

            var slnProjects = dotNetSolution.Projects
                .Where(p => !p.Type.IsSolutionFolder)
                .OrderBy(p => p.Name);

            return slnProjects;
        }
    }
}
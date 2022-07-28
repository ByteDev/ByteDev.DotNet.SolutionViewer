using System.Collections.Generic;
using System.Linq;
using ByteDev.Cmd.Arguments;

namespace ByteDev.DotNet.SolutionViewer
{
    internal static class CmdArgInfoExtensions
    {
        public static List<string> GetSlnsToIgnore(this CmdArgInfo source)
        {
            var cmdArg = source.GetArgument('i');

            if (cmdArg == null)
                return new List<string>();

            return cmdArg.Value.Split(',').ToList();
        }

        public static string GetPath(this CmdArgInfo source)
        {
            return source.GetArgument('p').Value;
        }

        public static bool DisplayProjectReferencePaths(this CmdArgInfo source)
        {
            return source.HasArgument("refprojpath");
        }

        public static bool DisplayProjectReferenceNames(this CmdArgInfo source)
        {
            return source.HasArgument("refprojname");
        }

        public static bool DisplayPackageReferences(this CmdArgInfo source)
        {
            return source.HasArgument("refpack");
        }
    }
}
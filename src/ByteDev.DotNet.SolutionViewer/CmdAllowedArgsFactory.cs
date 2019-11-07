using System.Collections.Generic;
using ByteDev.Cmd.Arguments;

namespace ByteDev.DotNet.SolutionViewer
{
    internal class CmdAllowedArgsFactory
    {
        public static List<CmdAllowedArg> Create()
        {
            return new List<CmdAllowedArg>
            {
                new CmdAllowedArg('p', true) {Description = "Base path to view .sln files from.", IsRequired = true},
                new CmdAllowedArg('t', false) {Description = "Output details in a table format."}
            };
        }
    }
}
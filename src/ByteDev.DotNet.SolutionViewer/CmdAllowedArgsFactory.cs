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
                new CmdAllowedArg('p', true) { Description = "Base path to view .sln files from or path to specific .sln file.", IsRequired = true, LongName = "path" },
                new CmdAllowedArg('t', false) { Description = "Output details in a table format.", LongName = "table" },
                new CmdAllowedArg('i', true) { Description = "CSV list of .sln files to ignore.", LongName = "ignoresln" }
            };
        }
    }
}
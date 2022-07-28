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
                new CmdAllowedArg('i', true) { Description = "CSV list of .sln files to ignore.", LongName = "ignoresln" },
                new CmdAllowedArg('x', false) { Description = "Display project reference dependencies (full path).", LongName = "refprojpath" },
                new CmdAllowedArg('y', false) { Description = "Display project reference dependencies (name only).", LongName = "refprojname" },
                new CmdAllowedArg('z', false) { Description = "Display package reference dependencies.", LongName = "refpack" }
            };
        }
    }
}
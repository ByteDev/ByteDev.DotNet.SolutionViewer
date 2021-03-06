﻿using System.Collections.Generic;
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
    }
}
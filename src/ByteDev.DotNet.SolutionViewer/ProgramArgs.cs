using System;
using System.Collections.Generic;
using ByteDev.Cmd.Arguments;

namespace ByteDev.DotNet.SolutionViewer
{
    internal class ProgramArgs
    {
        public string Path { get; }

        public bool UseTable { get; }

        public List<string> IgnoreSlnFiles { get; }

        public ProgramArgs(CmdArgInfo cmdArgInfo)
        {
            Path = cmdArgInfo.GetPath();

            if (string.IsNullOrEmpty(Path))
                throw new ArgumentException("No base or .sln file path supplied as argument.");
            
            UseTable = cmdArgInfo.GetUseTable();
            IgnoreSlnFiles = cmdArgInfo.GetSlnsToIgnore();
        }
    }
}
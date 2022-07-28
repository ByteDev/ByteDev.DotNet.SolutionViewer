using System;
using System.Collections.Generic;
using ByteDev.Cmd.Arguments;

namespace ByteDev.DotNet.SolutionViewer
{
    internal class ProgramArgs
    {
        public string Path { get; }

        public List<string> IgnoreSlnFiles { get; }

        public bool DisplayProjectReferencePaths { get; }

        public bool DisplayProjectReferenceNames { get; }

        public bool DisplayPackageReferences { get; }

        public ProgramArgs(CmdArgInfo cmdArgInfo)
        {
            Path = cmdArgInfo.GetPath();

            if (string.IsNullOrEmpty(Path))
                throw new ArgumentException("No base path or .sln file path supplied as argument.");
            
            IgnoreSlnFiles = cmdArgInfo.GetSlnsToIgnore();
            DisplayProjectReferencePaths = cmdArgInfo.DisplayProjectReferencePaths();
            DisplayProjectReferenceNames = cmdArgInfo.DisplayProjectReferenceNames();
            DisplayPackageReferences = cmdArgInfo.DisplayPackageReferences();
        }
    }
}
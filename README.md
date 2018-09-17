# ByteDev.DotNet.SolutionViewer

Console application that given a base path outputs a report of all the solutions in the base path, the projects in each solution and what their targets (.NET Framework, Standard, Core etc.) are.

## Installation

ByteDev.DotNet.SolutionViewer has been written to target .NET Core 2.1.

You can either run it from the command line/powershell using `dotnet run <BASE_PATH>` where `BASE_PATH` is a valid path to where a number of .sln files might be contained.  The program will search the base path directory as well as all child directories.

If you want to publish the project to an exe then a powershell script `build\publish-exe.ps1` has been included for convenience.

## Code

The repo can be cloned from git bash:

`git clone https://github.com/bytedev/ByteDev.SolutionViewer`


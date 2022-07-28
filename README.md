# ByteDev.DotNet.SolutionViewer

Console application that given a base path outputs a report of all the solutions in the base path, the projects in each solution and what their targets (.NET Framework, Standard, Core etc.) and version numbers are.

## Installation

ByteDev.DotNet.SolutionViewer has been written to target .NET Core 2.1.

If you want to publish the project to an exe then a powershell script `build\publish-exe.ps1` has been included for convenience.  This will output a `SolutionViewer.exe` file.

## Code

The repo can be cloned from git bash:

`git clone https://github.com/bytedev/ByteDev.SolutionViewer`

## Usage

Execute the `SolutionViewer.exe` with the following arguments:

| Short Argument | Long Argument | Description | Required
| --- | --- | --- | --- |
| `-p <BASE_PATH>` | `-path <BASE_PATH>` | Base path to view .sln files from. | Yes |
| `-t` | `-table` | Output details in a table format. | No |
| `-i` | `-ignoresln` | CSV list of .sln files to ignore. | No |




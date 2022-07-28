# ByteDev.DotNet.SolutionViewer

Console application that given a base path outputs a report of all the solutions (`.sln` files) in the path and details on the projects in each solution.

## Installation

ByteDev.DotNet.SolutionViewer has been written to target .NET Core 3.1.

If you want to publish the project to an exe then a powershell script `build\publish-exe.ps1` has been included for convenience.  This will output a `SolutionViewer.exe` file.

## Code

The repo can be cloned from git bash:

`git clone https://github.com/bytedev/ByteDev.SolutionViewer`

## Usage

After the project has been published execute `SolutionViewer.exe` with the following arguments:

| Short Argument | Long Argument | Description | Required
| --- | --- | --- | --- |
| `-p <BASE_PATH>` | `-path <BASE_PATH>` | Base path to view .sln files from. This can also be a path to a single .sln file. | Yes |
| `-i` | `-ignoresln` | CSV list of .sln files to ignore. | No |
| `-x` | `-refprojpath` | Display project reference dependencies (full path). | No |
| `-y` | `-refprojfile` | Display project reference dependencies (name only). | No |
| `-z` | `-refpack` | Display package reference dependencies. | No |

Or run the application without publishing from the project folder. For example:

`dotnet run -- -p C:\Git -refpack -refprojname -ignoresln libsass.sln,rubbish.sln`



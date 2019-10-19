# Publish the app as a windows x64 exe
# For list of runtime identifiers see: https://docs.microsoft.com/en-us/dotnet/core/rid-catalog
# Will output the exe to: ByteDev.DotNet.SolutionViewer\src\ByteDev.DotNet.SolutionViewer\bin\Release\netcoreapp2.1\win-x64
cd ..\src
& dotnet publish -c Release -r win-x64
# Publish the app as a windows x64 exe
# For list of runtime identifiers see: https://docs.microsoft.com/en-us/dotnet/core/rid-catalog
cd ..\src
& dotnet publish -c Release -r win-x64
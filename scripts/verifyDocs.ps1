$ErrorActionPreference = "Stop"

. ".\scripts\checkLastExitCode.ps1"

Push-Location Raven.Documentation.Cli; 

dotnet restore; 
CheckLastExitCode

dotnet run; 
CheckLastExitCode

Pop-Location

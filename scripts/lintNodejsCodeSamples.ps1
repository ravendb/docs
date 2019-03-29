
$ErrorActionPreference = "Stop"

#$env:PATH = $env:PATH + ";" + "C:\Program Files\nodejs"

. ".\scripts\checkLastExitCode.ps1"

$nodejsSamples = @(
    ".\Documentation\4.0\Samples\nodejs",
    ".\Documentation\4.1\Samples\nodejs",
    ".\Documentation\4.2\Samples\nodejs"
);

npm install
CheckLastExitCode

foreach ($samplesDir in $nodejsSamples) {
    if (Test-Path $samplesDir) {
        Write-Host "Checking $samplesDir"
        .\node_modules\.bin\eslint $samplesDir
        CheckLastExitCode
        Write-Host "OK $samplesDir"
    }
}

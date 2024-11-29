param (
    [string] $FromVersion = "6.2",
    [string] $ToVersion = "7.0"
)

$ErrorActionPreference = "Stop"

Write-Host $PSScriptRoot
$global:sourceFolder = ([io.path]::combine($PSScriptRoot, "../Documentation/$FromVersion/Raven.Documentation.Pages"))
$global:destFolder = ([io.path]::combine($PSScriptRoot, "../Documentation/$ToVersion/Raven.Documentation.Pages"))

function Get-RelativeSourcePath($path) {
    Push-Location $global:sourceFolder
    $relativePath = Resolve-Path -Path $path -Relative
    Pop-Location

    return $relativePath;
}

function Create-DestinationFolder($relativePath) {
    $destSubfolder = ([io.path]::combine($global:destFolder, $relativePath))

    if (Test-Path -Path $destSubfolder) {
        return;
    }

    Write-Host "Creating directory: $destSubfolder"
    New-Item -ItemType Directory -Path $destSubfolder
}

function Traverse-Dir($path) {
    $dirRelativePath = Get-RelativeSourcePath $path
    Create-DestinationFolder $dirRelativePath

    $docsJsonPath = [io.path]::combine($path, ".docs.json");

    if (Test-Path $docsJsonPath) {
        $copyToFolder = ([io.path]::combine($global:destFolder, $dirRelativePath))
        Write-Host "Copying .docs.json FROM $docsJsonPath TO $copyToFolder"
        Copy-Item -Path $docsJsonPath -Destination $copyToFolder
    }

    $nestedFolders = Get-ChildItem $path -Directory;

    foreach ($nestedFolder in $nestedFolders) {
        $nestedPath = [io.path]::combine($path, $nestedFolder);
        Traverse-Dir $nestedPath;
    }

    return;
}

# $sourceDocsJsonFiles = Get-ChildItem -Path $sourceFolder -Filter ".docs.json" -Recurse

try {
    Traverse-Dir $global:sourceFolder
}
finally {
    Pop-Location
}

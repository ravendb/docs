param (
    [array] $LanguagesToCheck = @("dotnet", "java", "js")
)

$ErrorActionPreference = "Stop"

$excludedVersions = @("1.0", "2.0", "2.5", "3.0", "3.5")

$validationFailed = $false;

function Get-MarkdownKey($file) {
    return $file.name.Split(".")[0];
}

function Get-MarkdownLanguage($file) {
    $split = $file.name.Split(".");
    if ( $split.length -eq 2) {
        return "All";
    }
    return $split[1];
}

function Validate-Markdown($version, $key, $languages, $folder) {
    if ($languages.length -eq 1 -and ($languages[0] -eq "All")) {
        return;
    }

    foreach ($expectedLang in $LanguagesToCheck) {
        if (-not ($languages -contains $expectedLang)) {
            $global:validationFailed = $true;
            Write-Host "[$version $expectedLang] '$key' in '$folder'"
        }
    }
}

function Validate-Markdowns($version, $dict, $folder) {
    foreach ($item in $dict.GetEnumerator()) {
        Validate-Markdown $version $item.Name $item.Value $folder;
    }
}

function Traverse-Pages($version, $path) {
    $markdownFiles = Get-ChildItem $path | Where { $_.Extension -eq ".markdown" };

    $dict = @{};

    foreach ($markdown in $markdownFiles) {
        $key = Get-MarkdownKey $markdown
        $lang = Get-MarkdownLanguage $markdown

        if ($dict.ContainsKey($key)) {
            $dict[$key] += $lang;
        }
        else {
            $dict[$key] = @($lang);
        }
    }

    Validate-Markdowns $version $dict $path;

    $nestedFolders = Get-ChildItem $path -Directory;

    foreach ($nestedFolder in $nestedFolders) {
        $nestedPath = [io.path]::combine($path, $nestedFolder);
        Traverse-Pages $version $nestedPath;
    }

    return;
}

function Traverse-Documentation($versionsToCheck) {
    foreach ($version in $versionsToCheck) {
        $path = [io.path]::combine(".", $version, "Raven.Documentation.Pages");
        Traverse-Pages $version $path;
    }

    if ($global:validationFailed) {
        Write-Error "Validation failed"
    }
    else {
        Write-Host "All OK"
    }
}

function Get-VersionsToCheck() {
    $allVersions = Get-ChildItem -Directory | Select-Object -ExpandProperty Name

    return $allVersions | Where-Object {$excludedVersions.Contains($_) -eq $false}
}

Write-Host "Missing markdown files:"
Push-Location ([io.path]::combine($PSScriptRoot, "../Documentation"))

$versionsToCheck = Get-VersionsToCheck

try {
    Traverse-Documentation $versionsToCheck
}
finally {
    Pop-Location
}
param (
    [string] $Version = "4.0",
    [array] $LanguagesToCheck = @("dotnet", "java", "js")
)

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

function Validate-Markdown($key, $languages, $folder) {
    if ($languages.length -eq 1 -and ($languages[0] -eq "All")) {
        return;
    }

    foreach ($expectedLang in $LanguagesToCheck) {
        if (-not ($languages -contains $expectedLang)) {
            $validationFailed = $true;
            Write-Host "Missing $expectedLang for $key in $folder"
        }
    }
}

function Validate-Markdowns( $dict, $folder ) {
    foreach ($item in $dict.GetEnumerator()) {
        Validate-Markdown $item.Name $item.Value $folder;
    }
}

function Traverse-Pages($path) {
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

    Validate-Markdowns $dict $path;

    $nestedFolders = Get-ChildItem $path -Directory;

    foreach ($nestedFolder in $nestedFolders) {
        $nestedPath = [io.path]::combine($path, $nestedFolder);
        Traverse-Pages $nestedPath;
    }

    return;
}

function Traverse-Documentation() {
    $path = [io.path]::combine(".", $Version, "Raven.Documentation.Pages");
    Traverse-Pages $path;

    if ($validationFailed) {
        Write-Error "Validation failed"
    }
    else {
        Write-Host "All OK"
    }
}

Write-Host $PSScriptRoot
Push-Location ([io.path]::combine($PSScriptRoot, "../Documentation"))
try {
    Traverse-Documentation
}
finally {
    Pop-Location
}
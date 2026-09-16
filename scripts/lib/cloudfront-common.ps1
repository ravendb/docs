# Shared by sync-csp.ps1 and sync-edge-function.ps1.

# CloudFront limits: CSP header value, and function code size.
$script:MaxCspBytes = 1783
$script:MaxFunctionBytes = 10240

function Assert-CloudFrontPrerequisites {
    foreach ($cmd in 'aws') {
        if (-not (Get-Command $cmd -ErrorAction SilentlyContinue)) { throw "$cmd not found in PATH" }
    }
    foreach ($name in 'AWS_ACCESS_KEY_ID', 'AWS_SECRET_ACCESS_KEY', 'AWS_DEFAULT_REGION') {
        if (-not (Get-Item -Path Env:$name -ErrorAction SilentlyContinue).Value) { throw "$name not set" }
    }
}

# Nested config shapes (KeyValueStoreAssociations, SecurityHeadersConfig) do not survive CLI
# shorthand, so they are passed by file reference. Caller removes the file.
function New-CloudFrontArgumentFile {
    param(
        [Parameter(Mandatory)] [string]$Content,
        [string]$Extension = 'json'
    )
    $path = Join-Path ([IO.Path]::GetTempPath()) "cf-arg-$([guid]::NewGuid()).$Extension"
    Set-Content -Path $path -Value $Content -Encoding utf8 -NoNewline
    return $path
}

# Config is applied per request, but a response already in the edge cache keeps the headers and the
# redirect it was stored with. deploy.ps1 runs its own single invalidation and skips this.
function Invoke-CloudFrontInvalidation {
    param([Parameter(Mandatory)] [string]$DistributionId)

    Write-Host "Invalidating CloudFront distribution $DistributionId" -ForegroundColor Cyan
    aws cloudfront create-invalidation --distribution-id $DistributionId --paths '/*' | Out-Null
    if ($LASTEXITCODE) { throw 'CloudFront invalidation failed' }
}

# Paths where $Candidate differs from $Reference: node kind, array length, added or dropped
# property, or scalar value. Proves a patch touched only the field it meant to.
function Get-JsonDifference {
    param(
        [AllowNull()] $Reference,
        [AllowNull()] $Candidate,
        [string]$Path = ''
    )

    $here = if ($Path) { $Path } else { '(root)' }

    $refNull = $null -eq $Reference
    $canNull = $null -eq $Candidate
    if ($refNull -or $canNull) {
        if ($refNull -ne $canNull) { return @("$here : $(if ($refNull) { 'added' } else { 'removed' })") }
        return @()
    }

    $refArr = $Reference -is [System.Collections.IEnumerable] -and $Reference -isnot [string]
    $canArr = $Candidate -is [System.Collections.IEnumerable] -and $Candidate -isnot [string]
    if ($refArr -ne $canArr) { return @("$here : kind changed (array vs scalar/object)") }

    if ($refArr) {
        $r = @($Reference); $c = @($Candidate)
        if ($r.Count -ne $c.Count) { return @("$here : array length $($r.Count) -> $($c.Count)") }
        $out = @()
        for ($i = 0; $i -lt $r.Count; $i++) {
            $out += Get-JsonDifference -Reference $r[$i] -Candidate $c[$i] -Path "$here[$i]"
        }
        return $out
    }

    $refObj = $Reference -is [System.Management.Automation.PSCustomObject]
    $canObj = $Candidate -is [System.Management.Automation.PSCustomObject]
    if ($refObj -ne $canObj) { return @("$here : kind changed (object vs scalar)") }

    if ($refObj) {
        $out = @()
        $names = @($Reference.PSObject.Properties.Name) + @($Candidate.PSObject.Properties.Name) | Select-Object -Unique
        foreach ($n in $names) {
            $childPath = if ($Path) { "$Path.$n" } else { $n }
            $out += Get-JsonDifference -Reference $Reference.$n -Candidate $Candidate.$n -Path $childPath
        }
        return $out
    }

    if ("$Reference" -cne "$Candidate") { return @("$here : value changed") }
    return @()
}

# True when live already matches the repo. Line endings normalised so a CRLF checkout is not drift.
function Compare-CloudFrontValue {
    param(
        [AllowNull()] [string]$Live,
        [Parameter(Mandatory)] [string]$Local,
        [string]$Noun = 'value'
    )

    $normalise = { param($s) if ($null -eq $s) { '' } else { ($s -replace "`r`n", "`n").Trim() } }
    if ((& $normalise $Live) -eq (& $normalise $Local)) {
        Write-Host "CloudFront already serves this $Noun. Nothing to do." -ForegroundColor Green
        return $true
    }

    if ($null -eq $Live) {
        Write-Host '  live: nothing published yet' -ForegroundColor DarkGray
    } elseif ($Live.Length -lt 400 -and $Local.Length -lt 400) {
        Write-Host "  live: $Live" -ForegroundColor DarkGray
        Write-Host "  repo: $Local" -ForegroundColor Gray
    } else {
        Write-Host "  live: $($Live.Length) chars" -ForegroundColor DarkGray
        Write-Host "  repo: $($Local.Length) chars" -ForegroundColor Gray
    }
    return $false
}

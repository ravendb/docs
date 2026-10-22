# Pushes the Algolia crawler config and index settings from scripts/lib when they differ; -Check reports drift.
#   pwsh scripts/sync-algolia.ps1 [-CrawlerId <id>] [-Check] [-DryRun]
# Env: ALGOLIA_CRAWLER_ID/_USER_ID/_API_KEY (crawler), ALGOLIA_API_KEY with editSettings (settings); unset = skipped.

[CmdletBinding()]
param(
    [Parameter(HelpMessage = 'Algolia Crawler ID. Defaults to $env:ALGOLIA_CRAWLER_ID.')]
    [string]$CrawlerId = $env:ALGOLIA_CRAWLER_ID,

    [Parameter(HelpMessage = 'Report drift and exit non-zero. Writes nothing.')]
    [switch]$Check,

    [Parameter(HelpMessage = 'Print the diff without writing.')]
    [switch]$DryRun
)

$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'lib/cloudfront-common.ps1')

# node prints non-ASCII (the breadcrumb separator).
[Console]::OutputEncoding = [Text.UTF8Encoding]::new()

$CrawlerConfigPath = Join-Path $PSScriptRoot 'lib/algolia-crawler-config.js'
$IndexSettingsPath = Join-Path $PSScriptRoot 'lib/algolia-index-settings.json'

$crawlerJson = (& node $CrawlerConfigPath | Out-String)
if ($LASTEXITCODE -or -not $crawlerJson) { throw "Failed to render $CrawlerConfigPath" }
$crawler = $crawlerJson | ConvertFrom-Json
$settingsJson = Get-Content -Path $IndexSettingsPath -Raw
$settings = $settingsJson | ConvertFrom-Json

$appId = $crawler.appId
$indexName = $crawler.actions[0].indexName

# Diff over the repo's keys; a push cannot remove live-only keys (both APIs merge), so those only warn.
function Get-ManagedDrift {
    param($Live, $Local, [string]$Noun, [string[]]$Unmanaged = @())

    $localNames = @($Local.PSObject.Properties.Name)
    $liveOnly = @($Live.PSObject.Properties.Name | Where-Object { $_ -notin $localNames -and $_ -notin $Unmanaged })
    if ($liveOnly.Count) {
        Write-Warning "Live $Noun keys missing from the repo: $($liveOnly -join ', '). Add them to the repo copy."
    }

    $managed = [ordered]@{}
    foreach ($name in $localNames) { $managed[$name] = $Live.$name }
    return @(Get-JsonDifference -Reference ([pscustomobject]$managed) -Candidate $Local)
}

function Test-ShouldPush {
    param([string[]]$Drift, [string]$Noun, [string]$SourcePath)

    if (-not $Drift.Count) {
        Write-Host "  $Noun`: in sync with the repo. Nothing to do." -ForegroundColor Green
        return $false
    }
    foreach ($d in $Drift) { Write-Host "  $d" -ForegroundColor Gray }

    if ($Check) {
        Write-Host "  $Noun`: live differs from $SourcePath. Run without -Check to push it." -ForegroundColor Red
        $script:driftFound = $true
        return $false
    }
    if ($DryRun) {
        Write-Host "  Dry run. Skipping the $Noun update." -ForegroundColor Yellow
        return $false
    }
    return $true
}

$driftFound = $false

if ($CrawlerId -and $env:ALGOLIA_CRAWLER_USER_ID -and $env:ALGOLIA_CRAWLER_API_KEY) {
    $crawlerUri = "https://crawler.algolia.com/api/1/crawlers/$CrawlerId"
    $pair = "$($env:ALGOLIA_CRAWLER_USER_ID):$($env:ALGOLIA_CRAWLER_API_KEY)"
    $crawlerHeaders = @{ Authorization = 'Basic ' + [Convert]::ToBase64String([Text.Encoding]::UTF8.GetBytes($pair)) }
    # The crawler's apiKey is not in the repo.
    $getCrawlerDrift = {
        $live = (Invoke-RestMethod -Uri "${crawlerUri}?withConfig=true" -Headers $crawlerHeaders).config
        Get-ManagedDrift -Live $live -Local $crawler -Noun 'crawler config' -Unmanaged 'apiKey'
    }

    Write-Host "Reading crawler $CrawlerId" -ForegroundColor Cyan
    if (Test-ShouldPush -Drift (& $getCrawlerDrift) -Noun 'crawler config' -SourcePath $CrawlerConfigPath) {
        Write-Host '  Updating the crawler config' -ForegroundColor Cyan
        Invoke-RestMethod -Uri "$crawlerUri/config" -Headers $crawlerHeaders -Method Patch `
            -Body $crawlerJson -ContentType 'application/json; charset=utf-8' | Out-Null

        # Fail if the API rewrote anything, or every deploy would recrawl.
        $left = @(& $getCrawlerDrift)
        if ($left.Count) { throw "The crawler config does not read back as written: $($left -join '; ')" }

        Write-Host '  Starting a recrawl' -ForegroundColor Cyan
        Invoke-RestMethod -Uri "$crawlerUri/reindex" -Headers $crawlerHeaders -Method Post | Out-Null
        Write-Host 'Crawler config updated, recrawl started.' -ForegroundColor Green
    }
} else {
    Write-Host '  crawler: skipped, no ALGOLIA_CRAWLER_ID / ALGOLIA_CRAWLER_USER_ID / ALGOLIA_CRAWLER_API_KEY' -ForegroundColor Yellow
}

if ($env:ALGOLIA_API_KEY) {
    $settingsRequest = @{
        Uri     = "https://$appId.algolia.net/1/indexes/$indexName/settings"
        Headers = @{ 'X-Algolia-Application-Id' = $appId; 'X-Algolia-API-Key' = $env:ALGOLIA_API_KEY }
    }

    Write-Host "Reading index settings for $indexName" -ForegroundColor Cyan
    $live = Invoke-RestMethod @settingsRequest
    # `version` is the response format, not a setting.
    $drift = Get-ManagedDrift -Live $live -Local $settings -Noun 'index settings' -Unmanaged 'version'

    if (Test-ShouldPush -Drift $drift -Noun 'index settings' -SourcePath $IndexSettingsPath) {
        Invoke-RestMethod @settingsRequest -Method Put -Body $settingsJson -ContentType 'application/json; charset=utf-8' | Out-Null
        Write-Host 'Index settings updated.' -ForegroundColor Green
    }
} else {
    Write-Host '  index settings: skipped, no ALGOLIA_API_KEY' -ForegroundColor Yellow
}

if ($driftFound) { exit 1 }

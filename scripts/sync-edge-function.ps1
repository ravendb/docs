# Publishes scripts/handle_redirects.js to the CloudFront function that runs on viewer-request.
# Deploying the site does not touch it, so an edit there has no effect until this runs: the site
# deploys cleanly while the edge keeps behaving like the previous release. Called by deploy.ps1
# phase 3; -Check reports drift without writing, for CI.
#
#   pwsh scripts/sync-edge-function.ps1 -FunctionName <function> [-Check] [-DryRun]

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true, HelpMessage = 'CloudFront function name')]
    [string]$FunctionName,

    [Parameter(HelpMessage = 'CloudFront distribution ID to invalidate (optional)')]
    [string]$CloudFrontDistributionId,

    [Parameter(HelpMessage = 'Report drift and exit non-zero. Writes nothing.')]
    [switch]$Check,

    [Parameter(HelpMessage = 'Print the diff without writing.')]
    [switch]$DryRun
)

$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'lib/cloudfront-common.ps1')

$FunctionSourcePath = Join-Path $PSScriptRoot 'handle_redirects.js'
if (-not (Test-Path $FunctionSourcePath)) { throw "Function source not found: $FunctionSourcePath" }

Assert-CloudFrontPrerequisites

$localBytes = (Get-Item $FunctionSourcePath).Length
if ($localBytes -gt $script:MaxFunctionBytes) {
    throw "handle_redirects.js is $localBytes bytes; CloudFront caps function code at $script:MaxFunctionBytes"
}

Write-Host "Reading function $FunctionName" -ForegroundColor Cyan
$described = aws cloudfront describe-function --name $FunctionName --stage DEVELOPMENT | ConvertFrom-Json
if ($LASTEXITCODE) { throw 'aws describe-function failed' }

$etag = $described.ETag
# update-function replaces the whole FunctionConfig, so it is re-sent verbatim. Dropping
# KeyValueStoreAssociations would leave cf.kvs() unbound and break every redirect at runtime.
$config = $described.FunctionSummary.FunctionConfig

$livePath = Join-Path ([IO.Path]::GetTempPath()) "cf-live-fn-$([guid]::NewGuid()).js"
try {
    aws cloudfront get-function --name $FunctionName --stage LIVE $livePath | Out-Null
    $liveCode = if ($LASTEXITCODE -eq 0) { Get-Content -Path $livePath -Raw } else { $null }
} finally {
    Remove-Item -Path $livePath -Force -ErrorAction SilentlyContinue
}

$localCode = Get-Content -Path $FunctionSourcePath -Raw
if (Compare-CloudFrontValue -Live $liveCode -Local $localCode -Noun 'function') { return }

if ($Check) {
    Write-Error "The live function does not match $FunctionSourcePath. Run without -Check to publish it."
    exit 1
}
if ($DryRun) {
    Write-Host 'Dry run. Skipping the update and publish.' -ForegroundColor Yellow
    return
}

# fileb:// because the code is sent as a binary blob.
$configPath = New-CloudFrontArgumentFile -Content ($config | ConvertTo-Json -Depth 30 -Compress)
try {
    Write-Host 'Updating the function (DEVELOPMENT stage)' -ForegroundColor Cyan
    $newEtag = aws cloudfront update-function `
        --name $FunctionName `
        --if-match $etag `
        --function-code "fileb://$FunctionSourcePath" `
        --function-config "file://$configPath" `
        --query 'ETag' --output text
    if ($LASTEXITCODE -or -not $newEtag) { throw 'aws update-function failed' }

    Write-Host 'Publishing to LIVE' -ForegroundColor Cyan
    aws cloudfront publish-function --name $FunctionName --if-match $newEtag | Out-Null
    if ($LASTEXITCODE) { throw 'aws publish-function failed' }
} finally {
    Remove-Item -Path $configPath -Force -ErrorAction SilentlyContinue
}

if ($CloudFrontDistributionId) { Invoke-CloudFrontInvalidation -DistributionId $CloudFrontDistributionId }

Write-Host 'Edge function published.' -ForegroundColor Green

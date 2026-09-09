# Writes scripts/lib/csp-policy.js into the CloudFront response headers policy that serves the CSP
# for docs.ravendb.net. Deploying the site cannot change it. Called by deploy.ps1 phase 3;
# -Check reports drift without writing, for CI, and -Print renders the header without AWS access.
#
#   pwsh scripts/sync-csp.ps1 -ResponseHeadersPolicyId <id> [-Check] [-DryRun]
#   pwsh scripts/sync-csp.ps1 -Print

[CmdletBinding()]
param(
    [Parameter(HelpMessage = 'CloudFront response headers policy ID. Required unless -Print.')]
    [string]$ResponseHeadersPolicyId,

    [Parameter(HelpMessage = 'CloudFront distribution ID to invalidate (optional)')]
    [string]$CloudFrontDistributionId,

    [Parameter(HelpMessage = 'Report drift and exit non-zero. Writes nothing.')]
    [switch]$Check,

    [Parameter(HelpMessage = 'Print the header built from csp-policy.js and exit.')]
    [switch]$Print,

    [Parameter(HelpMessage = 'Print the diff without writing.')]
    [switch]$DryRun
)

$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'lib/cloudfront-common.ps1')

$CspPolicyPath = Join-Path $PSScriptRoot 'lib/csp-policy.js'
if (-not (Test-Path $CspPolicyPath)) { throw "CSP source not found: $CspPolicyPath" }
if (-not (Get-Command 'node' -ErrorAction SilentlyContinue)) { throw 'node not found in PATH' }

$csp = (& node $CspPolicyPath | Out-String).Trim()
if ($LASTEXITCODE -or -not $csp) { throw "Failed to render the CSP from $CspPolicyPath" }
if ($csp -notmatch '^[a-z-]+ ') { throw "$CspPolicyPath did not print a directive" }
if ($csp.Length -gt $script:MaxCspBytes) {
    throw "CSP is $($csp.Length) chars; CloudFront caps it at $script:MaxCspBytes"
}

if ($Print) {
    Write-Output $csp
    return
}
if (-not $ResponseHeadersPolicyId) { throw 'ResponseHeadersPolicyId is required unless -Print is used' }

Assert-CloudFrontPrerequisites

Write-Host "Reading response headers policy $ResponseHeadersPolicyId" -ForegroundColor Cyan
$current = aws cloudfront get-response-headers-policy --id $ResponseHeadersPolicyId | ConvertFrom-Json
if ($LASTEXITCODE) { throw 'aws get-response-headers-policy failed' }

$etag = $current.ETag
$config = $current.ResponseHeadersPolicy.ResponseHeadersPolicyConfig

if (Compare-CloudFrontValue -Live $config.SecurityHeadersConfig.ContentSecurityPolicy.ContentSecurityPolicy -Local $csp -Noun 'policy') { return }

if ($Check) {
    Write-Error "The live CSP does not match $CspFilePath. Run without -Check to push it."
    exit 1
}

# update-response-headers-policy replaces the whole config, and this policy also carries CORS with
# a per-environment origin plus HSTS at a one-year max-age. So the patch is proved to touch one
# field before writing, and the policy is read back after. Losing HSTS here would not surface until
# someone reported a broken page.
$CspPath = 'SecurityHeadersConfig.ContentSecurityPolicy'
$before = $config | ConvertTo-Json -Depth 30 -Compress | ConvertFrom-Json

if (-not $config.SecurityHeadersConfig) {
    $config | Add-Member -NotePropertyName SecurityHeadersConfig -NotePropertyValue ([pscustomobject]@{}) -Force
}
$config.SecurityHeadersConfig | Add-Member -NotePropertyName ContentSecurityPolicy -NotePropertyValue ([pscustomobject]@{
        Override              = $true
        ContentSecurityPolicy = $csp
    }) -Force

$payload = $config | ConvertTo-Json -Depth 30 -Compress
$diffs = @(Get-JsonDifference -Reference $before -Candidate ($payload | ConvertFrom-Json))
$unexpected = @($diffs | Where-Object { $_ -notlike "$CspPath*" })

foreach ($d in $diffs) {
    Write-Host "  $d" -ForegroundColor $(if ($d -like "$CspPath*") { 'Gray' } else { 'Red' })
}
if ($unexpected.Count) {
    throw "Refusing to write: $($unexpected.Count) field(s) outside the CSP would change. $($unexpected -join '; ')"
}

if ($DryRun) {
    Write-Host 'Dry run. Skipping the CloudFront update.' -ForegroundColor Yellow
    return
}

$configPath = New-CloudFrontArgumentFile -Content $payload
try {
    Write-Host 'Updating the response headers policy' -ForegroundColor Cyan
    aws cloudfront update-response-headers-policy `
        --id $ResponseHeadersPolicyId `
        --if-match $etag `
        --response-headers-policy-config "file://$configPath" | Out-Null
    if ($LASTEXITCODE) { throw 'aws update-response-headers-policy failed' }
} finally {
    Remove-Item -Path $configPath -Force -ErrorAction SilentlyContinue
}

$reread = aws cloudfront get-response-headers-policy --id $ResponseHeadersPolicyId | ConvertFrom-Json
if ($LASTEXITCODE) { throw 'aws get-response-headers-policy failed on re-read' }

$now = $reread.ResponseHeadersPolicy.ResponseHeadersPolicyConfig | ConvertTo-Json -Depth 30 -Compress | ConvertFrom-Json
$postDiffs = @(Get-JsonDifference -Reference $before -Candidate $now | Where-Object { $_ -notlike "$CspPath*" })
if ($postDiffs.Count) {
    throw "The policy changed outside the CSP: $($postDiffs -join '; '). Restore the affected headers from the CloudFront console."
}
if ($now.SecurityHeadersConfig.ContentSecurityPolicy.ContentSecurityPolicy -cne $csp) {
    throw 'The CSP does not read back as written.'
}

if ($CloudFrontDistributionId) { Invoke-CloudFrontInvalidation -DistributionId $CloudFrontDistributionId }

Write-Host 'CSP updated.' -ForegroundColor Green

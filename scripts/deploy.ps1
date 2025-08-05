<#! -----------------------------------------------------------------------------
Deploy.ps1 – RavenDB Docs Deployment
-----------------------------------
Deploys the Docusaurus static site to an AWS S3 bucket and optionally
invalidates a CloudFront distribution. If the **-Versions** switch is used, the
script first regenerates the *What's New* Markdown for those RavenDB **versions**
(via `build_whats_new.py`) before building and publishing the site.

Prerequisites
-------------
* **Node.js ≥ 18** (with npm)
* **Python ≥ 3** (`python` in PATH) – runs the changelog generator
* **AWS CLI v2** – credentials via `AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY`,
  `AWS_DEFAULT_REGION` (and optional `AWS_SESSION_TOKEN`)
* Environment variable **`WHATS_NEW_URL`** set (only required when regenerating
  *What's New* for specific versions)
* Project `package.json` includes `@docusaurus/core` and `@docusaurus/cli`

Example
-------
```powershell
# WHATS_NEW_URL must be set if you request specific versions
$env:WHATS_NEW_URL = 'https://whats.new.api/v1/docs'

pwsh deploy.ps1 \
     -S3BucketName my-docs-bucket \
     -CloudFrontDistributionId ABCD1234 \
     -Versions "6.0,6.2,7.0,7.1,8.0"
```
------------------------------------------------------------------------------!#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true, HelpMessage = 'Target S3 bucket name')]
    [string]$S3BucketName,

    [Parameter(HelpMessage = 'CloudFront distribution ID to invalidate (optional)')]
    [string]$CloudFrontDistributionId,

    [Parameter(HelpMessage = "Comma-separated versions to regenerate Whats New for e.g. '6.0,6.2,7.0'")]
    [string]$Versions = "",

    [Parameter(HelpMessage = 'Dry run mode. If enabled, no sync to S3 or CloudFront invalidation will occur.')]
    [switch]$DryRun
)

$PythonWhatsNewPath = Join-Path $PSScriptRoot 'build_whats_new.py' 

function ThrowIfEmpty {
    param (
        [string]$Value,
        [string]$Message
    )
    if (-not $Value) { throw $Message }
}

function Ensure-Dependencies {
    Write-Host 'Verifying runtime dependencies...' -ForegroundColor Cyan

    foreach ($cmd in 'node','npm','python','aws') {
        if (-not (Get-Command $cmd -ErrorAction SilentlyContinue)) {
            throw "$cmd not found in PATH"
        }
    }

    $nodeVer   = (node -v).Trim()
    $npmVer    = (npm --version).Trim()
    $pythonVer = (python --version 2>&1).Trim()
    $awsVer    = (aws --version).Trim()

    Write-Host "Node.js $nodeVer | npm $npmVer | Python $pythonVer | AWS $awsVer" -ForegroundColor Gray
}

function Process-Changelogs {
    param (
        [string[]]$WhatsNewVersions
    )

    if (-not $WhatsNewVersions) { return }

    ThrowIfEmpty $Env:WHATS_NEW_URL 'WHATS_NEW_URL env var must be set when specifying versions'

    $VersionsArray = $WhatsNewVersions.Split(',') | ForEach-Object { $_.Trim() } | Where-Object { $_ }
    Write-Host "Generating *What's New* pages for versions: $WhatsNewVersions" -ForegroundColor Cyan

    & python $PythonWhatsNewPath @VersionsArray
    if ($LASTEXITCODE) { throw 'build_whats_new.py failed' }
}

Ensure-Dependencies

ThrowIfEmpty $Env:AWS_ACCESS_KEY_ID      'AWS_ACCESS_KEY_ID not set'
ThrowIfEmpty $Env:AWS_SECRET_ACCESS_KEY 'AWS_SECRET_ACCESS_KEY not set'
ThrowIfEmpty $Env:AWS_DEFAULT_REGION    'AWS_DEFAULT_REGION not set'

Write-Host "Region: '$Env:AWS_DEFAULT_REGION' | Bucket: '$S3BucketName'" -ForegroundColor Cyan

if ($Versions) {
    ThrowIfEmpty $Env:WHATS_NEW_URL 'WHATS_NEW_URL env var must be set when specifying versions'
}

Process-Changelogs -VersionsToBuild $Versions

Write-Host 'Installing JS dependencies (npm ci)...' -ForegroundColor Cyan
$env:NODE_OPTIONS = '--max-old-space-size=8192'

if (-not (Test-Path package.json)) { throw 'package.json not found' }

npm ci --no-audit --fund false
if ($LASTEXITCODE) { throw 'npm ci failed' }

Write-Host "Running 'npx docusaurus build'..." -ForegroundColor Gray
npx docusaurus build
if ($LASTEXITCODE) { throw 'Docusaurus build failed' }

$BuildDir = [IO.Path]::Combine($PSScriptRoot, '..', 'build')
if (-not (Test-Path $BuildDir)) { throw "Build folder not produced ($BuildDir)" }

if ($DryRun) {
    Write-Host "Dry run mode enabled. Skipping sync to s3://$S3BucketName/ and CloudFront invalidation." -ForegroundColor Yellow
} else {

    Write-Host "Syncing to s3://$S3BucketName/ ..." -ForegroundColor Cyan
    aws s3 sync $BuildDir "s3://$S3BucketName/" --delete
    if ($LASTEXITCODE) { throw 'aws s3 sync failed' }

    if ($CloudFrontDistributionId) {
        Write-Host "Invalidating CloudFront distribution $CloudFrontDistributionId" -ForegroundColor Cyan
        aws cloudfront create-invalidation --distribution-id $CloudFrontDistributionId --paths '/*' | Out-Null
        if ($LASTEXITCODE) { throw 'CloudFront invalidation failed' }
    }

}

Write-Host 'Deployment completed successfully.' -ForegroundColor Green
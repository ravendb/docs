<#! -----------------------------------------------------------------------------
TeamCity PowerShell Build Script
--------------------------------
Deploys a Docusaurus static site with RavenDB changelogs to AWS S3 and
invalidates CloudFront cache.

Prerequisites
-------------
* **Node.js ≥ 18** (with npm)
* **Python ≥ 3** (`python` in PATH) – used by the changelog patcher
* **AWS CLI v2** – credentials via `AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY`,
  `AWS_DEFAULT_REGION` (and optional `AWS_SESSION_TOKEN`)
* Environment variable **`RAVENDB_API_KEY`** set (only required when changelogs
  for specific RavenDB versions are processed)
* Project `package.json` includes `@docusaurus/core` and `@docusaurus/cli`

Example
-------
```powershell
# RAVENDB_API_KEY must be set if you request specific versions
$env:RAVENDB_API_KEY = 'YOUR-API-KEY'

pwsh teamcity_ravendb_deploy.ps1 -S3BucketName my-docs-bucket `
     -CloudFrontDistributionId ABCD1234 -6.0 -7.0
```
------------------------------------------------------------------------------!#>

[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)]
    [string]$S3BucketName,

    [string]$CloudFrontDistributionId,
    [string[]]$Versions = @(),

    # Convenience switches; dot aliases allow literal "-6.0" etc.
    [Alias('6.0')][switch]$v6_0,
    [Alias('6.2')][switch]$v6_2,
    [Alias('7.0')][switch]$v7_0,
    [Alias('7.1')][switch]$v7_1
)

# ---------------------------------------------------------------------------
# Constants
# ---------------------------------------------------------------------------
$PythonPatcherPath = Join-Path $PSScriptRoot 'scripts/patch_changelogs.py'

function ThrowIfEmpty {
    param([string]$Value, [string]$Message)
    if (-not $Value) { throw $Message }
}

# ---------------------------------------------------------------------------
# 0. Verify runtime dependencies
# ---------------------------------------------------------------------------
function Ensure-Dependencies {
    Write-Host "Verifying runtime dependencies…" -ForegroundColor Cyan

    if (-not (Get-Command node   -ErrorAction SilentlyContinue)) { throw "Node.js not found in PATH" }
    if (-not (Get-Command npm    -ErrorAction SilentlyContinue)) { throw "npm CLI not found in PATH" }
    if (-not (Get-Command python -ErrorAction SilentlyContinue)) { throw "Python not found in PATH" }
    if (-not (Get-Command aws    -ErrorAction SilentlyContinue)) { throw "AWS CLI v2 not found in PATH" }

    $nodeVer   = (node -v).Trim()
    $npmVer    = (npm --version).Trim()
    $pythonVer = (python --version 2>&1).Trim()
    $awsVer    = (aws --version).Trim()

    Write-Host "Node.js $nodeVer | npm $npmVer | Python $pythonVer | AWS $awsVer" -ForegroundColor Gray
}

Ensure-Dependencies

# ---------------------------------------------------------------------------
# 1. Validate AWS environment
# ---------------------------------------------------------------------------
ThrowIfEmpty $Env:AWS_ACCESS_KEY_ID      "AWS_ACCESS_KEY_ID not set"
ThrowIfEmpty $Env:AWS_SECRET_ACCESS_KEY "AWS_SECRET_ACCESS_KEY not set"

$CurrentRegion = $Env:AWS_DEFAULT_REGION
ThrowIfEmpty $CurrentRegion "AWS_DEFAULT_REGION not set"

Write-Host "Region: '$CurrentRegion' | Bucket: '$S3BucketName'" -ForegroundColor Cyan

# ---------------------------------------------------------------------------
# 2. Resolve RavenDB version list
# ---------------------------------------------------------------------------
$flagVersions = @()
if ($v6_0) { $flagVersions += '6.0' }
if ($v6_2) { $flagVersions += '6.2' }
if ($v7_0) { $flagVersions += '7.0' }
if ($v7_1) { $flagVersions += '7.1' }

$AllVersions = ($Versions + $flagVersions) | Sort-Object -Unique
Write-Host "Target versions: $($AllVersions -join ', ')" -ForegroundColor Yellow

# Fetch API key from environment when needed
$RavenDbApiKey = $Env:RAVENDB_API_KEY
if ($AllVersions) {
    ThrowIfEmpty $RavenDbApiKey "RAVENDB_API_KEY env var must be set when specifying versions"
}

# ---------------------------------------------------------------------------
# 3. Process changelogs (download & patch) – placeholder
# ---------------------------------------------------------------------------
function Process-Changelogs {
    param([string]$ApiKey, [string[]]$Versions)
    Write-Host "[TODO] Would download & patch changelogs via $PythonPatcherPath" -ForegroundColor Gray
}

if ($RavenDbApiKey -and $AllVersions) {
    Process-Changelogs -ApiKey $RavenDbApiKey -Versions $AllVersions
}

# ---------------------------------------------------------------------------
# 4. Install JS dependencies & build site
# ---------------------------------------------------------------------------
Write-Host "Installing JS dependencies (npm ci)…" -ForegroundColor Cyan
$env:NODE_OPTIONS = "--max-old-space-size=8192"

if (-not (Test-Path package.json)) { throw "package.json not found" }

npm ci --no-audit --fund false
if ($LASTEXITCODE) { throw "npm ci failed" }

Write-Host "Running 'npx docusaurus build --dev'…" -ForegroundColor Gray
npx docusaurus build --dev
if ($LASTEXITCODE) { throw "Docusaurus build failed" }

$BuildDir = Join-Path $PSScriptRoot 'build'
ThrowIfEmpty (Test-Path $BuildDir) "Build folder not produced ($BuildDir)"

# ---------------------------------------------------------------------------
# 5. Sync build output to S3
# ---------------------------------------------------------------------------
Write-Host "Syncing to s3://$S3BucketName/ …" -ForegroundColor Cyan
aws s3 sync $BuildDir "s3://$S3BucketName/" --delete
if ($LASTEXITCODE) { throw "aws s3 sync failed" }

# ---------------------------------------------------------------------------
# 6. Invalidate CloudFront (optional)
# ---------------------------------------------------------------------------
if ($CloudFrontDistributionId) {
    Write-Host "Invalidating CloudFront distribution $CloudFrontDistributionId" -ForegroundColor Cyan
    aws cloudfront create-invalidation --distribution-id $CloudFrontDistributionId --paths '/*' | Out-Null
    if ($LASTEXITCODE) { throw "CloudFront invalidation failed" }
}

Write-Host "Deployment completed successfully." -ForegroundColor Green
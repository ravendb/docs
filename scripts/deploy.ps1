<#! -----------------------------------------------------------------------------
TeamCity PowerShell Build Script
--------------------------------
Deploys a Docusaurus static site with RavenDB changelogs to AWS S3 and
invalidates CloudFront cache.
This version enables all operational steps except **3** (changelog download)
and **4** (Python patcher), which remain placeholders. Everything else now runs
for real.
    $env:NODE_OPTIONS = "--max-old-space-size=8192"
    docusaurus build --dev
Required parameters (passed as TeamCity build step parameters or CLI args):
  -AwsRegion <string>                 # e.g. eu-central-1
  -S3BucketName <string>              # destination bucket name
Optional parameters:
  -ChangelogApiKey <string>           # api.web.ravendb.net key (if omitted, changelogs are skipped)
  -CloudFrontDistributionId <string>  # distribution to invalidate (if omitted, no invalidation)
  -Versions <string[]>                # explicit list of RavenDB versions (e.g. 6.0,6.2,7.0,7.1)
Version convenience switches (alias-based, can be combined):
  -6.0  -6.2  -7.0  -7.1
------------------------------------------------------------------------------!#>

[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)]
    [string]$AwsRegion,

    [Parameter(Mandatory=$true)]
    [string]$S3BucketName,

    [Parameter(Mandatory=$false)]
    [string]$ChangelogApiKey,

    [Parameter(Mandatory=$false)]
    [string]$CloudFrontDistributionId,

    [Parameter(Mandatory=$false)]
    [string[]]$Versions = @(),

    # convenience switches; dot aliases allow calling with literal "-6.0"
    [Alias('6.0')][switch]$v6_0,
    [Alias('6.2')][switch]$v6_2,
    [Alias('7.0')][switch]$v7_0,
    [Alias('7.1')][switch]$v7_1
)

# ---------------------------------------------------------------------------
# Constants
# ---------------------------------------------------------------------------
# Path to the external Python patcher; update when script moves.
$PythonPatcherPath = Join-Path $PSScriptRoot 'tools/patch_changelogs.py'

function ThrowIfEmpty {
    param([string]$Value,[string]$Message)
    if (-not $Value) { throw $Message }
}

# ---------------------------------------------------------------------------
# Step 0. Verify / install runtime dependencies
# ---------------------------------------------------------------------------
function Ensure-Dependencies {
    Write-Host "Checking runtime dependencies..." -ForegroundColor Cyan

    # Node.js check
    if (-not (Get-Command node -ErrorAction SilentlyContinue)) {
        throw "Node.js runtime is required but was not found in PATH."
    }
    Write-Host "Node.js version: $(node -v)" -ForegroundColor Gray

    # npm check (usually ships with Node)
    if (-not (Get-Command npm -ErrorAction SilentlyContinue)) {
        throw "npm CLI is required but was not found in PATH."
    }

    # AWS CLI check
    if (-not (Get-Command aws -ErrorAction SilentlyContinue)) {
        throw "AWS CLI v2 is required but was not found in PATH."
    }
    Write-Host "AWS CLI version: $(aws --version)" -ForegroundColor Gray

    # Ensure Docusaurus CLI is available (docusaurus.cmd / docusaurus)
    if (-not (Get-Command docusaurus -ErrorAction SilentlyContinue)) {
        Write-Host "Docusaurus CLI not found – installing locally (no‑save)..." -ForegroundColor Yellow
        npm install --no-save @docusaurus/core@latest @docusaurus/cli@latest | Out-Null
        if (-not (Get-Command docusaurus -ErrorAction SilentlyContinue)) {
            throw "Docusaurus CLI installation failed."
        }
    }
    Write-Host "Dependencies are satisfied." -ForegroundColor Green
}

Ensure-Dependencies

# ---------------------------------------------------------------------------
# 1. Gather AWS credentials from environment
# ---------------------------------------------------------------------------
$EnvCreds = @{ 
    AccessKey = $Env:AWS_ACCESS_KEY_ID
    Secret    = $Env:AWS_SECRET_ACCESS_KEY
    Session   = $Env:AWS_SESSION_TOKEN
}
ThrowIfEmpty $EnvCreds.AccessKey "AWS_ACCESS_KEY_ID environment variable not set"
ThrowIfEmpty $EnvCreds.Secret    "AWS_SECRET_ACCESS_KEY environment variable not set"

# export region for downstream CLI calls
$Env:AWS_REGION = $AwsRegion

Write-Host "Using AWS region $AwsRegion and bucket $S3BucketName" -ForegroundColor Cyan

# ---------------------------------------------------------------------------
# 2. Resolve RavenDB version list
# ---------------------------------------------------------------------------
$flagVersions = @()
if ($v6_0) { $flagVersions += '6.0' }
if ($v6_2) { $flagVersions += '6.2' }
if ($v7_0) { $flagVersions += '7.0' }
if ($v7_1) { $flagVersions += '7.1' }

$AllVersions = ($Versions + $flagVersions) | Sort-Object -Unique
Write-Host "Versions targeted: $($AllVersions -join ', ')" -ForegroundColor Yellow

# ---------------------------------------------------------------------------
# 3. Placeholder: Download changelog files (still commented)
# ---------------------------------------------------------------------------
function Download-Changelogs {
    param([string]$ApiKey,[string[]]$Versions)
    Write-Host "[TODO] Downloading changelogs from api.web.ravendb.net for versions: $($Versions -join ', ')" -ForegroundColor Gray
    # Placeholder only – external call intentionally skipped
}

if ($ChangelogApiKey -and $AllVersions.Count -gt 0) {
    Download-Changelogs -ApiKey $ChangelogApiKey -Versions $AllVersions
}

# ---------------------------------------------------------------------------
# 4. Placeholder: Call external Python script to patch changelog contents (still commented)
# ---------------------------------------------------------------------------
function Patch-ChangelogsWithPython {
    param([string[]]$Versions)
    Write-Host "[SKIPPED] Placeholder: would call python patcher at $PythonPatcherPath for versions: $($Versions -join ', ')" -ForegroundColor DarkGray
    # Example real call (disabled):
    # & python $PythonPatcherPath --versions $Versions
    # if ($LASTEXITCODE -ne 0) { throw "Python patcher failed with exit code $LASTEXITCODE" }
}

if ($AllVersions.Count -gt 0) {
    Patch-ChangelogsWithPython -Versions $AllVersions
}

# ---------------------------------------------------------------------------
# 5. Build static site with Docusaurus (increased memory)
# ---------------------------------------------------------------------------
Write-Host "Building Docusaurus site (dev mode, 8 GiB heap)..." -ForegroundColor Cyan
$env:NODE_OPTIONS = "--max-old-space-size=8192"

# Ensure node modules (if package.json exists)
if (Test-Path package.json) {
    if (Test-Path package-lock.json -or Test-Path yarn.lock) {
        Write-Host "Using cached node_modules" -ForegroundColor Gray
    } else {
        Write-Host "Installing NPM dependencies" -ForegroundColor Gray
        npm ci --no-audit --fund false
    }
}

# Run Docusaurus build in dev mode
Write-Host "Running 'docusaurus build --dev'" -ForegroundColor Gray
docusaurus build --dev
if ($LASTEXITCODE -ne 0) { throw "Docusaurus build failed" }

$BuildDir = Join-Path $PSScriptRoot "build"
ThrowIfEmpty (Test-Path $BuildDir) "Docusaurus build folder was not produced ($BuildDir)"

# ---------------------------------------------------------------------------
# 6. Sync build output to S3
# ---------------------------------------------------------------------------
Write-Host "Uploading to s3://$S3BucketName/ ..." -ForegroundColor Cyan
aws s3 sync $BuildDir "s3://$S3BucketName/" --delete --region $AwsRegion
if ($LASTEXITCODE -ne 0) { throw "aws s3 sync failed" }

# ---------------------------------------------------------------------------
# 7. Invalidate CloudFront
# ---------------------------------------------------------------------------
if ($CloudFrontDistributionId) {
    Write-Host "Creating CloudFront invalidation on distribution $CloudFrontDistributionId" -ForegroundColor Cyan
    aws cloudfront create-invalidation --distribution-id $CloudFrontDistributionId --paths "/*" --region $AwsRegion | Out-Null
    if ($LASTEXITCODE -ne 0) { throw "CloudFront invalidation failed" }
}

Write-Host "Deployment completed successfully." -ForegroundColor Green
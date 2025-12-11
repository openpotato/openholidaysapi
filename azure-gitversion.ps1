#
# Generates a Semantic Version for Azure DevOps builds based on Git tags and branch names.
#
# Rules (GitFlow-inspired):
#
# - If HEAD is exactly on a tag (v-<major>.<minor>.<patch>) => use X.Y.Z
# - If not on a tag:
#   + develop branch => bump patch, suffix "-develop[.N]"
#   + release/<ver>  => use <ver>, suffix "-preview[.N]"
#   + hotfix/<ver>   => use <ver>, suffix "-hotfix[.N]"
#   + other branches => sanitize branch name as prerelease suffix
# - Distance (number of commits since last tag) is appended as ".N" if >0
# - If no tags exist => version starts at 0.0.0 and distance = total commits
#
# The resulting version is exported as `SemVersion` (plus parts like Major/Minor/Patch)
# and updates the Azure DevOps build number.
#
# Reference: https://danielkummer.github.io/git-flow-cheatsheet/index.html
#

Set-StrictMode -Version Latest          
$ErrorActionPreference = 'Stop'

# -------------
# Configuration
# -------------
$TagPrefix         = 'v-'       # required tag prefix, e.g. v-1.2.3
$DevelopSuffix     = 'develop'  # prerelease suffix for develop
$ReleaseSuffix     = 'preview'  # prerelease suffix for release/*
$HotfixSuffix      = 'hotfix'   # prerelease suffix for hotfix/*

# ----------------------------------------------
# State variables (defaults if repo has no tags)
# ----------------------------------------------
$MajorVersion    = 0
$MinorVersion    = 0
$PatchVersion    = 0
$VersionSuffix   = ''
$NumberOfCommits = 0

# ---------------
# Regex constants
# ---------------
$ExactTagPattern        = "^$([regex]::Escape($TagPrefix))(?<major>0|[1-9]\d*)\.(?<minor>0|[1-9]\d*)\.(?<patch>0|[1-9]\d*)$"
$DescribeLongTagPattern = "^$([regex]::Escape($TagPrefix))(?<major>0|[1-9]\d*)\.(?<minor>0|[1-9]\d*)\.(?<patch>0|[1-9]\d*)-(?<commits>0|[1-9]\d*)-g(?<sha>[0-9a-f]+)$"
$SemVerPattern          = '^(?<major>0|[1-9]\d*)\.(?<minor>0|[1-9]\d*)\.(?<patch>0|[1-9]\d*)$'

# -------
# Helpers
# -------
function Sanitize-Prerelease([string]$s) {
    # SemVer prerelease identifiers allow only [0-9A-Za-z-] and dot separators.
    # We sanitize the whole branch name into a single identifier (no dots).
    $s = $s.ToLowerInvariant()
    $s = $s -replace '[^0-9a-z-]+','-'  # replace anything not allowed with '-'
    $s = $s -replace '-{2,}','-'        # collapse dup dashes
    $s = $s.Trim('-')
    if ([string]::IsNullOrWhiteSpace($s)) { $s = 'ci' }  # last resort
    return $s
}

# -----------
# Environment
# -----------
$BranchRef  = $Env:BUILD_SOURCEBRANCH       # e.g. refs/heads/main
$BranchName = $Env:BUILD_SOURCEBRANCHNAME   # e.g. main

# -------------------------------------------
# Detect exact tag vs. distance from last tag
# -------------------------------------------
# Is HEAD exactly on a tag like v-1.2.3 ?
$ExactTag    = git describe --tags --exact-match HEAD 2>$null
$HasExactTag = ($LASTEXITCODE -eq 0) -and ($ExactTag -like "$TagPrefix*")

if ($HasExactTag) {
    # Exact tag => clean major/minor/patch from tag
    if ($ExactTag -match $ExactTagPattern) {
        $MajorVersion    = [int]$Matches.major
        $MinorVersion    = [int]$Matches.minor
        $PatchVersion    = [int]$Matches.patch
        $NumberOfCommits = 0
    } else {
        throw "Exact tag '$ExactTag' does not match expected pattern $TagPrefix<major>.<minor>.<patch>"
    }
}
else {
    # Not exactly on a tag: describe distance from most recent matching tag
    $Desc = git describe --tags --long --match "$TagPrefix[0-9]*" HEAD 2>$null
    if ($LASTEXITCODE -eq 0 -and $Desc) {
        if ($Desc -match $DescribeLongTagPattern) {
            $MajorVersion    = [int]$Matches.major
            $MinorVersion    = [int]$Matches.minor
            $PatchVersion    = [int]$Matches.patch
            $NumberOfCommits = [int]$Matches.commits
        }
        else {
            throw "Cannot parse 'git describe' output: $Desc"
        }
    }
    else {
        # No tags in the repo at all: fall back to total commit count
        $NumberOfCommits = [int](git rev-list --count HEAD)
    }
}

# ----------------------------
# Build SemVer based on branch
# ----------------------------
if ($BranchRef -eq 'refs/heads/main') {
    # Enforce tagging on main
    if (-not $HasExactTag) {
        throw "Main builds must be tagged ($TagPrefix<major>.<minor>.<patch>)."
    }
    # Clean X.Y.Z; these parts came from tag or last-tag base
    $SemVersion = "$MajorVersion.$MinorVersion.$PatchVersion"
}
elseif ($BranchRef -eq 'refs/heads/develop') {
    # Bump patch and add develop prerelease with distance count (if any)
    $PatchVersion  = $PatchVersion + 1
    $VersionSuffix = if ($NumberOfCommits -eq 0) { "-$DevelopSuffix" } else { "-$DevelopSuffix.$NumberOfCommits" }
    $SemVersion    = "$MajorVersion.$MinorVersion.$PatchVersion$VersionSuffix"
}
elseif ($BranchRef.StartsWith('refs/heads/hotfix/')) {
    # hotfix/<major>.<minor>.<patch>
    $verPart = $BranchName.Substring('hotfix/'.Length)
    if ($verPart -match $SemVerPattern) {
        $MajorVersion  = [int]$Matches.major
        $MinorVersion  = [int]$Matches.minor
        $PatchVersion  = [int]$Matches.patch
        $VersionSuffix = if ($NumberOfCommits -eq 0) { "-$HotfixSuffix" } else { "-$HotfixSuffix.$NumberOfCommits" }
        $SemVersion    = "$MajorVersion.$MinorVersion.$PatchVersion$VersionSuffix"
    } else {
        throw "Expected branch 'hotfix/<major>.<minor>.<patch>' but got '$BranchName'"
    }
}
elseif ($BranchRef.StartsWith('refs/heads/release/')) {
    # release/<major>.<minor>.<patch>
    $verPart = $BranchName.Substring('release/'.Length)
    if ($verPart -match $SemVerPattern) {
        $MajorVersion  = [int]$Matches.major
        $MinorVersion  = [int]$Matches.minor
        $PatchVersion  = [int]$Matches.patch
        $VersionSuffix = if ($NumberOfCommits -eq 0) { "-$ReleaseSuffix" } else { "-$ReleaseSuffix.$NumberOfCommits" }
        $SemVersion    = "$MajorVersion.$MinorVersion.$PatchVersion$VersionSuffix"
    } else {
        throw "Expected branch 'release/<major>.<minor>.<patch>' but got '$BranchName'"
    }
}
else {
	# Any other branch (feature/, bugfix/, fix/, etc.)
	$Safe = Sanitize-Prerelease ($BranchRef -replace '^refs/heads/','')
	$VersionSuffix = if ($NumberOfCommits -eq 0) { "-$Safe" } else { "-$Safe.$NumberOfCommits" }
	$SemVersion    = "$MajorVersion.$MinorVersion.$PatchVersion$VersionSuffix"
}

# -------
# Logging
# -------
Write-Host "Branch: $BranchRef"
Write-Host "Branch name: $BranchName"
Write-Host "Has exact tag: $HasExactTag"
Write-Host "Number of commits since tag (or total if none): $NumberOfCommits"
Write-Host "Major version: $MajorVersion"
Write-Host "Minor version: $MinorVersion"
Write-Host "Patch version: $PatchVersion"
Write-Host "Version Suffix: $VersionSuffix"
Write-Host "Sem version: $SemVersion"

# -------------------------------------------------------
# Export variables for later steps / jobs in Azure DevOps
# -------------------------------------------------------
Write-Host "##vso[task.setvariable variable=HasExactTag;isOutput=true]$HasExactTag"
Write-Host "##vso[task.setvariable variable=NumberOfCommits;isOutput=true]$NumberOfCommits"
Write-Host "##vso[task.setvariable variable=MajorVersion;isOutput=true]$MajorVersion"
Write-Host "##vso[task.setvariable variable=MinorVersion;isOutput=true]$MinorVersion"
Write-Host "##vso[task.setvariable variable=PatchVersion;isOutput=true]$PatchVersion"
Write-Host "##vso[task.setvariable variable=VersionSuffix;isOutput=true]$VersionSuffix"
Write-Host "##vso[task.setvariable variable=SemVersion;isOutput=true]$SemVersion"

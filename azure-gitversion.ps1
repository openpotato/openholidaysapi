#
# Generates a version number from Git tags and/or branch names following GitVersion/GitFlow approach
#

# Init global consts
$TagPrefix       = "v-"
$DevelopSuffix   = "develop"
$ReleaseSuffix   = "preview"
$HotfixSuffix    = "hotfix"

# Init global variables
$MajorVersion    = 0
$MinorVersion    = 0
$PatchVersion    = 1
$VersionSuffix   = ""
$NumberOfCommits = 0

# RegEx patterns
$GitDescribePattern   = "(?<major>0|[1-9]\d*)\.(?<minor>0|[1-9]\d*)\.(?<patch>0|[1-9]\d*)(\-(?<commits>0|[1-9]\d*))?"
$GitBranchNamePattern = "(?<major>0|[1-9]\d*)\.(?<minor>0|[1-9]\d*)\.(?<patch>0|[1-9]\d*)"

# Get the current git branch from Azure DevOps
$CurrentBranch = $Env:BUILD_SOURCEBRANCH
$CurrentBranchName = $Env:BUILD_SOURCEBRANCHNAME

# Try to extract version info from Git tag
$Tag = git describe --tags --match "$TagPrefix[0-9]*"
if ($Tag)
{
    if ($Tag -match $GitDescribePattern)
	{
		$MajorVersion = [int]$Matches.major
		$MinorVersion = [int]$Matches.minor
		$PatchVersion = [int]$Matches.patch
		$NumberOfCommits = [int]$Matches.commits
	}
    else
    {
        throw "Could not extract version info from Git tag " + $Tag
    }
}
else
{
    # Git git rev-list --count head
	$NumberOfCommits = git rev-list --count head
}

# Try to calculate semantic version based on GitFlow branching
if ($CurrentBranch -match "main")
{
	$SemVersion = [string]$MajorVersion + "." + [string]$MinorVersion + "." + [string]$PatchVersion
}
elseif ($CurrentBranch -match "develop")
{
	$PatchVersion = $PatchVersion + 1
    $VersionSuffix = if ($NumberOfCommits -eq 0) { "-" + $DevelopSuffix } else { "-" + $DevelopSuffix + "." + [string]$NumberOfCommits }
    $SemVersion = [string]$MajorVersion + "." + [string]$MinorVersion + "." + [string]$PatchVersion + $VersionSuffix
}
elseif ($CurrentBranch -match "hotfix")
{
	if ($CurrentBranchName -match $GitBranchNamePattern)
	{
		$MajorVersion = [int]$Matches.major
		$MinorVersion = [int]$Matches.minor
		$PatchVersion = [int]$Matches.patch
        $VersionSuffix = if ($NumberOfCommits -eq 0) { "-" + $HotfixSuffix } else { "-" + $HotfixSuffix + "." + [string]$NumberOfCommits }
        $SemVersion = [string]$MajorVersion + "." + [string]$MinorVersion + "." + [string]$PatchVersion + $VersionSuffix
	}
    else
    {
        throw "Could not extract version info from Git branch name " + $CurrentBranchName
    }
}
elseif ($CurrentBranch -match "release")
{
	if ($CurrentBranchName -match $GitBranchNamePattern)
	{
		$MajorVersion = [int]$Matches.major
		$MinorVersion = [int]$Matches.minor
		$PatchVersion = [int]$Matches.patch
        $VersionSuffix = if ($NumberOfCommits -eq 0) { "-" + $ReleaseSuffix } else { "-" + $ReleaseSuffix + "." + [string]$NumberOfCommits }
        $SemVersion = [string]$MajorVersion + "." + [string]$MinorVersion + "." + [string]$PatchVersion + $VersionSuffix
	}
    else
    {
        throw "Could not extract version info from Git branch name " + $CurrentBranchName
    }
}
elseif ($CurrentBranch -match "feature")
{
	$VersionSuffix = if ($NumberOfCommits -eq 0) { "-" + $CurrentBranchName } else { "-" + $CurrentBranchName + "." + [string]$NumberOfCommits }
    $SemVersion = [string]$MajorVersion + "." + [string]$MinorVersion + "." + [string]$PatchVersion + $VersionSuffix
}

# Log output
Write-Host "Branch: $CurrentBranch"
Write-Host "Branch name: $CurrentBranchName"
Write-Host "Git describe: $Tag"
Write-Host "Number of commits: $NumberOfCommits"
Write-Host "Major version: $MajorVersion"
Write-Host "Minor version: $MinorVersion"
Write-Host "Patch version: $PatchVersion"
Write-Host "Version Suffix: $VersionSuffix"
Write-Host "Sem version: $SemVersion"

# Set Azure DevOps variables
Write-Host "##vso[task.setvariable variable=MajorVersion]$MajorVersion"
Write-Host "##vso[task.setvariable variable=MinorVersion]$MinorVersion"
Write-Host "##vso[task.setvariable variable=PatchVersion]$PatchVersion"
Write-Host "##vso[task.setvariable variable=VersionSuffix]$VersionSuffix"
Write-Host "##vso[task.setvariable variable=SemVersion]$SemVersion"
# Taken from psake https://github.com/psake/psake

<#
.SYNOPSIS
  This is a helper function that runs a scriptblock and checks the PS variable $lastexitcode
  to see if an error occcured. If an error is detected then an exception is thrown.
  This function allows you to run command-line programs without having to
  explicitly check the $lastexitcode variable.
.EXAMPLE
  exec { svn info $repository_trunk } "Error executing SVN. Please verify SVN command-line client is installed"
#>
function Exec
{
    [CmdletBinding()]
    param(
        [Parameter(Position=0,Mandatory=1)][scriptblock]$cmd,
        [Parameter(Position=1,Mandatory=0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
    )
    & $cmd
    if ($lastexitcode -ne 0) {
        throw ("Exec: " + $errorMessage)
    }
}

function Update-SourceVersion
{
    param(
        [string]$Version,
        [string]$InfoVersion
    )
    $NewVersion = 'AssemblyVersion("' + $Version + '")'
    $NewFileVersion = 'AssemblyFileVersion("' + $Version + '")'
    $NewInformationalVersion = 'AssemblyInformationalVersion("' + $InfoVersion + '")'
    foreach ($o in $input)
    {
        Write-Output $o.FullName
        $TmpFile = $o.FullName + ".tmp"
        Get-Content $o.FullName |
            %{$_ -replace 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)', $NewVersion } |
            %{$_ -replace 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)', $NewFileVersion } |
            %{$_ -replace 'AssemblyInformationalVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)', $NewInformationalVersion } > $TmpFile
        Move-Item $TmpFile $o.FullName -Force
    }
}

$artifacts = ".\artifacts"

if (Test-Path $artifacts) {
    Remove-Item $artifacts -Force -Recurse
}

$VERSION_FROM_TAG = $Env:GITHUB_REF -replace "refs/tags/", ""
if ([string]::IsNullOrEmpty($VERSION_FROM_TAG) -or $VERSION_FROM_TAG.StartsWith("refs/")) {
    $VERSION_FROM_TAG = (git describe --tags --abbrev=0)
}

$VERSION_PREFIX = $VERSION_FROM_TAG

if ($VERSION_PREFIX.StartsWith("v")) {
    $VERSION_PREFIX = $VERSION_PREFIX.Substring(1)
}

if ($VERSION_PREFIX.Contains("-")) {
    $indexOfDash = $VERSION_PREFIX.IndexOf("-")
    $VERSION_SUFFIX = $VERSION_PREFIX.Substring($indexOfDash + 1)
    $VERSION_PREFIX = $VERSION_PREFIX.Substring(0, $indexOfDash)
}

Write-Host "Version from tag is ${VERSION_FROM_TAG}"
Write-Host "Version prefix is ${VERSION_PREFIX}"

$SOURCE_VERSION = (git log -n1 --format=format:"%H").Substring(0, 7)
Write-Host "Source version is ${SOURCE_VERSION}"

if ($VERSION_SUFFIX -ne $null) {
    $VERSION_SUFFIX = "-${VERSION_SUFFIX}"
}

$VERSION_SUFFIX = "${VERSION_SUFFIX}+${SOURCE_VERSION}"

Write-Host "Version suffix is ${VERSION_SUFFIX}"
Write-Host "Informational version is ${VERSION_PREFIX}${VERSION_SUFFIX}"

Write-Host "ZIP_FILE_NAME=NetIRC.Desktop-${VERSION_FROM_TAG}.zip"

if ([string]::IsNullOrEmpty($Env:GITHUB_ENV) -eq $false) {
    # Set GitHub environment variables
    echo "GIT_TAG_NAME=${VERSION_FROM_TAG}" | Out-File -FilePath $Env:GITHUB_ENV -Encoding utf-8 -Append
    echo "ZIP_FILE_NAME=NetIRC.Desktop-${VERSION_FROM_TAG}.zip" | Out-File -FilePath $Env:GITHUB_ENV -Encoding utf-8 -Append
}

Get-ChildItem -recurse |? {$_.Name -eq "AssemblyInfo.cs"} | Update-SourceVersion $VERSION_PREFIX "${VERSION_PREFIX}${VERSION_SUFFIX}"

exec { msbuild NetIRC.Desktop.sln /nologo /p:Configuration=Release /t:Clean }

exec { msbuild NetIRC.Desktop.sln /nologo /p:platform="Any CPU" /p:Configuration=Release /t:Build /p:OutputPath=..\$artifacts }

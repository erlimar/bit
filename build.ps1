#require -version 3

##########################################################################
# This is the Cake bootstrapper script for PowerShell.
# This file was downloaded from https://github.com/cake-build/resources
# Feel free to change this file to fit your needs.
##########################################################################

<#

.SYNOPSIS
This is a Powershell script to bootstrap a Cake build.

.DESCRIPTION
This Powershell script will download DotNet if missing, restore DotNet tools (including Cake)
and execute your Cake build script with the parameters you provide.

.PARAMETER Script
The build script to execute.
.PARAMETER Target
The build script target to run.
.PARAMETER Configuration
The build configuration to use.
.PARAMETER Verbosity
Specifies the amount of information to be displayed.
.PARAMETER ShowDescription
Shows description about tasks.
.PARAMETER DryRun
Performs a dry run.
.PARAMETER Experimental
Uses the nightly builds of the Roslyn script engine.
.PARAMETER Mono
Uses the Mono Compiler rather than the Roslyn script engine.
.PARAMETER SkipToolsPackageRestore
Skips restoring of tools packages.
.PARAMETER SkipModulePackageRestore
Skips restoring of modules packages.
.PARAMETER SkipAddinPackageRestore
Skips restoring of addins packages.
.PARAMETER ScriptArgs
Remaining arguments are added here.

.LINK
https://cakebuild.net

#>

[CmdletBinding()]
Param(
    [string]$Script = "build.cake",
    [string]$Target,
    [string]$Configuration,
    [ValidateSet("Quiet", "Minimal", "Normal", "Verbose", "Diagnostic")]
    [string]$Verbosity,
    [switch]$ShowDescription,
    [Alias("WhatIf", "Noop")]
    [switch]$DryRun,
    [switch]$Experimental,
    [switch]$Mono,
    [switch]$SkipToolsPackageRestore,
    [switch]$SkipModulePackageRestore,
    [switch]$SkipAddinPackageRestore,
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$ScriptArgs
)

#Default DotNet SDK minimal version 1.1.4
$SDK_VERSION_CAKE = "1.0.4"
$SDK_VERSION_GLOBAL = $SDK_VERSION_CAKE

$BUILD_DIR = Join-Path $PSScriptRoot "build"
$TOOLS_DIR = Join-Path $BUILD_DIR "tools"
$ADDINS_DIR = Join-Path $BUILD_DIR "addins"
$MODULES_DIR = Join-Path $BUILD_DIR "modules"
$DOTNET_DIR = Join-Path $BUILD_DIR ".dotnetsdk"
$DOTNET_INSTALL_URL ="https://dot.net/v1/dotnet-install.ps1"
$DOTNET_INSTALL_PATH = Join-Path $DOTNET_DIR "dotnet-install.ps1"
$DOTNET_COMMAND = Join-Path $DOTNET_DIR "dotnet.exe"
$DOTNET_RUNTIME_CAKE_PATH = Join-Path $DOTNET_DIR "shared" | Join-Path -ChildPath "Microsoft.NETCore.App"| Join-Path -ChildPath $SDK_VERSION_CAKE
$CAKE_VERSION = "0.23.0"
$CAKE_DLL = Join-Path $TOOLS_DIR "cake.coreclr/$CAKE_VERSION/Cake.dll"
$TOOLS_PACKAGES_CONFIG = Join-Path $TOOLS_DIR "packages.config"
$ADDINS_PACKAGES_CONFIG = Join-Path $ADDINS_DIR "packages.config"
$MODULES_PACKAGES_CONFIG = Join-Path $MODULES_DIR "packages.config"
$GLOBAL_JSON_PATH = Join-Path $PSScriptRoot "global.json"

# Detecting global.json
if ($GLOBAL_JSON_PATH | Test-Path) {
    Write-Verbose -Message "Detecting .NET SDK version from global.json in ${GLOBAL_JSON_PATH}"
    $globalJson = Get-Content $GLOBAL_JSON_PATH | ConvertFrom-Json

    if ($globalJson.sdk -and $globalJson.sdk.version) {
        $SDK_VERSION_GLOBAL = $globalJson.sdk.version
        Write-Verbose -Message "Detected .NET SDK version: ${SDK_VERSION_GLOBAL}"
    }
    else {
        Write-Verbose -Message "File global.json don't contain sdk version information"
    }
}

[Reflection.Assembly]::LoadWithPartialName("System.Security") | Out-Null
function MD5HashFile([string] $filePath)
{
    if ([string]::IsNullOrEmpty($filePath) -or !(Test-Path $filePath -PathType Leaf))
    {
        return $null
    }

    [System.IO.Stream] $file = $null;
    [System.Security.Cryptography.MD5] $md5 = $null;
    try
    {
        $md5 = [System.Security.Cryptography.MD5]::Create()
        $file = [System.IO.File]::OpenRead($filePath)
        return [System.BitConverter]::ToString($md5.ComputeHash($file))
    }
    finally
    {
        if ($file -ne $null)
        {
            $file.Dispose()
        }
    }
}

function GetProxyEnabledWebClient
{
    $wc = New-Object System.Net.WebClient
    $proxy = [System.Net.WebRequest]::GetSystemWebProxy()
    $proxy.Credentials = [System.Net.CredentialCache]::DefaultCredentials        
    $wc.Proxy = $proxy
    return $wc
}

function WriteCakeToolsDotNetProject([string] $outputPath) {
    $content = @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>netstandard1.6</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Cake.CoreCLR" Version="$CAKE_VERSION" />
  </ItemGroup>
</Project>
"@
    $content | Out-File $outputPath -Encoding "UTF8"
}

Write-Host "Preparing to run build script..."

if(!$PSScriptRoot){
    $PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent
}

# Make sure tools folder exists
if ((Test-Path $PSScriptRoot) -and !(Test-Path $TOOLS_DIR)) {
    Write-Verbose -Message "Creating tools directory..."
    New-Item -Path $TOOLS_DIR -Type directory | out-null
}

# Make sure that packages.config exist.
if (!(Test-Path $TOOLS_PACKAGES_CONFIG)) {
    Write-Verbose -Message "Writing packages.config..."    
    try {        
        WriteCakeToolsDotNetProject -outputPath $TOOLS_PACKAGES_CONFIG } catch {
        Throw "Could not write packages.config."
    }
}

# Try install DotNet if not exists
if (-not ($DOTNET_COMMAND | Test-Path)) {
    Write-Host "Installing DotNet SDK v${SDK_VERSION_GLOBAL}..."
    mkdir $DOTNET_DIR -Force | Out-Null
    
    if (-not ($DOTNET_INSTALL_PATH | Test-Path))
    {
        Write-Verbose -Message "Downloading DotNet installer..."
        try {
            $wc = GetProxyEnabledWebClient
            $wc.DownloadFile($DOTNET_INSTALL_URL, $DOTNET_INSTALL_PATH)
        } catch {
            Throw "Could not download DotNet installer."
        }
    }

    & $DOTNET_INSTALL_PATH -Version $SDK_VERSION_GLOBAL -InstallDir $DOTNET_DIR

    if (-not ($DOTNET_COMMAND | Test-Path)){
        Throw "Could not install DotNet $SDK_VERSION_GLOBAL."
    }
}

# Ensure DotNet runtime for Cake tool
if (-not ($DOTNET_RUNTIME_CAKE_PATH | Test-Path)) {
    Write-Host "Installing DotNet Runtime v${SDK_VERSION_CAKE} for Cake tool..."
    mkdir $DOTNET_DIR -Force | Out-Null
    
    if (-not ($DOTNET_INSTALL_PATH | Test-Path))
    {
        Write-Verbose -Message "Downloading DotNet installer..."
        try {
            $wc = GetProxyEnabledWebClient
            $wc.DownloadFile($DOTNET_INSTALL_URL, $DOTNET_INSTALL_PATH)
        } catch {
            Throw "Could not download DotNet installer."
        }
    }

    & $DOTNET_INSTALL_PATH -SharedRuntime -Version $SDK_VERSION_CAKE -InstallDir $DOTNET_DIR

    if (-not ($DOTNET_RUNTIME_CAKE_PATH | Test-Path)) {
        Throw "Could not install DotNet Runtime v${SDK_VERSION_CAKE} for Cake tool."
    }
}

if($DOTNET_INSTALL_PATH | Test-Path) {
    Remove-Item $DOTNET_INSTALL_PATH -Force
}

# Add dotnet.exe path to PATH environment to be available to child processed
$ENV:PATH = "${DOTNET_DIR};${ENV:PATH}"

function RestorePackages([string] $targetName, [string] $packageFile) {
    # Check for changes in packages.config and remove installed packages if true.
    $parentPath = Split-Path $packageFile -Parent
    $md5File = "$packageFile.md5sum"
    [string] $md5Hash = MD5HashFile($packageFile)

    if((!(Test-Path $md5File)) -Or ($md5Hash -ne (Get-Content $md5File ))) {
        Write-Verbose -Message "Missing or changed $targetName package.config hash..."
        Get-ChildItem -Path $parentPath -Exclude packages.config,Cake.Bakery |
            Remove-Item -Recurse

        Write-Verbose -Message "Restoring $targetName..."
        & "$DOTNET_COMMAND" restore $packageFile --packages "$parentPath"

        if ($LASTEXITCODE -ne 0) {
            Throw "An error occurred while restoring $targetName."
        }
        else
        {
            $md5Hash | Out-File $md5File -Encoding "ASCII"
        }
    }
}

# Restore tools?
if(-Not $SkipToolsPackageRestore.IsPresent) {
    RestorePackages -targetName "tools" -packageFile $TOOLS_PACKAGES_CONFIG
}

# Restore addins?
if(-Not $SkipAddinPackageRestore.IsPresent -and (Test-Path $ADDINS_PACKAGES_CONFIG)) {
    RestorePackages -targetName "addins" -packageFile $ADDINS_PACKAGES_CONFIG
}

# Restore modules?
if(-Not $SkipModulePackageRestore.IsPresent -and (Test-Path $MODULES_PACKAGES_CONFIG)) {
    RestorePackages -targetName "modules" -packageFile $MODULES_PACKAGES_CONFIG
}

# Make sure that Cake has been installed.
if (!(Test-Path $CAKE_DLL)) {
    Throw "Could not find Cake.dll at $CAKE_DLL"
}

# Build Cake arguments
$cakeArguments = @("$Script");
if ($Target) { $cakeArguments += "-target=$Target" }
if ($Configuration) { $cakeArguments += "-configuration=$Configuration" }
if ($Verbosity) { $cakeArguments += "-verbosity=$Verbosity" }
if ($ShowDescription) { $cakeArguments += "-showdescription" }
if ($DryRun) { $cakeArguments += "-dryrun" }
if ($Experimental) { $cakeArguments += "-experimental" }
if ($Mono) { $cakeArguments += "-mono" }
$cakeArguments += $ScriptArgs

# Start Cake
Write-Host "Running build script..."
& "$DOTNET_COMMAND" $CAKE_DLL $cakeArguments
exit $LASTEXITCODE

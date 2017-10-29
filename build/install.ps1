# Copyright (c) E5R Development Team. All rights reserved.
# Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

$SDK_INSTALL_DIR = "$pwd\.dotnetsdk"
$SDK_PLATFORM = "$env:PLATFORM"
$SDK_VERSION = "$env:CLI_VERSION"

$SCRIPT_INSTALL_URL ="https://dot.net/v1/dotnet-install.ps1"
$SCRIPT_INSTALL_PATH = "$SDK_INSTALL_DIR\dotnet-install.ps1"

mkdir $SDK_INSTALL_DIR -Force | Out-Null

if (-not ("$SCRIPT_INSTALL_PATH" | Test-Path))
{
    Invoke-WebRequest -Uri "$SCRIPT_INSTALL_URL" -OutFile "$SCRIPT_INSTALL_PATH"
}

if (-not ("$SDK_INSTALL_DIR\dotnet.exe" | Test-Path)){
    & "$SCRIPT_INSTALL_PATH" -Architecture $SDK_PLATFORM -Version $SDK_VERSION -InstallDir $SDK_INSTALL_DIR
}

$env:Path = "$env:DOTNET_INSTALL_DIR;$env:Path"

# Copyright (c) E5R Development Team. All rights reserved.
# Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

$SDK_INSTALL_DIR = "$pwd\.dotnetsdk"
$DOTNET_EXE ="$SDK_INSTALL_DIR\dotnet.exe"

& $DOTNET_EXE --info
& $DOTNET_EXE restore --no-cache
& $DOTNET_EXE build -c $env:CONFIGURATION

& $DOTNET_EXE pack ".\src\E5R.Sdk.Bit\E5R.Sdk.Bit.csproj" -c $env:CONFIGURATION /p:VersionPrefix="$env:APPVEYOR_BUILD_VERSION"

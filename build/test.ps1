# Copyright (c) E5R Development Team. All rights reserved.
# Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

$SDK_INSTALL_DIR = "$pwd\.dotnetsdk"
$DOTNET_EXE ="$SDK_INSTALL_DIR\dotnet.exe"

$DOTNET_EXE test
$DOTNET_EXE test ".\test\E5R.Sdk.Bit.Test\E5R.Sdk.Bit.Test.csproj"
$DOTNET_EXE test ".\test\E5R.Tools.Bit.Engine.Test\E5R.Tools.Bit.Engine.Test.csproj"
$DOTNET_EXE test ".\test\E5R.Tools.Bit.Test\E5R.Tools.Bit.Test.csproj"

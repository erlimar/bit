#require -version 3
#
# Copyright (c) E5R Development Team. All rights reserved.
# Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

[CmdletBinding()]
Param(
    [string]$ZipPath,
    [string]$DirectoryPath
)

Add-Type -Assembly System.IO.Compression.FileSystem;

# try {
    if(!($DirectoryPath | Test-Path)) {
        mkdir $DirectoryPath -Force | Out-Null
    }
    [System.IO.Compression.ZipFile].ExtractToDirectory($ZipPath, $DirectoryPath);
# } catch {
#    Throw "Could not extract zip file ${ZipPath}."
# }

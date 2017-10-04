@echo off

echo -^> Running [E5R.Tools.Bit.dll]
echo.

dotnet src/E5R.Tools.Bit/bin/Debug/netcoreapp2.0/E5R.Tools.Bit.dll %*

echo.
echo -^> ExitCode [%ERRORLEVEL%]

@echo off
setlocal

set MSBUILD_EXE=C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\msbuild.exe
set SOLUTION_FILE=.\KeyPickupApp.sln
set TARGET_FRAMEWORK=net6.0-windows10.0.19041
set RUNTIME_IDENTIFIER=win10-x64

call "%MSBUILD_EXE%" "%SOLUTION_FILE%" /restore /t:build /p:Configuration=Release /p:Platform="Any CPU" /p:TargetFramework="%TARGET_FRAMEWORK%" /p:WindowsAppSDKSelfContained=true /p:WindowsPackageType=None /p:RuntimeIdentifier="%RUNTIME_IDENTIFIER%"

endlocal

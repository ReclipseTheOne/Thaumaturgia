@echo off
REM Batch wrapper for thaumaturgia.ps1
REM Pass all arguments to the PowerShell script

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0thaumaturgia.ps1" %*

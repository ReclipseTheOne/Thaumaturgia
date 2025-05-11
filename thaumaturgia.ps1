<#
.SYNOPSIS
    Thaumaturgia task runner (Gradle-style)
.DESCRIPTION
    This script provides Gradle-like task execution for the Thaumaturgia project.
    Run `.\thaumaturgia.ps1 tasks` to see all available tasks.
.PARAMETER Task
    The task to execute. Use 'tasks' to list all available tasks.
.EXAMPLE
    .\thaumaturgia.ps1 tasks
    .\thaumaturgia.ps1 build
    .\thaumaturgia.ps1 runDev
#>

param (
    [Parameter(Position=0)]
    [string]$Task = "build"
)

$projectPath = Join-Path $PSScriptRoot "Thaumaturgia.csproj"
$msbuildPath = "dotnet msbuild"

# Define tasks and their descriptions
$tasks = @{
    "build" = "Build the project"
    "cleanRunFolder" = "Clean the run folder"
    "setupEnvironment" = "Setup the environment directories"
    "copyAssets" = "Copy assets to run folder"
    "runDev" = "Build and run the game in development mode"
    "cleanBuildRun" = "Clean the run folder, rebuild, and run"
    "prepareDistribution" = "Prepare a distribution package"
    "clean" = "Clean build outputs"
    "rebuild" = "Clean and build"
}

function Show-TaskList {
    Write-Host "`nAvailable tasks:`n" -ForegroundColor Cyan
    
    foreach ($key in $tasks.Keys | Sort-Object) {
        Write-Host "  $key".PadRight(25) -NoNewline -ForegroundColor Yellow
        Write-Host "- $($tasks[$key])" -ForegroundColor Gray
    }
    
    Write-Host "`nExample usage: .\thaumaturgia.ps1 runDev`n" -ForegroundColor Cyan
}

function Execute-Task {
    param (
        [string]$TaskName
    )
    
    Write-Host "`n>> Executing task: $TaskName" -ForegroundColor Cyan
    
    switch ($TaskName) {
        "tasks" {
            Show-TaskList
            return
        }
        "clean" {
            # Use MSBuild's built-in Clean target
            $command = "$msbuildPath `"$projectPath`" /t:Clean"
        }
        "rebuild" {
            $command = "$msbuildPath `"$projectPath`" /t:Clean;Build"
        }
        default {
            # Convert task name to pascal case for MSBuild target (e.g., "runDev" to "RunDev")
            $targetName = [regex]::Replace($TaskName, '(?:^|_)(.)', { param($match) $match.Groups[1].Value.ToUpper() })
            $command = "$msbuildPath `"$projectPath`" /t:$targetName /p:Configuration=Debug /v:m"
        }
    }
    
    Write-Host "Executing: $command" -ForegroundColor DarkGray
    Invoke-Expression $command
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Task failed with exit code $LASTEXITCODE" -ForegroundColor Red
        exit $LASTEXITCODE
    } else {
        Write-Host "Task completed successfully" -ForegroundColor Green
    }
}

# Check if the project file exists
if (-not (Test-Path $projectPath)) {
    Write-Host "Project file not found: $projectPath" -ForegroundColor Red
    exit 1
}

# Execute the specified task
if ($Task -eq "tasks") {
    Show-TaskList
} else {
    # Check if the task exists
    $validTask = $tasks.ContainsKey($Task.ToLower())
    
    if (-not $validTask) {
        Write-Host "Unknown task: $Task" -ForegroundColor Red
        Write-Host "Run '.\thaumaturgia.ps1 tasks' to see all available tasks." -ForegroundColor Yellow
        exit 1
    }
    
    Execute-Task -TaskName $Task
}

# Creating Custom Tasks in Thaumaturgia

This guide explains how to create custom tasks for the Thaumaturgia project using our Gradle-like task system.

## Understanding the Task System

The Thaumaturgia task system is built on MSBuild targets defined in the `.csproj` file and wrapped with a PowerShell script for easier access. This setup provides a Gradle-like experience for managing build tasks.

## Adding a New Task

To add a new task to the system, follow these steps:

### 1. Add an MSBuild Target

Open the `Thaumaturgia.csproj` file and add a new `<Target>` element within the project:

```xml
<!-- Custom Task: YourTaskName -->
<Target Name="YourTaskName" DependsOnTargets="OptionalDependencies">
  <!-- Task actions go here -->
  <Message Text="Running YourTaskName task" Importance="high" />
  
  <!-- Example: Create directories -->
  <MakeDir Directories="$(RunDir)\your-directory" Condition="!Exists('$(RunDir)\your-directory')" />
  
  <!-- Example: Copy files -->
  <Copy SourceFiles="$(AssetsDir)\your-file.txt" DestinationFolder="$(RunDir)\your-directory" />
  
  <!-- Example: Run a command -->
  <Exec Command="your-command --with-arguments" WorkingDirectory="$(RunDir)" />
</Target>
```

### 2. Add Task to the PowerShell Script

Open the `thaumaturgia.ps1` file and add your task to the `$tasks` dictionary:

```powershell
$tasks = @{
    # Existing tasks...
    "yourTaskName" = "Description of your task"
}
```

### 3. Add Task to VS Code Tasks

Open the `.vscode/tasks.json` file and add a new task configuration:

```json
{
    "label": "Your Task Name",
    "command": "powershell.exe",
    "type": "process",
    "args": [
        "-NoProfile",
        "-ExecutionPolicy", "Bypass",
        "-File", "${workspaceFolder}\\thaumaturgia.ps1",
        "yourTaskName"
    ],
    "problemMatcher": "$msCompile"
}
```

## Task Dependencies

You can make tasks depend on other tasks using the `DependsOnTargets` attribute:

```xml
<Target Name="YourTask" DependsOnTargets="SetupEnvironment;AnotherTask">
    <!-- Task actions -->
</Target>
```

This ensures that `SetupEnvironment` and `AnotherTask` run before `YourTask`.

## Common MSBuild Tasks

Here are some common MSBuild tasks you can use:

### Creating Directories

```xml
<MakeDir Directories="$(RunDir)\your-directory" Condition="!Exists('$(RunDir)\your-directory')" />
```

### Removing Directories

```xml
<RemoveDir Directories="$(RunDir)\your-directory" />
```

### Copying Files

```xml
<Copy SourceFiles="$(AssetsDir)\your-file.txt" DestinationFolder="$(RunDir)\your-directory" />
```

### Running Commands

```xml
<Exec Command="your-command --with-arguments" WorkingDirectory="$(RunDir)" />
```

## Example: Adding a "Clean Logs" Task

Here's a complete example of adding a task to clean log files:

1. In the project file:

```xml
<!-- Clean logs task: Removes log files -->
<Target Name="CleanLogs">
  <ItemGroup>
    <LogFiles Include="$(RunDir)\logs\*.log" />
  </ItemGroup>
  <Delete Files="@(LogFiles)" />
  <Message Text="Cleaned log files" Importance="high" />
</Target>
```

2. In the PowerShell script:

```powershell
$tasks = @{
    # Existing tasks...
    "cleanLogs" = "Clean log files from the run directory"
}
```

3. In the VS Code tasks:

```json
{
    "label": "Clean Logs",
    "command": "powershell.exe",
    "type": "process",
    "args": [
        "-NoProfile",
        "-ExecutionPolicy", "Bypass",
        "-File", "${workspaceFolder}\\thaumaturgia.ps1",
        "cleanLogs"
    ],
    "problemMatcher": []
}
```

## Running Your Tasks

Run your task using one of these methods:

1. Command line: `.\thaumaturgia.ps1 yourTaskName`
2. Batch file: `thaumaturgia yourTaskName`
3. VS Code: Press `Ctrl+Shift+P`, type "Tasks: Run Task", and select your task

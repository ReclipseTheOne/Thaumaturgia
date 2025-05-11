# Thaumaturgia

Thaumaturgia is a voxel-based survival game written in C# with extensive modding capabilities. This project is currently in early stages of development.

## Documentation

- [Project Structure](docs/structure.md) - Overview of the project architecture and file organization
- [Task System](docs/tasks.md) - Guide to using and extending the Gradle-like task system
- [OpenGL Integration](docs/opengl.md) - Details about OpenTK integration and rendering
- [Rendering Reference](docs/rendering-reference.md) - Quick reference guide for rendering components

## Development Environment

### Gradle-like Task System

Thaumaturgia uses a Gradle-inspired task system for building and running the game. You can use either the PowerShell script (`thaumaturgia.ps1`) or batch file (`thaumaturgia.bat`) to run tasks.

#### Available Tasks:

- `build` - Build the project
- `cleanRunFolder` - Clean the run folder
- `setupEnvironment` - Setup the environment directories
- `copyAssets` - Copy assets to run folder
- `runDev` - Build and run the game in development mode
- `cleanBuildRun` - Clean the run folder, rebuild, and run
- `prepareDistribution` - Prepare a distribution package
- `clean` - Clean build outputs
- `rebuild` - Clean and build
- `tasks` - List all available tasks

#### Examples:

```bash
# List all available tasks
.\thaumaturgia.ps1 tasks

# Build and run the game
.\thaumaturgia.ps1 runDev

# Clean, build and run
.\thaumaturgia.ps1 cleanBuildRun

# Prepare a distribution package
.\thaumaturgia.ps1 prepareDistribution
```

Or using the batch file:

```bash
thaumaturgia tasks
thaumaturgia runDev
```

### VS Code Tasks

The same tasks are available in VS Code's task runner. Press `Ctrl+Shift+B` to build or `Ctrl+Shift+P` and type "Tasks: Run Task" to see all available tasks.

## Project Structure

Thaumaturgia uses a development structure inspired by Minecraft modding:

- `Thaumaturgia/src/` - Source code
- `Thaumaturgia/assets/` - Game assets (textures, shaders, models, sounds)
- `run/` - The game instance directory created when building/running
  - `assets/` - Copied assets
  - `config/` - Game configuration files
  - `mods/` - Mods directory
  - `saves/` - Game save files

When distributing the game, the structure of the `run/` directory will be mirrored in the distribution package.

## Modding

Modding is intended to be handled via Harmony and BepInEx (after implementation)
Since fields aren't modifiable after compilation, it's recommended to use `Thaumaturgia.Modding.Field` for fields.

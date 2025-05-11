# Thaumaturgia Project Structure

This document describes the structure of the Thaumaturgia project, a voxel-based survival game written in C# with modding capabilities.

## Overview

The project follows a modular architecture with the following main components:

- **Engine**: Core game systems (rendering, input, lifecycle management)
- **Client**: Client-side components (rendering, audio, UI)
- **Server**: Server-side logic (physics, world generation)
- **Networking**: Communication between client and server
- **Modding**: Extensibility framework for mods

## Directory Structure

```
Thaumaturgia-Dev/              # Solution root
├── Thaumaturgia.csproj        # Main project file
├── Thaumaturgia.sln           # Solution file
├── thaumaturgia.ps1           # Gradle-like task runner (PowerShell)
├── thaumaturgia.bat           # Batch wrapper for task runner
├── README.md                  # Main documentation
├── docs/                      # Documentation files
├── run/                       # Runtime environment (created during build)
│   ├── assets/                # Game assets (copied from source)
│   ├── config/                # Configuration files
│   ├── mods/                  # Mods directory
│   └── saves/                 # Game saves
└── Thaumaturgia/              # Source code root
    ├── assets/                # Source assets
    └── src/                   # Source code
        ├── ThaumaturgiaProgram.cs    # Main entry point
        ├── Core/              # Core game systems
        │   ├── Client/        # Client-side systems
        │   │   ├── Audio/     # Audio system
        │   │   └── Rendering/ # Rendering system
        │   ├── Engine/        # Core engine components
        │   ├── Lifecycle/     # Game object lifecycle
        │   ├── Networking/    # Networking components
        │   ├── Registries/    # Game registries
        │   └── Server/        # Server-side components
        ├── Modding/           # Modding framework
        └── Utils/             # Utility classes
```

## Key Components

### Engine (Core/Engine)

The core game engine that manages the game loop, object lifecycle, and primary systems:

- **Engine.cs**: Main engine class that coordinates systems
- **GameTime.cs**: Handles time and frame management
- **InputManager.cs**: Processes keyboard/mouse input
- **GamePaths.cs**: Manages file paths for game assets and data

### Rendering (Core/Client/Rendering)

The rendering system that handles graphics output:

- **Renderer.cs**: Main rendering coordinator
- **ShaderProgram.cs**: OpenGL shader program management
- **ShaderLoader.cs**: Loads and compiles shaders
- **TextureLoader.cs**: Loads and manages textures

### Lifecycle (Core/Lifecycle)

Manages game object lifecycles:

- **IGameObject.cs**: Interface for all game objects

### Modding (Modding)

Framework for extending the game:

- **Field.cs**: Field system for moddable properties
- **IModdableObject.cs**: Interface for objects that can be modded

### Networking (Core/Networking)

Handles client-server communication:

- **Serialization/**: Serialization system for network packets
- **Codecs/**: Encoders/decoders for data types

## Runtime Environment

The game runs from a `run/` directory that contains:

- Compiled game executable (`Thaumaturgia.dll`, `Thaumaturgia.exe`)
- Game assets (copied from source)
- Configuration files
- Mods directory
- Save files

This structure is designed to separate source code from runtime data, similar to how Minecraft modding environments work.

## Development Workflow

The project uses a Gradle-like task system for building and running:

1. Source code is compiled using the `build` task
2. The `setupEnvironment` task creates necessary directories
3. The `copyAssets` task copies assets to the run directory
4. The `runDev` task runs the game in development mode

See the [tasks documentation](tasks.md) for detailed information about the available tasks.

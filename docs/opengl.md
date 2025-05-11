# OpenTK Integration in Thaumaturgia

This document provides an overview of how OpenTK is integrated and used within Thaumaturgia.

## Overview

Thaumaturgia uses OpenTK (Open Toolkit) for graphics rendering, window management, and input handling. OpenTK is a low-level C# binding for OpenGL, OpenAL, and OpenCL that is particularly suited for game development.

## Key Components

### 1. Window Management

The main game window is created and managed in the `ThaumaturgiaProgram.cs` file. This includes:

- Window creation and configuration
- Input binding
- Game loop
- OpenGL context initialization

### 2. Rendering System

The rendering system is structured as follows:

- **Renderer.cs**: Main renderer class that manages the rendering pipeline
- **PrimitiveRenderer.cs**: Renders basic shapes for testing purposes
- **IDisposableResource.cs**: Interface for proper resource management

### 3. Input Handling

Input handling is managed through the `InputManager` class which:

- Maps game actions to keyboard/mouse inputs
- Manages input state tracking
- Provides a query system for game logic

## Game Loop

The game loop is structured with distinct update and render phases:

1. **Update Phase**: Game logic is processed
2. **Render Phase**: Scene is rendered based on current game state

## OpenGL Resource Management

OpenGL resources (VAOs, VBOs, textures, shaders) are managed with a consistent pattern:

1. **Creation**: Resources are created during initialization
2. **Usage**: Resources are bound and used during rendering
3. **Cleanup**: Resources are properly disposed when no longer needed

## Shader System

Shaders are compiled and linked during initialization and used during rendering. The basic shader flow is:

1. Vertex shader processes vertices
2. Fragment shader processes fragments (pixels)

## Example Usage

### Creating a Window

```csharp
// Load settings
var settings = LoadGraphicsSettings();

// Create window
var nativeWindowSettings = new NativeWindowSettings
{
    ClientSize = new Vector2i(settings.ScreenWidth, settings.ScreenHeight),
    Title = "Thaumaturgia",
    APIVersion = new Version(4, 1),
    Flags = ContextFlags.ForwardCompatible,
    Profile = ContextProfile.Core
};

var gameWindowSettings = new GameWindowSettings
{
    UpdateFrequency = 60.0
};

GameWindow window = new GameWindow(gameWindowSettings, nativeWindowSettings);
```

### Setting Up the Game Loop

```csharp
// Create renderer
var renderer = new Renderer(window);

// Update callback
window.UpdateFrame += (FrameEventArgs args) => {
    // Update game logic
    engine.Update();
    
    // Update animations
    renderer.UpdateScene((float)args.Time);
};

// Render callback
window.RenderFrame += (FrameEventArgs args) => {
    // Begin frame
    renderer.BeginFrame();
    
    // Render scene
    renderer.RenderScene();
    
    // End frame (swaps buffers)
    renderer.EndFrame();
};
```

### Running the Game

To run the game with the OpenGL window, you can use the Gradle-like task system:

```powershell
# Clean, build and run the game
.\thaumaturgia.ps1 cleanBuildRun

# Or just run the game if already built
.\thaumaturgia.ps1 runDev
```

Alternatively, use VS Code tasks:
1. Press `Ctrl+Shift+P` and select "Tasks: Run Task"
2. Choose "Clean Build Run" or "Run Dev"

## Troubleshooting

Common issues and their solutions:

1. **Black Screen**: 
   - Ensure shaders are compiling correctly (check console for shader compilation errors)
   - Verify that all OpenGL calls are properly sequenced
   - Make sure the renderer's BeginFrame() and EndFrame() methods are being called

2. **Crashes on Startup**: 
   - Check that OpenTK dependencies are correctly referenced and the run folder contains all necessary DLLs
   - Verify the OpenGL version in NativeWindowSettings is supported by your hardware
   - Run in a clean environment using the cleanBuildRun task

3. **Input Not Working**: 
   - Verify that the InputManager is connected to the game window
   - Check if the window has focus when testing input
   - Debug the keyboard state directly in the update callback

4. **Path Resolution Issues**:
   - Ensure GamePaths.Initialize() is correctly handling run folder detection
   - Check that assets are being copied to the correct location
   - Verify that the application can find shader files and textures

5. **Performance Issues**:
   - Enable VSync for smoother rendering
   - Reduce draw calls by optimizing rendering code
   - Use a profiler to identify bottlenecks

## Future Improvements

- Add texture loading and management
- Implement a camera system
- Add support for 3D model loading

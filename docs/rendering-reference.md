# Thaumaturgia Rendering Quick Reference

This document provides a quick reference for the Thaumaturgia rendering system including the OpenTK integration.

## Core Components

### 1. ThaumaturgiaProgram

The main program that initializes the game window and connects all components:

```csharp
// Create window
GameWindow window = CreateGameWindow(settings);

// Create engine instance
var engine = new Engine();

// Create renderer
var renderer = new Renderer(window);
```

### 2. Renderer

The main rendering manager that handles all rendering operations:

```csharp
// Initialize renderer
var renderer = new Renderer(window);

// Update animations
renderer.UpdateScene(deltaTime);

// Render a frame
renderer.BeginFrame();
renderer.RenderScene();
renderer.EndFrame();
```

### 3. PrimitiveRenderer

Renders basic shapes for testing and debugging:

```csharp
// Create primitive renderer (done in the Renderer class)
_primitiveRenderer = new PrimitiveRenderer();

// Update and render
_primitiveRenderer.Update(deltaTime);
_primitiveRenderer.Render();
```

## Common Tasks

### Creating a New Renderer Component

All renderer components should implement the `IDisposableResource` interface:

```csharp
public class MyRenderer : IDisposableResource
{
    // OpenGL resource IDs
    private int _vao, _vbo;
    
    public MyRenderer()
    {
        Initialize();
    }
    
    private void Initialize()
    {
        // Create OpenGL resources
        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();
        
        // Set up resources...
    }
    
    public void Render()
    {
        // Render logic...
    }
    
    public void Update(float deltaTime)
    {
        // Animation logic...
    }
    
    public void Cleanup()
    {
        // Clean up OpenGL resources
        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
    }
    
    public void Dispose()
    {
        Cleanup();
        GC.SuppressFinalize(this);
    }
}
```

### Creating and Using Shaders

```csharp
// Loading shaders from files
int vertexShader = ShaderLoader.LoadShader(ShaderType.VertexShader, "basic.vert");
int fragmentShader = ShaderLoader.LoadShader(ShaderType.FragmentShader, "basic.frag");

// Creating a shader program
int program = ShaderLoader.CreateShaderProgram("basic.vert", "basic.frag");

// Using a shader program
GL.UseProgram(program);

// Setting uniforms
int location = GL.GetUniformLocation(program, "model");
GL.UniformMatrix4(location, false, ref modelMatrix);
```

### Resource Management

Always follow this pattern for OpenGL resources:

1. **Create**: Generate resource IDs during initialization
2. **Use**: Bind resources, perform operations, unbind
3. **Dispose**: Delete resources when done

```csharp
// Generate
int vao = GL.GenVertexArray();

// Use
GL.BindVertexArray(vao);
// ...operations...
GL.BindVertexArray(0); // Unbind

// Dispose
GL.DeleteVertexArray(vao);
```

## Common Issues

1. **Missing Bindings**: Always ensure VAO is bound before drawing
2. **Resource Leaks**: Always delete OpenGL resources when done
3. **Shader Errors**: Check shader compilation status with GL.GetShaderInfoLog()
4. **Rendering Order**: Clear buffers first, then draw opaque objects, then transparent ones

## Best Practices

1. Group draw calls by shader program to minimize state changes
2. Use VAOs and VBOs for all vertex data
3. Implement proper resource cleanup for all OpenGL objects
4. Enable depth testing for 3D rendering
5. Use instancing for repetitive objects
6. Cache uniform locations for better performance

using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using Thaumaturgia.Core.Engine;

namespace Thaumaturgia.Core.Client.Rendering
{    /// <summary>
    /// Main renderer class for Thaumaturgia
    /// </summary>
    public class Renderer : IDisposableResource
    {        private GameWindow _window;
        private Vector2i _viewportSize;
        private Color4 _clearColor = new Color4(0.1f, 0.2f, 0.3f, 1.0f);
        private PrimitiveRenderer? _primitiveRenderer;public Renderer(GameWindow window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
            _viewportSize = window.ClientSize;
            
            Initialize();
        }

        /// <summary>
        /// Initialize the renderer
        /// </summary>
        public void Initialize()
        {
            // Set up OpenGL
            GL.ClearColor(_clearColor);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            
            // Print out some information about the graphics driver
            Console.WriteLine($"OpenGL Version: {GL.GetString(StringName.Version)}");
            Console.WriteLine($"Graphics Card: {GL.GetString(StringName.Renderer)}");
            Console.WriteLine($"Vendor: {GL.GetString(StringName.Vendor)}");
            Console.WriteLine($"GLSL Version: {GL.GetString(StringName.ShadingLanguageVersion)}");
            
            // Set up viewport
            GL.Viewport(0, 0, _viewportSize.X, _viewportSize.Y);
            
            // Initialize primitive renderer
            _primitiveRenderer = new PrimitiveRenderer();
        }

        /// <summary>
        /// Resize the viewport
        /// </summary>
        public void Resize(Vector2i newSize)
        {
            _viewportSize = newSize;
            GL.Viewport(0, 0, _viewportSize.X, _viewportSize.Y);
        }        /// <summary>
        /// Begin rendering a new frame
        /// </summary>
        public void BeginFrame()
        {
            // Clear the screen
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }        /// <summary>
        /// Update the scene
        /// </summary>
        /// <param name="deltaTime">Time elapsed since the last update</param>
        public void UpdateScene(float deltaTime)
        {
            // Update test primitive if available
            if (_primitiveRenderer != null)
            {
                _primitiveRenderer.Update(deltaTime);
            }
        }
        
        /// <summary>
        /// Render the scene
        /// </summary>
        public void RenderScene()
        {
            // Render test primitive if available
            _primitiveRenderer?.Render();
        }/// <summary>
        /// End the current frame
        /// </summary>
        public void EndFrame()
        {
            // Swap buffers to display the frame
            _window.SwapBuffers();
        }
        
        /// <summary>
        /// Clean up any OpenGL resources
        /// </summary>
        public void Cleanup()
        {
            // Dispose of the primitive renderer
            _primitiveRenderer?.Dispose();
        }
        
        /// <summary>
        /// IDisposable implementation
        /// </summary>
        public void Dispose()
        {
            Cleanup();
            GC.SuppressFinalize(this);
        }
    }
}
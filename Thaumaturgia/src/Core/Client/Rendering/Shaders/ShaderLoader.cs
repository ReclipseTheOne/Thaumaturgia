using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using Thaumaturgia.Core.Engine;

namespace Thaumaturgia.Core.Client.Rendering.Shaders
{
    /// <summary>
    /// Handles loading and compiling shader code
    /// </summary>
    public static class ShaderLoader
    {
        private static readonly string DEFAULT_SHADER_PATH = "assets/thaumaturgia/shaders";

        /// <summary>
        /// Loads and compiles a shader from a file
        /// </summary>
        /// <param name="shaderType">Type of shader to load</param>
        /// <param name="filename">Filename of the shader</param>
        /// <returns>Shader handle</returns>
        public static int LoadShader(ShaderType shaderType, string filename)
        {
            try
            {
                string shaderPath = Path.Combine(GamePaths.AssetsDirectory, "thaumaturgia/shaders", filename);
                string shaderSource;

                // Try to read the shader file
                if (File.Exists(shaderPath))
                {
                    shaderSource = File.ReadAllText(shaderPath);
                }
                else
                {
                    Console.WriteLine($"Shader file not found: {shaderPath}");
                    // Return a basic fallback shader code
                    return CompileShader(shaderType, GetFallbackShaderCode(shaderType));
                }

                // Compile the shader
                return CompileShader(shaderType, shaderSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading shader: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Compiles a shader from source code
        /// </summary>
        /// <param name="shaderType">Type of shader to compile</param>
        /// <param name="source">Source code of the shader</param>
        /// <returns>Shader handle</returns>
        public static int CompileShader(ShaderType shaderType, string source)
        {
            int shader = GL.CreateShader(shaderType);
            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);

            // Check for compilation errors
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(shader);
                Console.WriteLine($"Shader compile error: {infoLog}");
            }

            return shader;
        }

        /// <summary>
        /// Creates a basic shader program from vertex and fragment shaders
        /// </summary>
        /// <param name="vertexShaderPath">Path to vertex shader</param>
        /// <param name="fragmentShaderPath">Path to fragment shader</param>
        /// <returns>Shader program handle</returns>
        public static int CreateShaderProgram(string vertexShaderPath, string fragmentShaderPath)
        {
            // Load and compile shaders
            int vertexShader = LoadShader(ShaderType.VertexShader, vertexShaderPath);
            int fragmentShader = LoadShader(ShaderType.FragmentShader, fragmentShaderPath);

            // Create shader program
            int program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            GL.LinkProgram(program);

            // Check for linking errors
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(program);
                Console.WriteLine($"Program link error: {infoLog}");
            }

            // Detach and delete shaders as they're linked into the program now
            GL.DetachShader(program, vertexShader);
            GL.DetachShader(program, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            return program;
        }
        
        /// <summary>
        /// Get fallback shader code for when shader files are not found
        /// </summary>
        private static string GetFallbackShaderCode(ShaderType shaderType)
        {
            switch (shaderType)
            {
                case ShaderType.VertexShader:
                    return @"
                        #version 330 core
                        layout (location = 0) in vec3 aPosition;
                        layout (location = 1) in vec3 aColor;
                        
                        out vec3 vertexColor;
                        
                        void main()
                        {
                            gl_Position = vec4(aPosition, 1.0);
                            vertexColor = aColor;
                        }
                    ";
                
                case ShaderType.FragmentShader:
                    return @"
                        #version 330 core
                        in vec3 vertexColor;
                        
                        out vec4 FragColor;
                        
                        void main()
                        {
                            FragColor = vec4(vertexColor, 1.0);
                        }
                    ";
                    
                default:
                    return string.Empty;
            }
        }
    }
}
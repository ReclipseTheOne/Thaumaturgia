using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Thaumaturgia.Core.Client.Rendering.Shaders;

namespace Thaumaturgia.Core.Client.Rendering
{
    /// <summary>
    /// Renders primitive shapes for testing purposes
    /// </summary>
    public class PrimitiveRenderer : IDisposableResource
    {
        private int _vao;
        private int _vbo;
        private int _ebo;
        private int _shader; // We'll replace this with ShaderProgram later
        
        // Transformation matrices
        private Matrix4 _model = Matrix4.Identity;
        private Matrix4 _view = Matrix4.CreateTranslation(0f, 0f, -3f);
        private Matrix4 _projection = Matrix4.CreatePerspectiveFieldOfView(
            MathHelper.DegreesToRadians(45f), 16f / 9f, 0.1f, 100f);
        
        private readonly float[] _vertices = {
            // X, Y, Z            R, G, B
            -0.5f, -0.5f, 0.0f,   1.0f, 0.0f, 0.0f, // Bottom left
             0.5f, -0.5f, 0.0f,   0.0f, 1.0f, 0.0f, // Bottom right
             0.5f,  0.5f, 0.0f,   0.0f, 0.0f, 1.0f, // Top right
            -0.5f,  0.5f, 0.0f,   1.0f, 1.0f, 0.0f  // Top left
        };

        private readonly uint[] _indices = {
            0, 1, 2, // First triangle
            2, 3, 0  // Second triangle
        };
        
        private readonly string _vertexShaderSource = @"
            #version 330 core
            layout (location = 0) in vec3 aPosition;
            layout (location = 1) in vec3 aColor;
            
            out vec3 vertexColor;
            
            uniform mat4 model;
            uniform mat4 view;
            uniform mat4 projection;
            
            void main()
            {
                gl_Position = projection * view * model * vec4(aPosition, 1.0);
                vertexColor = aColor;
            }
        ";

        private readonly string _fragmentShaderSource = @"
            #version 330 core
            in vec3 vertexColor;
            
            out vec4 FragColor;
            
            void main()
            {
                FragColor = vec4(vertexColor, 1.0);
            }
        ";

        public PrimitiveRenderer()
        {
            Initialize();
        }

        private void Initialize()
        {
            // Create and compile shaders
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, _vertexShaderSource);
            GL.CompileShader(vertexShader);
            CheckShaderCompileError(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, _fragmentShaderSource);
            GL.CompileShader(fragmentShader);
            CheckShaderCompileError(fragmentShader);

            // Create shader program
            _shader = GL.CreateProgram();
            GL.AttachShader(_shader, vertexShader);
            GL.AttachShader(_shader, fragmentShader);
            GL.LinkProgram(_shader);
            CheckProgramLinkError(_shader);

            // Delete shaders as they're linked into our program now
            GL.DetachShader(_shader, vertexShader);
            GL.DetachShader(_shader, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            // Create VAO, VBO, and EBO
            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
            _ebo = GL.GenBuffer();

            // Bind VAO
            GL.BindVertexArray(_vao);

            // Bind VBO and upload vertex data
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            // Bind EBO and upload index data
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            // Configure vertex attributes
            // Position attribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Color attribute
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // Unbind VBO and VAO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }        
        
        public void Render()
        {
            // Use shader program
            GL.UseProgram(_shader);

            // Set uniforms if we have them
            int modelLocation = GL.GetUniformLocation(_shader, "model");
            int viewLocation = GL.GetUniformLocation(_shader, "view");
            int projectionLocation = GL.GetUniformLocation(_shader, "projection");

            if (modelLocation != -1)
                GL.UniformMatrix4(modelLocation, false, ref _model);
            
            if (viewLocation != -1)
                GL.UniformMatrix4(viewLocation, false, ref _view);
            
            if (projectionLocation != -1)
                GL.UniformMatrix4(projectionLocation, false, ref _projection);

            // Bind VAO
            GL.BindVertexArray(_vao);

            // Draw elements
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            // Unbind VAO
            GL.BindVertexArray(0);
        }
        
        // Add rotation to the model
        public void Update(float deltaTime)
        {
            _model = _model * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(45f * deltaTime));
        }
        
        public void Cleanup()
        {
            GL.DeleteVertexArray(_vao);
            GL.DeleteBuffer(_vbo);
            GL.DeleteBuffer(_ebo);
            GL.DeleteProgram(_shader);
        }
        
        public void Dispose()
        {
            Cleanup();
            GC.SuppressFinalize(this);
        }

        private void CheckShaderCompileError(int shader)
        {
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(shader);
                Console.WriteLine($"Shader compile error: {infoLog}");
            }
        }

        private void CheckProgramLinkError(int program)
        {
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(program);
                Console.WriteLine($"Program link error: {infoLog}");
            }
        }
    }
}

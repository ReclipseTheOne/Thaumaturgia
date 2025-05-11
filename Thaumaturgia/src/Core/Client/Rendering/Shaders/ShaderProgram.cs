using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Thaumaturgia.Core.Client.Rendering.Shaders
{
    public class ShaderProgram
    {
        private readonly int _programId;
        private readonly Dictionary<string, int> _uniformLocations;

        public ShaderProgram()
        {
            _programId = GL.CreateProgram();
            _uniformLocations = new Dictionary<string, int>();
        }

        public void AttachShader(int shaderId)
        {
            GL.AttachShader(_programId, shaderId);
        }

        public void Link()
        {
            GL.LinkProgram(_programId);
            GL.GetProgram(_programId, GetProgramParameterName.LinkStatus, out int status);
            if (status == 0)
            {
                string infoLog = GL.GetProgramInfoLog(_programId);
                throw new Exception($"Error linking shader program: {infoLog}");
            }
        }

        public void Use()
        {
            GL.UseProgram(_programId);
        }

        public void SetUniform(string name, float value)
        {
            int location = GetUniformLocation(name);
            GL.Uniform1(location, value);
        }        public void SetUniform(string name, int value)
        {
            int location = GetUniformLocation(name);
            GL.Uniform1(location, value);
        }
        
        public void SetUniform(string name, Vector2 value)
        {
            int location = GetUniformLocation(name);
            GL.Uniform2(location, value.X, value.Y);
        }
        
        public void SetUniform(string name, Vector3 value)
        {
            int location = GetUniformLocation(name);
            GL.Uniform3(location, value.X, value.Y, value.Z);
        }
        
        public void SetUniform(string name, Vector4 value)
        {
            int location = GetUniformLocation(name);
            GL.Uniform4(location, value.X, value.Y, value.Z, value.W);
        }
        
        public void SetUniform(string name, Matrix4 value)
        {
            int location = GetUniformLocation(name);
            GL.UniformMatrix4(location, false, ref value);
        }
        
        public void SetUniform(string name, Color4 value)
        {
            int location = GetUniformLocation(name);
            GL.Uniform4(location, value.R, value.G, value.B, value.A);
        }

        private int GetUniformLocation(string name)
        {
            if (!_uniformLocations.TryGetValue(name, out int location))
            {
                location = GL.GetUniformLocation(_programId, name);
                if (location == -1)
                {
                    throw new Exception($"Uniform '{name}' not found in shader program.");
                }
                _uniformLocations[name] = location;
            }
            return location;
        }

        public void Dispose()
        {
            GL.DeleteProgram(_programId);
        }
    }
}
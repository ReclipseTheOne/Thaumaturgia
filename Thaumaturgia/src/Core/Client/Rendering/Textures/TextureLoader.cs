using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK.Graphics.OpenGL;
using Thaumaturgia.Core.Engine;
using Thaumaturgia.Utils;
using System.Runtime.Versioning;

namespace Thaumaturgia.Core.Client.Rendering.Textures {
    public class TextureLoader {
        
        /// <summary>
        /// Loads a texture from the specified file path
        /// </summary>
        /// <param name="filePath">Absolute path to the texture file</param>
        /// <returns>OpenGL texture ID</returns>
        [SupportedOSPlatform("windows")]
        public int LoadTexture(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Texture file not found: {filePath}");
            }
            
            return LoadTextureInternal(filePath);
        }
        
        /// <summary>
        /// Loads a texture from the assets/textures folder
        /// </summary>
        /// <param name="textureName">Name of the texture file with extension (e.g., "dirt.png")</param>
        /// <returns>OpenGL texture ID</returns>
        [SupportedOSPlatform("windows")]
        public int LoadTextureFromAssets(string textureName)
        {
            string fullPath = FileUtils.GetTexturePath(textureName);
            return LoadTextureInternal(fullPath);
        }
          [SupportedOSPlatform("windows")]
        private int LoadTextureInternal(string filePath)
        {
            try
            {
                int textureId = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, textureId);

                using (var bitmap = new Bitmap(filePath))
                {
                    var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), 
                                                ImageLockMode.ReadOnly, 
                                                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 
                                bitmap.Width, bitmap.Height, 0, 
                                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, 
                                PixelType.UnsignedByte, data.Scan0);

                    bitmap.UnlockBits(data);
                    
                    // Set texture parameters
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
                }

                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                return textureId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading texture {filePath}: {ex.Message}");
                throw;
            }
        }
    }
}
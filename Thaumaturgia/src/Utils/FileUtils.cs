using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Thaumaturgia.Core.Engine;

namespace Thaumaturgia.Utils
{
    /// <summary>
    /// Utility class for file operations
    /// </summary>
    public static class FileUtils
    {
        /// <summary>
        /// Gets the full path for a file in the assets folder
        /// </summary>
        /// <param name="relativePath">The path relative to the assets folder</param>
        /// <returns>The full path to the file</returns>
        public static string GetAssetPath(string relativePath)
        {
            return GamePaths.GetAssetPath(relativePath);
        }        /// <summary>
        /// Gets the full path for a file in the config folder
        /// </summary>
        /// <param name="relativePath">The path relative to the config folder</param>
        /// <returns>The full path to the file</returns>
        public static string GetConfigPath(string relativePath)
        {
            return GamePaths.GetConfigPath(relativePath);
        }

        /// <summary>
        /// Gets the full path for a file in the save folder
        /// </summary>
        /// <param name="relativePath">The path relative to the save folder</param>
        /// <returns>The full path to the file</returns>
        public static string GetSavePath(string relativePath)
        {
            return GamePaths.GetSavePath(relativePath);
        }

        /// <summary>
        /// Gets the full path for a file in the mods folder
        /// </summary>
        /// <param name="relativePath">The path relative to the mods folder</param>
        /// <returns>The full path to the file</returns>
        public static string GetModPath(string relativePath)
        {
            return GamePaths.GetModPath(relativePath);
        }

        /// <summary>
        /// Loads settings from a JSON file
        /// </summary>
        /// <typeparam name="T">The type to deserialize to</typeparam>
        /// <param name="path">The path to the JSON file</param>
        /// <returns>The deserialized object or default if file doesn't exist</returns>
        public static T? LoadFromJson<T>(string path) where T : class
        {
            if (!File.Exists(path))
            {
                return null;
            }

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<T>(json);
        }        /// <summary>
        /// Saves settings to a JSON file
        /// </summary>
        /// <typeparam name="T">The type to serialize</typeparam>
        /// <param name="path">The path to the JSON file</param>
        /// <param name="data">The object to serialize</param>
        public static void SaveToJson<T>(string path, T data)
        {
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            
            // Create the directory if it doesn't exist
            string? directory = Path.GetDirectoryName(path);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            File.WriteAllText(path, json);
        }
        
        /// <summary>
        /// Loads a shader file from the assets/shaders directory
        /// </summary>
        /// <param name="shaderName">Name of the shader file (without the extension)</param>
        /// <param name="type">Type of shader - "vert" for vertex, "frag" for fragment</param>
        /// <returns>The shader source code</returns>
        public static string LoadShaderSource(string shaderName, string type)
        {
            string extension = type == "vert" ? "vert" : "frag";
            string filename = $"{shaderName}.{extension}";
            return LoadTextAsset(Path.Combine("shaders", filename));
        }
        
        /// <summary>
        /// Loads a texture file and returns its path
        /// </summary>
        /// <param name="textureName">Name of the texture file with extension</param>
        /// <returns>The full path to the texture</returns>
        public static string GetTexturePath(string textureName)
        {
            return GetAssetPath(Path.Combine("textures", textureName));
        }
        
        /// <summary>
        /// Loads a text asset from the assets directory
        /// </summary>
        /// <param name="relativePath">Path relative to the assets directory</param>
        /// <returns>The contents of the file</returns>
        public static string LoadTextAsset(string relativePath)
        {
            string fullPath = GetAssetPath(relativePath);
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"Asset not found: {relativePath}", fullPath);
            }
            
            return File.ReadAllText(fullPath);
        }
        
        /// <summary>
        /// Loads a binary asset from the assets directory
        /// </summary>
        /// <param name="relativePath">Path relative to the assets directory</param>
        /// <returns>The binary contents of the file</returns>
        public static byte[] LoadBinaryAsset(string relativePath)
        {
            string fullPath = GetAssetPath(relativePath);
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"Asset not found: {relativePath}", fullPath);
            }
            
            return File.ReadAllBytes(fullPath);
        }
    }
}

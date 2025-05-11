using System;
using System.IO;

namespace Thaumaturgia.Core.Engine
{
    /// <summary>
    /// Manages game paths for both development and distribution environments
    /// </summary>
    public static class GamePaths
    {        // Base paths
        private static string _baseDir = string.Empty;
        private static string _runDir = string.Empty;

        // Game directories
        private static string _assetsDir = string.Empty;
        private static string _configDir = string.Empty;
        private static string _savesDir = string.Empty;
        private static string _modsDir = string.Empty;
        
        // Development mode flag - affects how paths are resolved
        private static bool _isDevelopmentMode = false;

        /// <summary>
        /// Initialize the game paths system
        /// </summary>
        public static void Initialize(bool developmentMode = false)
        {
            _isDevelopmentMode = developmentMode;
              // In development mode, the run folder is at the solution level or the current directory
            // In distribution mode, the run folder is the same as the executable directory
            _baseDir = AppDomain.CurrentDomain.BaseDirectory;
            
            // Check if we're already in the run folder
            bool alreadyInRunFolder = Path.GetFileName(Path.GetDirectoryName(_baseDir)) == "run" ||
                                      _baseDir.Contains(Path.DirectorySeparatorChar + "run" + Path.DirectorySeparatorChar) ||
                                      _baseDir.EndsWith(Path.DirectorySeparatorChar + "run");
            
            if (_isDevelopmentMode && !alreadyInRunFolder)
            {
                // For development outside of run folder, we need to go up to the solution directory
                // and then to the run folder
                _runDir = Path.GetFullPath(Path.Combine(_baseDir, "../../run"));
                Console.WriteLine($"Development mode: Setting run directory to: {_runDir}");
            }
            else
            {
                // For distribution or if we're already in the run folder,
                // everything is in the same directory as the executable
                _runDir = _baseDir;
                Console.WriteLine($"Using current directory as run directory: {_runDir}");
            }
              // Set up game directories
            _assetsDir = Path.Combine(_runDir, "assets");
            _configDir = Path.Combine(_runDir, "config");
            _savesDir = Path.Combine(_runDir, "saves");
            _modsDir = Path.Combine(_runDir, "mods");
            
            // Print paths for debugging
            Console.WriteLine("------- Path Initialization -------");
            Console.WriteLine($"Base Directory: {_baseDir}");
            Console.WriteLine($"Run Directory: {_runDir}");
            Console.WriteLine($"Assets Directory: {_assetsDir}");
            Console.WriteLine($"Config Directory: {_configDir}");
            Console.WriteLine($"Saves Directory: {_savesDir}");
            Console.WriteLine($"Mods Directory: {_modsDir}");
            
            // Ensure directories exist
            try 
            {
                Directory.CreateDirectory(_runDir);
                Directory.CreateDirectory(_assetsDir);
                Directory.CreateDirectory(_configDir);
                Directory.CreateDirectory(_savesDir);
                Directory.CreateDirectory(_modsDir);
                
                // Create assets subdirectories
                Directory.CreateDirectory(Path.Combine(_assetsDir, "textures"));
                Directory.CreateDirectory(Path.Combine(_assetsDir, "shaders"));
                Directory.CreateDirectory(Path.Combine(_assetsDir, "models"));
                Directory.CreateDirectory(Path.Combine(_assetsDir, "sounds"));
                
                Console.WriteLine("All directories created successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating directories: {ex.Message}");
            }
            
            Console.WriteLine($"Game paths initialized. Running in {(_isDevelopmentMode ? "DEVELOPMENT" : "DISTRIBUTION")} mode");
            Console.WriteLine($"Run directory: {_runDir}");
            Console.WriteLine("-----------------------------------");
        }
        
        /// <summary>
        /// Gets the absolute path to the run directory
        /// </summary>
        public static string RunDirectory => _runDir;
        
        /// <summary>
        /// Gets the absolute path to the assets directory
        /// </summary>
        public static string AssetsDirectory => _assetsDir;
        
        /// <summary>
        /// Gets the absolute path to the config directory
        /// </summary>
        public static string ConfigDirectory => _configDir;
        
        /// <summary>
        /// Gets the absolute path to the saves directory
        /// </summary>
        public static string SavesDirectory => _savesDir;
        
        /// <summary>
        /// Gets the absolute path to the mods directory
        /// </summary>
        public static string ModsDirectory => _modsDir;
        
        /// <summary>
        /// Gets a path within the assets directory
        /// </summary>
        /// <param name="relativePath">Path relative to the assets directory</param>
        /// <returns>The absolute path to the asset</returns>
        public static string GetAssetPath(string relativePath)
        {
            return Path.Combine(_assetsDir, relativePath);
        }
        
        /// <summary>
        /// Gets a path within the config directory
        /// </summary>
        /// <param name="relativePath">Path relative to the config directory</param>
        /// <returns>The absolute path to the config file</returns>
        public static string GetConfigPath(string relativePath)
        {
            return Path.Combine(_configDir, relativePath);
        }
        
        /// <summary>
        /// Gets a path within the saves directory
        /// </summary>
        /// <param name="relativePath">Path relative to the saves directory</param>
        /// <returns>The absolute path to the save file</returns>
        public static string GetSavePath(string relativePath)
        {
            return Path.Combine(_savesDir, relativePath);
        }
        
        /// <summary>
        /// Gets a path within the mods directory
        /// </summary>
        /// <param name="relativePath">Path relative to the mods directory</param>
        /// <returns>The absolute path to the mod file</returns>
        public static string GetModPath(string relativePath)
        {
            return Path.Combine(_modsDir, relativePath);
        }
    }
}

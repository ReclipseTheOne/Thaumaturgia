using System;
using System.IO;
using System.Text.Json;
using Thaumaturgia.Core.Engine;
using Thaumaturgia.Core.Client.Rendering;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;

namespace Thaumaturgia
{
    public class ThaumaturgiaProgram
    {
        // Constants for development/distribution configuration
        public const bool DEVELOPMENT_MODE = true; // Set to false for final builds
        
        // Window settings
        private const int DEFAULT_WIDTH = 1280;
        private const int DEFAULT_HEIGHT = 720;
        private const string WINDOW_TITLE = "Thaumaturgia";
          
        public static void Main(string[] args)
        {
            // Initialize game paths system
            GamePaths.Initialize(DEVELOPMENT_MODE);
            
            // Setup run environment
            SetupRunEnvironment();
            
            try
            {
                Console.WriteLine("Starting Thaumaturgia...");
                Console.WriteLine($"Current Directory: {Environment.CurrentDirectory}");
                Console.WriteLine($"Executable Path: {AppDomain.CurrentDomain.BaseDirectory}");
                
                // Load settings from config
                var settings = LoadGraphicsSettings();
                
                // Create window
                Console.WriteLine($"Creating game window ({settings.ScreenWidth}x{settings.ScreenHeight})...");
                GameWindow window = CreateGameWindow(settings);
                
                // Create engine instance
                Console.WriteLine("Creating engine instance...");
                var engine = new Engine();
                
                // Connect the input manager to the window
                engine.INPUT.SetWindow(window);
                  // Initialize the engine
                Console.WriteLine("Initializing engine...");
                engine.Initialize();
                  // Create renderer
                Console.WriteLine("Creating renderer...");
                var renderer = new Renderer(window);
                
                // Setup callbacks
                window.UpdateFrame += (FrameEventArgs args) => {
                    // Update game logic
                    engine.Update();
                    
                    // Update rendering (animation)
                    renderer.UpdateScene((float)args.Time);
                };
                  
                window.RenderFrame += (FrameEventArgs args) => {
                    // Render frame
                    renderer.BeginFrame();
                    renderer.RenderScene();
                    renderer.EndFrame();
                };
                
                window.Resize += (ResizeEventArgs args) => {
                    // Handle window resize events
                    renderer.Resize(new Vector2i(args.Width, args.Height));
                    Console.WriteLine($"Window resized to {args.Width}x{args.Height}");
                };
                  window.Closing += (System.ComponentModel.CancelEventArgs args) => {
                    Console.WriteLine("Window closing...");
                    
                    // Clean up renderer resources
                    renderer.Dispose();
                };
                
                // Handle keyboard input for testing
                window.KeyDown += (KeyboardKeyEventArgs args) => {
                    if (args.Key == Keys.Escape)
                    {
                        window.Close();
                    }
                };
                  // Run the game
                Console.WriteLine("Game initialized successfully. Starting main loop...");
                try 
                {
                    window.Run();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("==========================================");
                    Console.WriteLine("ERROR: An exception occurred during game loop:");
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("==========================================");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("==========================================");
                Console.WriteLine("ERROR: An exception occurred during startup:");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("==========================================");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Creates the game window based on settings
        /// </summary>
        private static GameWindow CreateGameWindow(GraphicsSettings settings)
        {            var nativeWindowSettings = new NativeWindowSettings
            {
                ClientSize = new Vector2i(settings.ScreenWidth, settings.ScreenHeight),
                Title = WINDOW_TITLE,
                // Use OpenGL 4.1
                APIVersion = new Version(4, 1),
                Flags = ContextFlags.ForwardCompatible,
                Profile = ContextProfile.Core,
                NumberOfSamples = 4 // Anti-aliasing
            };
              var gameWindowSettings = new GameWindowSettings
            {
                UpdateFrequency = settings.VSync ? 60.0 : 0.0
            };
            
            return new GameWindow(gameWindowSettings, nativeWindowSettings);
        }
        
        /// <summary>
        /// Load graphics settings from config or return defaults
        /// </summary>
        private static GraphicsSettings LoadGraphicsSettings()
        {
            string path = GamePaths.GetConfigPath("settings.json");
            if (File.Exists(path))
            {
                try
                {
                    string json = File.ReadAllText(path);
                    var settings = JsonSerializer.Deserialize<GameSettings>(json);
                    return settings?.GraphicsSettings ?? new GraphicsSettings 
                    {
                        ScreenWidth = DEFAULT_WIDTH,
                        ScreenHeight = DEFAULT_HEIGHT,
                        Fullscreen = false,
                        VSync = true,
                        RenderDistance = 16
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading settings: {ex.Message}");
                }
            }
            
            return new GraphicsSettings
            {
                ScreenWidth = DEFAULT_WIDTH,
                ScreenHeight = DEFAULT_HEIGHT,
                Fullscreen = false,
                VSync = true,
                RenderDistance = 16
            };
        }

        /// <summary>
        /// Sets up the run environment, creating necessary folders and files
        /// </summary>
        private static void SetupRunEnvironment()
        {
            // Create default config file if it doesn't exist
            CreateDefaultConfigFile(GamePaths.GetConfigPath("settings.json"));
            
            Console.WriteLine("Run environment setup complete!");
        }
          /// <summary>
        /// Creates a default configuration file if it doesn't exist
        /// </summary>
        /// <param name="path">The path to the configuration file</param>
        private static void CreateDefaultConfigFile(string path)
        {
            if (!File.Exists(path))
            {
                var defaultSettings = new GameSettings
                {
                    GraphicsSettings = new GraphicsSettings
                    {
                        ScreenWidth = 1280,
                        ScreenHeight = 720,
                        Fullscreen = false,
                        VSync = true,
                        RenderDistance = 16
                    },
                    AudioSettings = new AudioSettings
                    {
                        MasterVolume = 0.8f,
                        MusicVolume = 0.5f,
                        SoundEffectsVolume = 1.0f,
                        AmbientVolume = 0.7f
                    },
                    ControlSettings = new ControlSettings
                    {
                        MouseSensitivity = 0.5f,
                        InvertMouseY = false
                    }
                };
                
                string json = JsonSerializer.Serialize(defaultSettings, new JsonSerializerOptions 
                {
                    WriteIndented = true
                });
                
                File.WriteAllText(path, json);
                Console.WriteLine($"Created default settings file: {path}");
            }
        }
    }

    /// <summary>
    /// Represents the game settings
    /// </summary>
    public class GameSettings
    {
        public GraphicsSettings GraphicsSettings { get; set; } = new GraphicsSettings();
        public AudioSettings AudioSettings { get; set; } = new AudioSettings();
        public ControlSettings ControlSettings { get; set; } = new ControlSettings();
    }

    /// <summary>
    /// Represents graphics settings
    /// </summary>
    public class GraphicsSettings
    {
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public bool Fullscreen { get; set; }
        public bool VSync { get; set; }
        public int RenderDistance { get; set; }
    }

    /// <summary>
    /// Represents audio settings
    /// </summary>
    public class AudioSettings
    {
        public float MasterVolume { get; set; }
        public float MusicVolume { get; set; }
        public float SoundEffectsVolume { get; set; }
        public float AmbientVolume { get; set; }
    }

    /// <summary>
    /// Represents control settings
    /// </summary>
    public class ControlSettings
    {
        public float MouseSensitivity { get; set; }
        public bool InvertMouseY { get; set; }
    }
}
using System;
using System.Collections.Generic;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Thaumaturgia.Core.Engine {
    /// <summary>
    /// Represents a game action that can be mapped to a keyboard key
    /// </summary>
    public enum GameAction
    {
        MoveForward,
        MoveBackward,
        MoveLeft,
        MoveRight,
        Jump,
        Sprint,
        Interact,
        Inventory,
        Pause,
        Exit
    }

    /// <summary>
    /// Manages keyboard and mouse input with configurable key bindings
    /// </summary>
    public class InputManager {
        private Dictionary<GameAction, Keys> _keyBindings = new();
        private Dictionary<GameAction, bool> _currentState = new();
        private Dictionary<GameAction, bool> _previousState = new();
        
        // Track current mouse position
        private Vector2 _mousePosition = Vector2.Zero;
        
        // Reference to the keyboard and mouse state
        private KeyboardState? _keyboardState;
        private MouseState? _mouseState;
        
        // Reference to the window for input access
        private GameWindow? _window;        public InputManager() {
            InitializeDefaultBindings();
            
            // Initialize states
            foreach (GameAction action in Enum.GetValues(typeof(GameAction))) {
                _currentState[action] = false;
                _previousState[action] = false;
            }
        }
        
        /// <summary>
        /// Sets the window to get input from
        /// </summary>
        public void SetWindow(GameWindow window) {
            _window = window;
        }

        private void InitializeDefaultBindings() {
            _keyBindings = new Dictionary<GameAction, Keys> {
                { GameAction.MoveForward, Keys.W },
                { GameAction.MoveBackward, Keys.S },
                { GameAction.MoveLeft, Keys.A },
                { GameAction.MoveRight, Keys.D },
                { GameAction.Jump, Keys.Space },
                { GameAction.Sprint, Keys.LeftShift },
                { GameAction.Interact, Keys.E },
                { GameAction.Inventory, Keys.Tab },
                { GameAction.Pause, Keys.Escape },
                { GameAction.Exit, Keys.Escape }
            };
        }        /// <summary>
        /// Updates the input state for the current frame
        /// </summary>
        public void Update() {
            try {
                // Store previous state
                foreach (GameAction action in Enum.GetValues(typeof(GameAction))) {
                    _previousState[action] = _currentState[action];
                }

                // Update current state if we have a window reference
                if (_window != null) {
                    _keyboardState = _window.KeyboardState;
                    _mouseState = _window.MouseState;
                    
                    // Update key states
                    foreach (var binding in _keyBindings) {
                        _currentState[binding.Key] = _keyboardState.IsKeyDown(binding.Value);
                    }
                    
                    // Update mouse position
                    _mousePosition = new Vector2(_mouseState.X, _mouseState.Y);
                }                else {
                    // Use a neutral state when no window is available
                    foreach (var action in _currentState.Keys.ToArray()) {
                        _currentState[action] = false;
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Error updating input: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Updates the input state using provided keyboard and mouse states
        /// (For use when direct window access is not available)
        /// </summary>
        public void Update(KeyboardState keyboardState, MouseState mouseState) {
            // Store previous state
            foreach (GameAction action in Enum.GetValues(typeof(GameAction))) {
                _previousState[action] = _currentState[action];
            }
            
            _keyboardState = keyboardState;
            _mouseState = mouseState;
            
            // Update key states
            foreach (var binding in _keyBindings) {
                _currentState[binding.Key] = _keyboardState.IsKeyDown(binding.Value);
            }
            
            // Update mouse position
            _mousePosition = new Vector2(_mouseState.X, _mouseState.Y);
        }

        /// <summary>
        /// Rebinds a game action to a new key
        /// </summary>
        /// <param name="action">The game action to rebind</param>
        /// <param name="key">The new key to bind to</param>
        public void RebindKey(GameAction action, Keys key) {
            _keyBindings[action] = key;
        }

        /// <summary>
        /// Checks if a game action is currently being performed
        /// </summary>
        /// <param name="action">The game action to check</param>
        /// <returns>True if the key for the action is pressed, false otherwise</returns>
        public bool IsActionActive(GameAction action) {
            return _currentState.TryGetValue(action, out bool value) && value;
        }

        /// <summary>
        /// Checks if a game action was just pressed this frame
        /// </summary>
        /// <param name="action">The game action to check</param>
        /// <returns>True if the key was just pressed, false otherwise</returns>
        public bool IsActionPressed(GameAction action) {
            return _currentState.TryGetValue(action, out bool current) && current && 
                   _previousState.TryGetValue(action, out bool previous) && !previous;
        }

        /// <summary>
        /// Checks if a game action was just released this frame
        /// </summary>
        /// <param name="action">The game action to check</param>
        /// <returns>True if the key was just released, false otherwise</returns>
        public bool IsActionReleased(GameAction action) {
            return _previousState.TryGetValue(action, out bool previous) && previous && 
                   _currentState.TryGetValue(action, out bool current) && !current;
        }

        /// <summary>
        /// Gets the key currently bound to an action
        /// </summary>
        /// <param name="action">The game action to get the key for</param>
        /// <returns>The Key bound to the action</returns>
        public Keys GetBoundKey(GameAction action) {
            return _keyBindings.TryGetValue(action, out Keys key) ? key : Keys.Unknown;
        }

        /// <summary>
        /// Gets the current mouse position
        /// </summary>
        /// <returns>A Vector2 containing the X and Y coordinates of the mouse</returns>
        public Vector2 GetMousePosition() {
            return _mousePosition;
        }
    }
}
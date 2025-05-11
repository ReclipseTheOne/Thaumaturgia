using System;
using System.Collections.Generic;
using Thaumaturgia.Core.Lifecycle;

namespace Thaumaturgia.Core.Engine {
    public class Engine
    {
        private List<IGameObject> _gameObjects;
        private GameTime _gameTime;
        private InputManager _inputManager;

        public Engine()
        {
            _gameObjects = new List<IGameObject>();
            _gameTime = new GameTime();
            _inputManager = new InputManager();
        }

        /// <summary>
        /// Gets the input manager instance
        /// </summary>
        public InputManager INPUT => _inputManager;

        public void Initialize()
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Initialize();
            }
        }        public void Update()
        {
            try
            {
                // Update input state first
                _inputManager.Update();

                // Handle exit action
                if (_inputManager.IsActionActive(GameAction.Exit))
                {
                    Console.WriteLine("Exit requested. Shutting down...");
                    Environment.Exit(0);
                }
                
                // Update game time and objects
                _gameTime.Update();
                foreach (var gameObject in _gameObjects)
                {
                    gameObject.Update(_gameTime);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in engine update: {ex.Message}");
                // Continue execution rather than crashing
            }
        }

        public void Render()
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Render();
            }
        }

        public void AddGameObject(IGameObject gameObject)
        {
            _gameObjects.Add(gameObject);
        }
    }
}
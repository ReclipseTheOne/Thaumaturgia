using System;
using Thaumaturgia.Core.Engine;
using Thaumaturgia.Core.Lifecycle;

namespace Thaumaturgia {
    public class Thaumaturgia
    {
        private List<IGameObject> _gameObjects;

        public Thaumaturgia()
        {
            _gameObjects = new List<IGameObject>();
        }

        public void Initialize()
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Initialize();
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Update(gameTime);
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
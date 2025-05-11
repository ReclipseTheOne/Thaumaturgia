using Thaumaturgia.Core.Engine;

namespace Thaumaturgia.Core.Lifecycle
{
    public interface IGameObject
    {   
        bool Tickable { get; set;}
        bool Renderable {get; set;}
        void Initialize();
        void Update(GameTime gameTime);
        void Render();
    }
}
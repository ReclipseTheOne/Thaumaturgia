using System;
using Thaumaturgia.Utils;

namespace Thaumaturgia.Core.Registries
{
    public interface IRegistry<T> where T : class {
        ResourceLocation Location { get; }
        IRegistryObject<T> Get(string name);
        List<IRegistryObject<T>> GetAllObjects();
        List<string> GetAllKeys();
        
        IRegistryObject<T> Register(string name, Func<T> factory);
    }
}
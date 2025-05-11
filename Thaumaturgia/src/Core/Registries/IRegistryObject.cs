using System;
using Thaumaturgia.Utils;

namespace Thaumaturgia.Core.Registries {
    public interface IRegistryObject<T> where T : class {
        ResourceLocation Location { get; }
        T Get();
    }
}
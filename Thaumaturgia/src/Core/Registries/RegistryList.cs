using System;
using Thaumaturgia.Utils;

namespace Thaumaturgia.Core.Registries
{
    public class RegistryList {
        public static RegistryList Instance { get; } = new();

        private RegistryList() { }

        public Dictionary<ResourceLocation, IRegistry<object>> Registries { get; } = new();

        public void AddRegistry(IRegistry<object> registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));
            
            if (Registries.ContainsKey(registry.Location))
                throw new ArgumentException($"Registry with location {registry.Location} already exists.");

            Registries[registry.Location] = registry;
        }
    }
}
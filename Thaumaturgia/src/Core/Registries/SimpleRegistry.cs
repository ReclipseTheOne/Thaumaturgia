using System;
using Thaumaturgia.Utils;

namespace Thaumaturgia.Core.Registries {
    public class SimpleRegistry<T> : IRegistry<T> where T : class {
        public ResourceLocation Location { get; }

        private readonly Dictionary<string, IRegistryObject<T>> _registryObjects = new();

        public SimpleRegistry(ResourceLocation location) {
            Location = location ?? throw new ArgumentNullException(nameof(location));
        }

        public IRegistryObject<T> Get(string name) {
            if (_registryObjects.TryGetValue(name, out var registryObject)) {
                return registryObject;
            } else {
                throw new KeyNotFoundException($"Registry object with name {name} not found.");
            }
        }

        public List<IRegistryObject<T>> GetAllObjects() {
            return _registryObjects.Values.ToList();
        }

        public List<string> GetAllKeys() {
            return _registryObjects.Keys.ToList();
        }

        public IRegistryObject<T> Register(ResourceLocation location, Func<T> factory) {
            if (location == null)
                throw new ArgumentException("Location cannot be null.", nameof(location));
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            
            SimpleRegistryObject<T> registryObject = new SimpleRegistryObject<T>(location, factory);

            _registryObjects[location.Path] = registryObject;

            return registryObject;
        }

        public IRegistryObject<T> Register(string name, Func<T> factory) {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
   
            return Register(Location.AsChild(name), factory);
        }
    }
}
using System;
using Thaumaturgia.Utils;

namespace Thaumaturgia.Core.Registries {
    public class SimpleRegistryObject<T> : IRegistryObject<T> where T : class {
        public ResourceLocation Location { get; }
        private Func<T> _factory;
        private T _instance;

        public SimpleRegistryObject(ResourceLocation location, Func<T> factory) {
            Location = location ?? throw new ArgumentNullException(nameof(location));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _instance = _factory();
        }

        public T Get() {
            if (_instance == null) {
                _instance = _factory();
            }
            return _instance;
        }
    }
}
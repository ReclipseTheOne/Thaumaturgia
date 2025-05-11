using System;
using Thaumaturgia.Utils;
using Thaumaturgia.Core.World;

namespace Thaumaturgia.Core.Registries
{
    public static class Blocks {
        public static SimpleRegistry<Block> BLOCKS = new SimpleRegistry<Block>(ResourceLocation.FromDefaultNamespace("blocks"));

        public static IRegistryObject<Block> DIRT = Register("dirt", () => new Block(0, 0, 0, ResourceLocation.FromDefaultNamespace("dirt")));

        public static IRegistryObject<Block> Register(string name, Func<Block> factory) {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            var location = ResourceLocation.FromDefaultNamespace(name);
            return BLOCKS.Register(location, factory);
        }
    }
}
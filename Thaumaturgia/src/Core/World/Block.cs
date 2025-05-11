using Thaumaturgia.Utils;
using System;
using Thaumaturgia.Modding;

namespace Thaumaturgia.Core.World {
    public class Block : ModdableObject {
        private Func<Block> _factory;
        public ResourceLocation Location { get; }
        public SerializableField<int> X { get; set; } = new SerializableField<int>(Codecs.IntCodec, 0);
        public SerializableField<int> Y { get; set; } = new SerializableField<int>(Codecs.IntCodec, 0);
        public SerializableField<int> Z { get; set; } = new SerializableField<int>(Codecs.IntCodec, 0);


        public Block (int x, int y, int z, ResourceLocation location)
        {
            Location = location ?? throw new ArgumentNullException(nameof(location));
            X.Value = x;
            Y.Value = y;
            Z.Value = z;

            AddField("x", X);
            AddField("y", Y);
            AddField("z", Z);
            AddField("location", new SerializableField<ResourceLocation>(Codecs.ResourceLocationCodec, location));
        }
    }
}
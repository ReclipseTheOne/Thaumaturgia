using System.ComponentModel.DataAnnotations;
using Thaumaturgia.Core.Networking.Serialization;
using Thaumaturgia.Core.Networking.Serialization.Codecs;

namespace Thaumaturgia.Utils {
    class Codecs {
        public static ICodec<int> IntCodec = new SimpleCodec<int>();
        public static ICodec<short> ShortCodec = new SimpleCodec<short>();
        public static ICodec<long> LongCodec = new SimpleCodec<long>();
        public static ICodec<float> FloatCodec = new SimpleCodec<float>();
        public static ICodec<string> StringCodec = new SimpleCodec<string>();
        public static ICodec<bool> BoolCodec = new SimpleCodec<bool>();
        public static ICodec<byte[]> ByteArrayCodec = new SimpleCodec<byte[]>();
        public static ICodec<ResourceLocation> ResourceLocationCodec = new TupleCodec<String, String, ResourceLocation>(
            StringCodec,
            StringCodec,
            (ns, path) => new ResourceLocation(ns, path),
            (rl) => rl.NameSpace,
            (rl) => rl.Path
        );
    }
}
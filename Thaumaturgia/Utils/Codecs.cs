using Thaumaturgia.Core.Networking.Serialization;

namespace Thaumaturgia.Utils {
    class Codecs {
        public static ICodec<int> IntCodec = new SimpleCodec<int>();
        public static ICodec<short> ShortCodec = new SimpleCodec<short>();
        public static ICodec<long> LongCodec = new SimpleCodec<long>();
        public static ICodec<float> FloatCodec = new SimpleCodec<float>();
        public static ICodec<string> StringCodec = new SimpleCodec<string>();
        public static ICodec<bool> BoolCodec = new SimpleCodec<bool>();
        public static ICodec<byte[]> ByteArrayCodec = new SimpleCodec<byte[]>();
    }
}
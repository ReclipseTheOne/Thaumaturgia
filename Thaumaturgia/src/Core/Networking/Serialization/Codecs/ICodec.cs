using System.Text;
using System.Text.Json;

namespace Thaumaturgia.Core.Networking.Serialization.Codecs {
    public interface ICodec<T> {
        void Encode(BinaryWriter writter, T value);
        T Decode(BinaryReader reader);
        string SerializeToJson(T value);
        T DeserializeFromJson(string json);
    }
}

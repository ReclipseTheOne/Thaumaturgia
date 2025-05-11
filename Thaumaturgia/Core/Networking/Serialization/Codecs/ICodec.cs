using System.Text;
using System.Text.Json;

namespace Thaumaturgia.Core.Networking.Serialization {
    public interface ICodec<T> {
        byte[] SerializeToBytes(T value);
        T DeserializeFromBytes(byte[] bytes);
        string SerializeToJson(T value);
        T DeserializeFromJson(string json);
    }
}

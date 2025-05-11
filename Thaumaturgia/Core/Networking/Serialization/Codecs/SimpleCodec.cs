using System.Text;
using System.Text.Json;

namespace Thaumaturgia.Core.Networking.Serialization {
    public class SimpleCodec<T> : ICodec<T> {   
        private JsonSerializerOptions _options;

        public SimpleCodec()
        {
            _options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
        }
        
        public byte[] SerializeToBytes(T value)
        {
            string json = SerializeToJson(value);
            return Encoding.UTF8.GetBytes(json);
        }
        
        public T DeserializeFromBytes(byte[] bytes)
        {
            string json = Encoding.UTF8.GetString(bytes);
            return DeserializeFromJson(json);
        }
        
        public string SerializeToJson(T value)
        {
            return JsonSerializer.Serialize(value, _options);
        }
        
        public T DeserializeFromJson(string json)
        {
            T toRet = JsonSerializer.Deserialize<T>(json, _options) 
                      ?? throw new InvalidOperationException("Deserialization resulted in a null object.");
            return toRet;
        }
    }
}
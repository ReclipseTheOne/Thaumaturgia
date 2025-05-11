using System.Text;
using System.Text.Json;
using System.IO;

namespace Thaumaturgia.Core.Networking.Serialization.Codecs {
    public class SimpleCodec<T> : ICodec<T> {   
        private JsonSerializerOptions _options;

        public SimpleCodec()
        {
            _options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
        }
        
        public void Encode(BinaryWriter writer, T value)
        {
            string json = SerializeToJson(value);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            writer.Write(bytes.Length);
            writer.Write(bytes);
        }
        
        public T Decode(BinaryReader reader)
        {
            int length = reader.ReadInt32();
            byte[] bytes = reader.ReadBytes(length);
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
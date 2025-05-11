using System.Text;
using System.Text.Json;

namespace Thaumaturgia.Core.Networking.Serialization {
    public class CompositeCodec<T> : ICodec<T> where T : new()
    {
        private readonly List<IFieldCodec> _fieldCodecs = new List<IFieldCodec>();
        
        public byte[] SerializeToBytes(T value)
        {
            var jsonString = SerializeToJson(value);
            return System.Text.Encoding.UTF8.GetBytes(jsonString);
        }
        
        public T DeserializeFromBytes(byte[] bytes)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(bytes);
            return DeserializeFromJson(jsonString);
        }
        
        public string SerializeToJson(T value)
        {
            using var doc = JsonDocument.Parse("{}");
            var jsonObj = new Dictionary<string, JsonElement>();
            
            // Serialize each field
            foreach (var fieldCodec in _fieldCodecs)
            {
                fieldCodec.SerializeField(value, jsonObj);
            }
            
            return JsonSerializer.Serialize(jsonObj);
        }
        
        public T DeserializeFromJson(string json)
        {
            var jsonObj = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json) ?? 
                          throw new InvalidOperationException("Deserialization resulted in a null object.");
            var result = new T();
            
            // Deserialize each field
            foreach (var fieldCodec in _fieldCodecs)
            {
                fieldCodec.DeserializeField(jsonObj, result);
            }
            
            return result;
        }
        
        public void AddField<TField>(string fieldName, ICodec<TField> codec, 
                                   Func<T, TField> getter, Action<T, TField> setter)
        {
            _fieldCodecs.Add(new FieldCodec<TField>(fieldName, codec, getter, setter));
        }
        
        
        private interface IFieldCodec
        {
            void SerializeField(T obj, Dictionary<string, JsonElement> jsonObj);
            void DeserializeField(Dictionary<string, JsonElement> jsonObj, T obj);
        }
        
        private class FieldCodec<TField> : IFieldCodec
        {
            private readonly string _fieldName;
            private readonly ICodec<TField> _codec;
            private readonly Func<T, TField> _getter;
            private readonly Action<T, TField> _setter;
            
            public FieldCodec(string fieldName, ICodec<TField> codec, 
                            Func<T, TField> getter, Action<T, TField> setter)
            {
                _fieldName = fieldName;
                _codec = codec;
                _getter = getter;
                _setter = setter;
            }
            
            public void SerializeField(T obj, Dictionary<string, JsonElement> jsonObj)
            {
                var fieldValue = _getter(obj);
                var jsonString = _codec.SerializeToJson(fieldValue);
                var element = JsonSerializer.Deserialize<JsonElement>(jsonString);
                jsonObj[_fieldName] = element;
            }
            
            public void DeserializeField(Dictionary<string, JsonElement> jsonObj, T obj)
            {
                if (jsonObj.TryGetValue(_fieldName, out var element))
                {
                    var jsonString = element.GetRawText();
                    var fieldValue = _codec.DeserializeFromJson(jsonString);
                    _setter(obj, fieldValue);
                }
            }
        }
    }

    public class CompositeCodecBuilder<T> where T : new()
    {
        private readonly CompositeCodec<T> _codec = new CompositeCodec<T>();
        
        public CompositeCodecBuilder<T> AddField<TField>(
            string fieldName, 
            ICodec<TField> codec,
            Func<T, TField> getter,
            Action<T, TField> setter)
        {
            _codec.AddField(fieldName, codec, getter, setter);
            return this;
        }
        
        public ICodec<T> Build()
        {
            return _codec;
        }
    }
}
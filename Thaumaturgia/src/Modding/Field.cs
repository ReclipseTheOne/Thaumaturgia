using System;
using Thaumaturgia.Core.Networking.Serialization;
using Thaumaturgia.Core.Networking.Serialization.Codecs;

namespace Thaumaturgia.Modding
{    public interface IField
    {
        Type GetValueType();
        object? GetObjectValue();
        void SetObjectValue(object value);
    }
      public class Field<T> : IField {
        protected T _value = default!;
        
        public Field(T? initialValue = default) {   
            _value = initialValue is null ? default! : initialValue;
        }
        
        public T Value
        {
            get => _value;
            set => _value = value;
        }
        
        public Type GetValueType() => typeof(T);
        
        public object? GetObjectValue() => _value;
        
        public void SetObjectValue(object value)
        {
            if (value is T typedValue)
                _value = typedValue;
            else
                throw new InvalidCastException($"Cannot cast to {typeof(T).Name}");
        }
    }    public class SerializableField<T> : Field<T>, ISerializable<T>
    {
        public readonly ICodec<T> _codec;
        public ICodec<T> Codec => _codec;

        public SerializableField(ICodec<T> codec, T? initialValue = default) : base(initialValue)
        {
            _codec = codec ?? throw new ArgumentNullException(nameof(codec));
        }
    }
}
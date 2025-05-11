using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Thaumaturgia.Utils;

namespace Thaumaturgia.Core.Networking.Serialization.Codecs {
    #region TupleCodec1
    public class TupleCodec<T1, TResult> : ICodec<TResult>
    {
        private readonly ICodec<T1> _codec1;
        private readonly Func<T1, TResult> _factory;
        private readonly Func<TResult, T1> _getter1;

        public TupleCodec(
            ICodec<T1> codec1, 
            Func<T1, TResult> factory, 
            Func<TResult, T1> getter1)
        {
            _codec1 = codec1 ?? throw new ArgumentNullException(nameof(codec1));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _getter1 = getter1 ?? throw new ArgumentNullException(nameof(getter1));
        }

        public void Encode(BinaryWriter writer, TResult value)
        {
            _codec1.Encode(writer, _getter1(value));
        }        public TResult Decode(BinaryReader reader)
        {
            T1 val1 = _codec1.Decode(reader);
            return _factory(val1);
        }
          public string SerializeToJson(TResult value)
        {
            var jsonObj = new object[]
            {
                JsonSerializer.Deserialize<object>(_codec1.SerializeToJson(_getter1(value))) ?? new object()
            };
            return JsonSerializer.Serialize(jsonObj);
        }
        
        public TResult DeserializeFromJson(string json)
        {
            var jsonArray = JsonSerializer.Deserialize<object[]>(json) ?? 
                throw new InvalidOperationException("Deserialization resulted in a null array.");
                
            if (jsonArray.Length != 1)
                throw new InvalidOperationException($"Expected array of length 1, got {jsonArray.Length}");
                
            var val1 = _codec1.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[0]));
            
            return _factory(val1);
        }
    }
    #endregion

    #region TupleCodec2
    public class TupleCodec<T1, T2, TResult> : ICodec<TResult>
    {
        private readonly ICodec<T1> _codec1;
        private readonly ICodec<T2> _codec2;
        private readonly Func<T1, T2, TResult> _factory;
        private readonly Func<TResult, T1> _getter1;
        private readonly Func<TResult, T2> _getter2;

        public TupleCodec(
            ICodec<T1> codec1, 
            ICodec<T2> codec2,
            Func<T1, T2, TResult> factory, 
            Func<TResult, T1> getter1,
            Func<TResult, T2> getter2)
        {
            _codec1 = codec1 ?? throw new ArgumentNullException(nameof(codec1));
            _codec2 = codec2 ?? throw new ArgumentNullException(nameof(codec2));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _getter1 = getter1 ?? throw new ArgumentNullException(nameof(getter1));
            _getter2 = getter2 ?? throw new ArgumentNullException(nameof(getter2));
        }

        public void Encode(BinaryWriter writer, TResult value)
        {
            _codec1.Encode(writer, _getter1(value));
            _codec2.Encode(writer, _getter2(value));
        }        public TResult Decode(BinaryReader reader)
        {
            T1 val1 = _codec1.Decode(reader);
            T2 val2 = _codec2.Decode(reader);
            return _factory(val1, val2);
        }
          public string SerializeToJson(TResult value)
        {
            var jsonObj = new object[]
            {
                JsonSerializer.Deserialize<object>(_codec1.SerializeToJson(_getter1(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec2.SerializeToJson(_getter2(value))) ?? new object()
            };
            return JsonSerializer.Serialize(jsonObj);
        }
        
        public TResult DeserializeFromJson(string json)
        {
            var jsonArray = JsonSerializer.Deserialize<object[]>(json) ?? 
                throw new InvalidOperationException("Deserialization resulted in a null array.");
                
            if (jsonArray.Length != 2)
                throw new InvalidOperationException($"Expected array of length 2, got {jsonArray.Length}");
                
            var val1 = _codec1.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[0]));
            var val2 = _codec2.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[1]));
            
            return _factory(val1, val2);
        }
    }
    #endregion

    #region TupleCodec3
    public class TupleCodec<T1, T2, T3, TResult> : ICodec<TResult>
    {
        private readonly ICodec<T1> _codec1;
        private readonly ICodec<T2> _codec2;
        private readonly ICodec<T3> _codec3;
        private readonly Func<T1, T2, T3, TResult> _factory;
        private readonly Func<TResult, T1> _getter1;
        private readonly Func<TResult, T2> _getter2;
        private readonly Func<TResult, T3> _getter3;

        public TupleCodec(
            ICodec<T1> codec1, 
            ICodec<T2> codec2,
            ICodec<T3> codec3,
            Func<T1, T2, T3, TResult> factory, 
            Func<TResult, T1> getter1,
            Func<TResult, T2> getter2,
            Func<TResult, T3> getter3)
        {
            _codec1 = codec1 ?? throw new ArgumentNullException(nameof(codec1));
            _codec2 = codec2 ?? throw new ArgumentNullException(nameof(codec2));
            _codec3 = codec3 ?? throw new ArgumentNullException(nameof(codec3));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _getter1 = getter1 ?? throw new ArgumentNullException(nameof(getter1));
            _getter2 = getter2 ?? throw new ArgumentNullException(nameof(getter2));
            _getter3 = getter3 ?? throw new ArgumentNullException(nameof(getter3));
        }

        public void Encode(BinaryWriter writer, TResult value)
        {
            _codec1.Encode(writer, _getter1(value));
            _codec2.Encode(writer, _getter2(value));
            _codec3.Encode(writer, _getter3(value));
        }        public TResult Decode(BinaryReader reader)
        {
            T1 val1 = _codec1.Decode(reader);
            T2 val2 = _codec2.Decode(reader);
            T3 val3 = _codec3.Decode(reader);
            return _factory(val1, val2, val3);
        }
          public string SerializeToJson(TResult value)
        {
            var jsonObj = new object[]
            {
                JsonSerializer.Deserialize<object>(_codec1.SerializeToJson(_getter1(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec2.SerializeToJson(_getter2(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec3.SerializeToJson(_getter3(value))) ?? new object()
            };
            return JsonSerializer.Serialize(jsonObj);
        }
        
        public TResult DeserializeFromJson(string json)
        {
            var jsonArray = JsonSerializer.Deserialize<object[]>(json) ?? 
                throw new InvalidOperationException("Deserialization resulted in a null array.");
                
            if (jsonArray.Length != 3)
                throw new InvalidOperationException($"Expected array of length 3, got {jsonArray.Length}");
                
            var val1 = _codec1.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[0]));
            var val2 = _codec2.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[1]));
            var val3 = _codec3.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[2]));
            
            return _factory(val1, val2, val3);
        }
    }
    #endregion

    #region TupleCodec4
    public class TupleCodec<T1, T2, T3, T4, TResult> : ICodec<TResult>
    {
        private readonly ICodec<T1> _codec1;
        private readonly ICodec<T2> _codec2;
        private readonly ICodec<T3> _codec3;
        private readonly ICodec<T4> _codec4;
        private readonly Func<T1, T2, T3, T4, TResult> _factory;
        private readonly Func<TResult, T1> _getter1;
        private readonly Func<TResult, T2> _getter2;
        private readonly Func<TResult, T3> _getter3;
        private readonly Func<TResult, T4> _getter4;

        public TupleCodec(
            ICodec<T1> codec1, 
            ICodec<T2> codec2,
            ICodec<T3> codec3,
            ICodec<T4> codec4,
            Func<T1, T2, T3, T4, TResult> factory, 
            Func<TResult, T1> getter1,
            Func<TResult, T2> getter2,
            Func<TResult, T3> getter3,
            Func<TResult, T4> getter4)
        {
            _codec1 = codec1 ?? throw new ArgumentNullException(nameof(codec1));
            _codec2 = codec2 ?? throw new ArgumentNullException(nameof(codec2));
            _codec3 = codec3 ?? throw new ArgumentNullException(nameof(codec3));
            _codec4 = codec4 ?? throw new ArgumentNullException(nameof(codec4));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _getter1 = getter1 ?? throw new ArgumentNullException(nameof(getter1));
            _getter2 = getter2 ?? throw new ArgumentNullException(nameof(getter2));
            _getter3 = getter3 ?? throw new ArgumentNullException(nameof(getter3));
            _getter4 = getter4 ?? throw new ArgumentNullException(nameof(getter4));
        }

        public void Encode(BinaryWriter writer, TResult value)
        {
            _codec1.Encode(writer, _getter1(value));
            _codec2.Encode(writer, _getter2(value));
            _codec3.Encode(writer, _getter3(value));
            _codec4.Encode(writer, _getter4(value));
        }

        public TResult Decode(BinaryReader reader)
        {
            T1 val1 = _codec1.Decode(reader);
            T2 val2 = _codec2.Decode(reader);
            T3 val3 = _codec3.Decode(reader);
            T4 val4 = _codec4.Decode(reader);
            return _factory(val1, val2, val3, val4);
        }
          public string SerializeToJson(TResult value)
        {
            var jsonObj = new object[]
            {
                JsonSerializer.Deserialize<object>(_codec1.SerializeToJson(_getter1(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec2.SerializeToJson(_getter2(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec3.SerializeToJson(_getter3(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec4.SerializeToJson(_getter4(value))) ?? new object()
            };
            return JsonSerializer.Serialize(jsonObj);
        }
        
        public TResult DeserializeFromJson(string json)
        {
            var jsonArray = JsonSerializer.Deserialize<object[]>(json) ?? 
                throw new InvalidOperationException("Deserialization resulted in a null array.");
                
            if (jsonArray.Length != 4)
                throw new InvalidOperationException($"Expected array of length 4, got {jsonArray.Length}");
                
            var val1 = _codec1.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[0]));
            var val2 = _codec2.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[1]));
            var val3 = _codec3.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[2]));
            var val4 = _codec4.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[3]));
            
            return _factory(val1, val2, val3, val4);
        }
    }
    #endregion

    #region TupleCodec5
    public class TupleCodec<T1, T2, T3, T4, T5, TResult> : ICodec<TResult>
    {
        private readonly ICodec<T1> _codec1;
        private readonly ICodec<T2> _codec2;
        private readonly ICodec<T3> _codec3;
        private readonly ICodec<T4> _codec4;
        private readonly ICodec<T5> _codec5;
        private readonly Func<T1, T2, T3, T4, T5, TResult> _factory;
        private readonly Func<TResult, T1> _getter1;
        private readonly Func<TResult, T2> _getter2;
        private readonly Func<TResult, T3> _getter3;
        private readonly Func<TResult, T4> _getter4;
        private readonly Func<TResult, T5> _getter5;

        public TupleCodec(
            ICodec<T1> codec1, 
            ICodec<T2> codec2,
            ICodec<T3> codec3,
            ICodec<T4> codec4,
            ICodec<T5> codec5,
            Func<T1, T2, T3, T4, T5, TResult> factory, 
            Func<TResult, T1> getter1,
            Func<TResult, T2> getter2,
            Func<TResult, T3> getter3,
            Func<TResult, T4> getter4,
            Func<TResult, T5> getter5)
        {
            _codec1 = codec1 ?? throw new ArgumentNullException(nameof(codec1));
            _codec2 = codec2 ?? throw new ArgumentNullException(nameof(codec2));
            _codec3 = codec3 ?? throw new ArgumentNullException(nameof(codec3));
            _codec4 = codec4 ?? throw new ArgumentNullException(nameof(codec4));
            _codec5 = codec5 ?? throw new ArgumentNullException(nameof(codec5));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _getter1 = getter1 ?? throw new ArgumentNullException(nameof(getter1));
            _getter2 = getter2 ?? throw new ArgumentNullException(nameof(getter2));
            _getter3 = getter3 ?? throw new ArgumentNullException(nameof(getter3));
            _getter4 = getter4 ?? throw new ArgumentNullException(nameof(getter4));
            _getter5 = getter5 ?? throw new ArgumentNullException(nameof(getter5));
        }

        public void Encode(BinaryWriter writer, TResult value)
        {
            _codec1.Encode(writer, _getter1(value));
            _codec2.Encode(writer, _getter2(value));
            _codec3.Encode(writer, _getter3(value));
            _codec4.Encode(writer, _getter4(value));
            _codec5.Encode(writer, _getter5(value));
        }

        public TResult Decode(BinaryReader reader)
        {
            T1 val1 = _codec1.Decode(reader);
            T2 val2 = _codec2.Decode(reader);
            T3 val3 = _codec3.Decode(reader);
            T4 val4 = _codec4.Decode(reader);
            T5 val5 = _codec5.Decode(reader);
            return _factory(val1, val2, val3, val4, val5);
        }
        
        public string SerializeToJson(TResult value)
        {
            var jsonObj = new object[]
            {
                JsonSerializer.Deserialize<object>(_codec1.SerializeToJson(_getter1(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec2.SerializeToJson(_getter2(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec3.SerializeToJson(_getter3(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec4.SerializeToJson(_getter4(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec5.SerializeToJson(_getter5(value))) ?? new object()
            };
            return JsonSerializer.Serialize(jsonObj);
        }
        
        public TResult DeserializeFromJson(string json)
        {
            var jsonArray = JsonSerializer.Deserialize<object[]>(json) ?? 
                throw new InvalidOperationException("Deserialization resulted in a null array.");
                
            if (jsonArray.Length != 5)
                throw new InvalidOperationException($"Expected array of length 5, got {jsonArray.Length}");
                
            var val1 = _codec1.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[0]));
            var val2 = _codec2.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[1]));
            var val3 = _codec3.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[2]));
            var val4 = _codec4.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[3]));
            var val5 = _codec5.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[4]));
            
            return _factory(val1, val2, val3, val4, val5);
        }
    }
    #endregion

    #region TupleCodec6
    public class TupleCodec<T1, T2, T3, T4, T5, T6, TResult> : ICodec<TResult>
    {
        private readonly ICodec<T1> _codec1;
        private readonly ICodec<T2> _codec2;
        private readonly ICodec<T3> _codec3;
        private readonly ICodec<T4> _codec4;
        private readonly ICodec<T5> _codec5;
        private readonly ICodec<T6> _codec6;
        private readonly Func<T1, T2, T3, T4, T5, T6, TResult> _factory;
        private readonly Func<TResult, T1> _getter1;
        private readonly Func<TResult, T2> _getter2;
        private readonly Func<TResult, T3> _getter3;
        private readonly Func<TResult, T4> _getter4;
        private readonly Func<TResult, T5> _getter5;
        private readonly Func<TResult, T6> _getter6;

        public TupleCodec(
            ICodec<T1> codec1, 
            ICodec<T2> codec2,
            ICodec<T3> codec3,
            ICodec<T4> codec4,
            ICodec<T5> codec5,
            ICodec<T6> codec6,
            Func<T1, T2, T3, T4, T5, T6, TResult> factory, 
            Func<TResult, T1> getter1,
            Func<TResult, T2> getter2,
            Func<TResult, T3> getter3,
            Func<TResult, T4> getter4,
            Func<TResult, T5> getter5,
            Func<TResult, T6> getter6)
        {
            _codec1 = codec1 ?? throw new ArgumentNullException(nameof(codec1));
            _codec2 = codec2 ?? throw new ArgumentNullException(nameof(codec2));
            _codec3 = codec3 ?? throw new ArgumentNullException(nameof(codec3));
            _codec4 = codec4 ?? throw new ArgumentNullException(nameof(codec4));
            _codec5 = codec5 ?? throw new ArgumentNullException(nameof(codec5));
            _codec6 = codec6 ?? throw new ArgumentNullException(nameof(codec6));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _getter1 = getter1 ?? throw new ArgumentNullException(nameof(getter1));
            _getter2 = getter2 ?? throw new ArgumentNullException(nameof(getter2));
            _getter3 = getter3 ?? throw new ArgumentNullException(nameof(getter3));
            _getter4 = getter4 ?? throw new ArgumentNullException(nameof(getter4));
            _getter5 = getter5 ?? throw new ArgumentNullException(nameof(getter5));
            _getter6 = getter6 ?? throw new ArgumentNullException(nameof(getter6));
        }

        public void Encode(BinaryWriter writer, TResult value)
        {
            _codec1.Encode(writer, _getter1(value));
            _codec2.Encode(writer, _getter2(value));
            _codec3.Encode(writer, _getter3(value));
            _codec4.Encode(writer, _getter4(value));
            _codec5.Encode(writer, _getter5(value));
            _codec6.Encode(writer, _getter6(value));
        }

        public TResult Decode(BinaryReader reader)
        {
            T1 val1 = _codec1.Decode(reader);
            T2 val2 = _codec2.Decode(reader);
            T3 val3 = _codec3.Decode(reader);
            T4 val4 = _codec4.Decode(reader);
            T5 val5 = _codec5.Decode(reader);
            T6 val6 = _codec6.Decode(reader);
            return _factory(val1, val2, val3, val4, val5, val6);
        }
        
        public string SerializeToJson(TResult value)
        {
            var jsonObj = new object[]
            {
                JsonSerializer.Deserialize<object>(_codec1.SerializeToJson(_getter1(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec2.SerializeToJson(_getter2(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec3.SerializeToJson(_getter3(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec4.SerializeToJson(_getter4(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec5.SerializeToJson(_getter5(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec6.SerializeToJson(_getter6(value))) ?? new object()
            };
            return JsonSerializer.Serialize(jsonObj);
        }
        
        public TResult DeserializeFromJson(string json)
        {
            var jsonArray = JsonSerializer.Deserialize<object[]>(json) ?? 
                throw new InvalidOperationException("Deserialization resulted in a null array.");
                
            if (jsonArray.Length != 6)
                throw new InvalidOperationException($"Expected array of length 6, got {jsonArray.Length}");
                
            var val1 = _codec1.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[0]));
            var val2 = _codec2.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[1]));
            var val3 = _codec3.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[2]));
            var val4 = _codec4.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[3]));
            var val5 = _codec5.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[4]));
            var val6 = _codec6.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[5]));
            
            return _factory(val1, val2, val3, val4, val5, val6);
        }
    }
    #endregion

    #region TupleCodec7
    public class TupleCodec<T1, T2, T3, T4, T5, T6, T7, TResult> : ICodec<TResult>
    {
        private readonly ICodec<T1> _codec1;
        private readonly ICodec<T2> _codec2;
        private readonly ICodec<T3> _codec3;
        private readonly ICodec<T4> _codec4;
        private readonly ICodec<T5> _codec5;
        private readonly ICodec<T6> _codec6;
        private readonly ICodec<T7> _codec7;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, TResult> _factory;
        private readonly Func<TResult, T1> _getter1;
        private readonly Func<TResult, T2> _getter2;
        private readonly Func<TResult, T3> _getter3;
        private readonly Func<TResult, T4> _getter4;
        private readonly Func<TResult, T5> _getter5;
        private readonly Func<TResult, T6> _getter6;
        private readonly Func<TResult, T7> _getter7;

        public TupleCodec(
            ICodec<T1> codec1, 
            ICodec<T2> codec2,
            ICodec<T3> codec3,
            ICodec<T4> codec4,
            ICodec<T5> codec5,
            ICodec<T6> codec6,
            ICodec<T7> codec7,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> factory, 
            Func<TResult, T1> getter1,
            Func<TResult, T2> getter2,
            Func<TResult, T3> getter3,
            Func<TResult, T4> getter4,
            Func<TResult, T5> getter5,
            Func<TResult, T6> getter6,
            Func<TResult, T7> getter7)
        {
            _codec1 = codec1 ?? throw new ArgumentNullException(nameof(codec1));
            _codec2 = codec2 ?? throw new ArgumentNullException(nameof(codec2));
            _codec3 = codec3 ?? throw new ArgumentNullException(nameof(codec3));
            _codec4 = codec4 ?? throw new ArgumentNullException(nameof(codec4));
            _codec5 = codec5 ?? throw new ArgumentNullException(nameof(codec5));
            _codec6 = codec6 ?? throw new ArgumentNullException(nameof(codec6));
            _codec7 = codec7 ?? throw new ArgumentNullException(nameof(codec7));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _getter1 = getter1 ?? throw new ArgumentNullException(nameof(getter1));
            _getter2 = getter2 ?? throw new ArgumentNullException(nameof(getter2));
            _getter3 = getter3 ?? throw new ArgumentNullException(nameof(getter3));
            _getter4 = getter4 ?? throw new ArgumentNullException(nameof(getter4));
            _getter5 = getter5 ?? throw new ArgumentNullException(nameof(getter5));
            _getter6 = getter6 ?? throw new ArgumentNullException(nameof(getter6));
            _getter7 = getter7 ?? throw new ArgumentNullException(nameof(getter7));
        }

        public void Encode(BinaryWriter writer, TResult value)
        {
            _codec1.Encode(writer, _getter1(value));
            _codec2.Encode(writer, _getter2(value));
            _codec3.Encode(writer, _getter3(value));
            _codec4.Encode(writer, _getter4(value));
            _codec5.Encode(writer, _getter5(value));
            _codec6.Encode(writer, _getter6(value));
            _codec7.Encode(writer, _getter7(value));
        }

        public TResult Decode(BinaryReader reader)
        {
            T1 val1 = _codec1.Decode(reader);
            T2 val2 = _codec2.Decode(reader);
            T3 val3 = _codec3.Decode(reader);
            T4 val4 = _codec4.Decode(reader);
            T5 val5 = _codec5.Decode(reader);
            T6 val6 = _codec6.Decode(reader);
            T7 val7 = _codec7.Decode(reader);
            return _factory(val1, val2, val3, val4, val5, val6, val7);
        }
        
        public string SerializeToJson(TResult value)
        {
            var jsonObj = new object[]
            {
                JsonSerializer.Deserialize<object>(_codec1.SerializeToJson(_getter1(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec2.SerializeToJson(_getter2(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec3.SerializeToJson(_getter3(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec4.SerializeToJson(_getter4(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec5.SerializeToJson(_getter5(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec6.SerializeToJson(_getter6(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec7.SerializeToJson(_getter7(value))) ?? new object()
            };
            return JsonSerializer.Serialize(jsonObj);
        }
        
        public TResult DeserializeFromJson(string json)
        {
            var jsonArray = JsonSerializer.Deserialize<object[]>(json) ?? 
                throw new InvalidOperationException("Deserialization resulted in a null array.");
                
            if (jsonArray.Length != 7)
                throw new InvalidOperationException($"Expected array of length 7, got {jsonArray.Length}");
                
            var val1 = _codec1.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[0]));
            var val2 = _codec2.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[1]));
            var val3 = _codec3.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[2]));
            var val4 = _codec4.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[3]));
            var val5 = _codec5.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[4]));
            var val6 = _codec6.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[5]));
            var val7 = _codec7.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[6]));
            
            return _factory(val1, val2, val3, val4, val5, val6, val7);
        }
    }
    #endregion

    #region TupleCodec8
    public class TupleCodec<T1, T2, T3, T4, T5, T6, T7, T8, TResult> : ICodec<TResult>
    {
        private readonly ICodec<T1> _codec1;
        private readonly ICodec<T2> _codec2;
        private readonly ICodec<T3> _codec3;
        private readonly ICodec<T4> _codec4;
        private readonly ICodec<T5> _codec5;
        private readonly ICodec<T6> _codec6;
        private readonly ICodec<T7> _codec7;
        private readonly ICodec<T8> _codec8;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> _factory;
        private readonly Func<TResult, T1> _getter1;
        private readonly Func<TResult, T2> _getter2;
        private readonly Func<TResult, T3> _getter3;
        private readonly Func<TResult, T4> _getter4;
        private readonly Func<TResult, T5> _getter5;
        private readonly Func<TResult, T6> _getter6;
        private readonly Func<TResult, T7> _getter7;
        private readonly Func<TResult, T8> _getter8;

        public TupleCodec(
            ICodec<T1> codec1, 
            ICodec<T2> codec2,
            ICodec<T3> codec3,
            ICodec<T4> codec4,
            ICodec<T5> codec5,
            ICodec<T6> codec6,
            ICodec<T7> codec7,
            ICodec<T8> codec8,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> factory, 
            Func<TResult, T1> getter1,
            Func<TResult, T2> getter2,
            Func<TResult, T3> getter3,
            Func<TResult, T4> getter4,
            Func<TResult, T5> getter5,
            Func<TResult, T6> getter6,
            Func<TResult, T7> getter7,
            Func<TResult, T8> getter8)
        {
            _codec1 = codec1 ?? throw new ArgumentNullException(nameof(codec1));
            _codec2 = codec2 ?? throw new ArgumentNullException(nameof(codec2));
            _codec3 = codec3 ?? throw new ArgumentNullException(nameof(codec3));
            _codec4 = codec4 ?? throw new ArgumentNullException(nameof(codec4));
            _codec5 = codec5 ?? throw new ArgumentNullException(nameof(codec5));
            _codec6 = codec6 ?? throw new ArgumentNullException(nameof(codec6));
            _codec7 = codec7 ?? throw new ArgumentNullException(nameof(codec7));
            _codec8 = codec8 ?? throw new ArgumentNullException(nameof(codec8));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _getter1 = getter1 ?? throw new ArgumentNullException(nameof(getter1));
            _getter2 = getter2 ?? throw new ArgumentNullException(nameof(getter2));
            _getter3 = getter3 ?? throw new ArgumentNullException(nameof(getter3));
            _getter4 = getter4 ?? throw new ArgumentNullException(nameof(getter4));
            _getter5 = getter5 ?? throw new ArgumentNullException(nameof(getter5));
            _getter6 = getter6 ?? throw new ArgumentNullException(nameof(getter6));
            _getter7 = getter7 ?? throw new ArgumentNullException(nameof(getter7));
            _getter8 = getter8 ?? throw new ArgumentNullException(nameof(getter8));
        }

        public void Encode(BinaryWriter writer, TResult value)
        {
            _codec1.Encode(writer, _getter1(value));
            _codec2.Encode(writer, _getter2(value));
            _codec3.Encode(writer, _getter3(value));
            _codec4.Encode(writer, _getter4(value));
            _codec5.Encode(writer, _getter5(value));
            _codec6.Encode(writer, _getter6(value));
            _codec7.Encode(writer, _getter7(value));
            _codec8.Encode(writer, _getter8(value));
        }

        public TResult Decode(BinaryReader reader)
        {
            T1 val1 = _codec1.Decode(reader);
            T2 val2 = _codec2.Decode(reader);
            T3 val3 = _codec3.Decode(reader);
            T4 val4 = _codec4.Decode(reader);
            T5 val5 = _codec5.Decode(reader);
            T6 val6 = _codec6.Decode(reader);
            T7 val7 = _codec7.Decode(reader);
            T8 val8 = _codec8.Decode(reader);
            return _factory(val1, val2, val3, val4, val5, val6, val7, val8);
        }
        
        public string SerializeToJson(TResult value)
        {
            var jsonObj = new object[]
            {
                JsonSerializer.Deserialize<object>(_codec1.SerializeToJson(_getter1(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec2.SerializeToJson(_getter2(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec3.SerializeToJson(_getter3(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec4.SerializeToJson(_getter4(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec5.SerializeToJson(_getter5(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec6.SerializeToJson(_getter6(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec7.SerializeToJson(_getter7(value))) ?? new object(),
                JsonSerializer.Deserialize<object>(_codec8.SerializeToJson(_getter8(value))) ?? new object()
            };
            return JsonSerializer.Serialize(jsonObj);
        }
        
        public TResult DeserializeFromJson(string json)
        {
            var jsonArray = JsonSerializer.Deserialize<object[]>(json) ?? 
                throw new InvalidOperationException("Deserialization resulted in a null array.");
                
            if (jsonArray.Length != 8)
                throw new InvalidOperationException($"Expected array of length 8, got {jsonArray.Length}");
                
            var val1 = _codec1.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[0]));
            var val2 = _codec2.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[1]));
            var val3 = _codec3.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[2]));
            var val4 = _codec4.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[3]));
            var val5 = _codec5.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[4]));
            var val6 = _codec6.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[5]));
            var val7 = _codec7.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[6]));
            var val8 = _codec8.DeserializeFromJson(JsonSerializer.Serialize(jsonArray[7]));
            
            return _factory(val1, val2, val3, val4, val5, val6, val7, val8);
        }
    }
    #endregion
}

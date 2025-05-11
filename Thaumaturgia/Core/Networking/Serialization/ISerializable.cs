using System;

namespace Thaumaturgia.Core.Networking.Serialization
{
    public interface ISerializable<T>
    {
        ICodec<T> Codec { get; }
    }
}
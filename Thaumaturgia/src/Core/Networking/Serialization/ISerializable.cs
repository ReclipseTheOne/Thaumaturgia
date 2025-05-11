using System;
using Thaumaturgia.Core.Networking.Serialization.Codecs;

namespace Thaumaturgia.Core.Networking.Serialization
{
    public interface ISerializable<T>
    {
        ICodec<T> Codec { get; }
    }
}
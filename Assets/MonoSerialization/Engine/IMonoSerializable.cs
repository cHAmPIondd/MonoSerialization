using System.Collections.Generic;

namespace MonoSerialization
{
    /// <summary>
    /// Used for gameObject serialization. There can only be one 'IMonoSerializable' in this gameObject, and 'IMonoSerializable' can be serialized through 'Serialization' class
    /// Tip: the class implementing the 'IMonoSerializable' interface must be a MonoBehavior
    /// </summary>
    public interface IMonoSerializable
    {
        MonoSerializationInfo SerializationInfo { get; }
        Queue<object> GetSerializableDatas();
        void Deserialize(Queue<object> _serializables);
    }
}
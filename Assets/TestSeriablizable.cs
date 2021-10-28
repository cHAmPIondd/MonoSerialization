using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonoSerialization;

[RequireComponent(typeof(MonoSerializationInfo))]
public class TestSeriablizable : MonoBehaviour, IMonoSerializable
{
    public MonoSerializationInfo SerializationInfo => GetComponent<MonoSerializationInfo>();

    Queue<object> IMonoSerializable.GetSerializableDatas()
    {
        var queue = new Queue<object>();
        queue.Enqueue(new MapObjectBasicData() { position = new Queue<object>(new List<object>() { Vector2.right }) }); ;
        return queue;
    }

    void IMonoSerializable.Deserialize(Queue<object> _serializables)
    {
        var basicData = ((MapObjectBasicData)_serializables.Dequeue()).position;
        Debug.Log(basicData.Dequeue());
    }
}
[System.Serializable]
public struct MapObjectBasicData
{
    public Queue<object> position;
}
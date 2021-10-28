using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Assertions;
using System.Linq;

namespace MonoSerialization
{
    public class Serialization
    {
        private System.Func<string, GameObject> getInstanceFunc;
        public Serialization(System.Func<string, GameObject> _getInstanceFunc)
        {
            Assert.IsNotNull(_getInstanceFunc);
            getInstanceFunc = _getInstanceFunc;
        }
        public string Serialize(IMonoSerializable serializable)
        {
            Assert.IsNotNull(serializable.SerializationInfo);
            Assert.IsTrue(serializable is MonoBehaviour);
            var go = (serializable as MonoBehaviour).gameObject;
            Assert.IsTrue(go.GetComponentsInChildren<IMonoSerializable>().Length == 1, "You need to ensure that only one IMonoSerializable exists on this gameObject");

            var json = new JSONObject();
            json["Prefab"] = serializable.SerializationInfo.PrefabName;
            json["Mono"] = SerializeData(serializable);
            return json.ToString();
        }
        private string SerializeData(IMonoSerializable serializable)
        {
            var serializableDatas = serializable.GetSerializableDatas();
            int count = serializableDatas.Count;
            var jsonArray = new JSONArray();
            for (int i = 0; i < count; i++)
            {
                var serializableData = serializableDatas.Dequeue();
                jsonArray.Add(JsonUtility.ToJson(serializableData));
            }
            return jsonArray.ToString();
        }
        public IMonoSerializable Deserialize(string _jsonStr)
        {
            Assert.IsNotNull(_jsonStr);

            var json = JSONNode.Parse(_jsonStr);
            var prefabJsonStr = json["Prefab"].Value;
            var monoJsonStr = json["Mono"].Value;
            var instance = getInstanceFunc.Invoke(prefabJsonStr);

            Assert.IsNotNull(instance, $"Make sure you use the correct method to create instance,can't create instance with key [{prefabJsonStr}]");

            var serializables = instance.GetComponentsInChildren<IMonoSerializable>();
            Assert.IsTrue(serializables.Length == 1, $"You need to ensure that only one 'IMonoSerializable' exists on this instance: [{instance.name}]");
            var serializable = serializables.FirstOrDefault();
            serializable.Deserialize(DeserializeData(serializable, monoJsonStr));
            return serializable;
        }
        private Queue<object> DeserializeData(IMonoSerializable serializable, string _jsonStr)
        {
            var dataStructures = serializable.GetSerializableDatas();
            var jsonArray = JSONNode.Parse(_jsonStr).AsArray;

            if (jsonArray.Count != dataStructures.Count)
            {
                Debug.LogError("Deserialization error! Maybe the deserialization method has changed,but the data is old.", serializable.SerializationInfo.gameObject);
                return dataStructures;
            }

            var serializableQueue = new Queue<object>();
            int count = dataStructures.Count;
            for (int i = 0; i < count; i++)
            {
                var dataStructure = dataStructures.Dequeue();
                var serializableData = JsonUtility.FromJson(jsonArray[i].Value, dataStructure.GetType());
                serializableQueue.Enqueue(serializableData);
            }
            return serializableQueue;
        }
    }
}
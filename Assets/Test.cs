using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonoSerialization;
using SimpleJSON;

public class Test : MonoBehaviour
{
    Serialization serialization;
    private void Start()
    {
        serialization = new Serialization((x) => { return GameObject.Instantiate(Resources.Load<GameObject>(x)); });
        var go = GameObject.Instantiate(Resources.Load<GameObject>("Test22"));
        var json = serialization.Serialize(go.GetComponent<IMonoSerializable>());
        serialization.Deserialize(json);
    }
    private void Update()
    {
    }
}
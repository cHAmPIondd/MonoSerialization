using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is used to store the GameObject information required for serialization
/// </summary>
public class MonoSerializationInfo : MonoBehaviour
{
    [SerializeField, HideInInspector]
    private string m_prefabName;
    /// <summary>
    /// The name of the prefab connected to this GameObject
    /// </summary>
    public string PrefabName { get { return m_prefabName; } set { m_prefabName = value; } }
}

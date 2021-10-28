using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(MonoSerializationInfo))]
public class MonoSerializationInfoInspector : Editor
{
    private MonoSerializationInfo m_target;
    private MonoSerializationInfo Target
    {
        get
        {
            if (m_target == null)
                m_target = target as MonoSerializationInfo;
            return m_target;
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUI.enabled = false;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Prefab Name:");
        EditorGUILayout.TextField(Target.PrefabName);
        EditorGUILayout.EndHorizontal();
        if (PrefabUtility.IsPartOfPrefabAsset(Target.gameObject))
        {
            Target.PrefabName = Target.gameObject.name;
            EditorUtility.SetDirty(Target);
        }
    }
}

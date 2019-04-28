#if (UNITY_EDITOR) 

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorldManager))]
public class WorldManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WorldManager myScript = (WorldManager) target;
        if (GUILayout.Button("Clear Saves"))
        {
            PersistantData.ClearSaves();
        }
    }
}
#endif
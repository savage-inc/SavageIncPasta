#if (UNITY_EDITOR) 

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyTeamCreator : EditorWindow
{
    public List<EnemyList> _teamList;
    Vector2 scrollPos;

    [MenuItem("Window/Enemy Team Manager")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(EnemyTeamCreator));
    }

    private void OnGUI()
    {
        if(_teamList == null)
        {
            _teamList = new List<EnemyList>();
            load();
        }
        EditorGUILayout.LabelField("Team editor:");

        if (GUILayout.Button("Save to File"))
        {
            save();
        }

        var serializedObject = new SerializedObject(this);
        var property = serializedObject.FindProperty("_teamList");

        EditorGUILayout.BeginHorizontal();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(1000), GUILayout.Height(1000));

        serializedObject.Update();
        EditorGUILayout.PropertyField(property, true);
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();
    }

    void load()
    {
        string path = Application.dataPath + "/Resources/Data/Enemies/";
        if (System.IO.File.Exists(path + "Teams.bytes"))
        {
            var data = PersistantData.ReadBytesFromFile(path, "Teams.bytes");
            _teamList = PersistantData.DeserializeToType<List<EnemyList>>(data);
        }
    }

    void save()
    {
        string path = Application.dataPath + "/Resources/Data/Enemies";
        var data = PersistantData.SerializeToBytes(_teamList);
        PersistantData.SaveBytesToFile(Application.dataPath + "/Resources/Data/Enemies/", "Teams.bytes", data);
    }
}
#endif
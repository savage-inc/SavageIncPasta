using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyCreator : EditorWindow
{
    private static Character _enemy;
    private string _enemyFileName;
    [MenuItem("Window/Enemy Creator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(EnemyCreator));
        _enemy = new Character();
        _enemy.Class = ClassType.eENEMY;
        _enemy.Player = false;
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        GUILayout.Label("Enemy Creator");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Load Enemy File");
        _enemyFileName = EditorGUILayout.TextField(_enemyFileName);
        if (GUILayout.Button("load enemy"))
        {
            LoadEnemy(_enemyFileName);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Enemy Name");
        _enemy.Name = EditorGUILayout.TextField(_enemy.Name);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Enemy Sprite");
        _enemy.SpritePreviewName = EditorGUILayout.TextField(_enemy.SpritePreviewName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Enemy Level");
        _enemy.Level = EditorGUILayout.IntField(_enemy.Level);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Enemy Max Health");
        _enemy.MaxHealth = EditorGUILayout.IntField(_enemy.MaxHealth);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Enemy Strength");
        _enemy.Strength = EditorGUILayout.IntField(_enemy.Strength);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Enemy Constitution");
        _enemy.Constitution = EditorGUILayout.IntField(_enemy.Constitution);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Enemy Dexterity");
        _enemy.Dexterity = EditorGUILayout.IntField(_enemy.Dexterity);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Enemy Intelligence");
        _enemy.Intelligence = EditorGUILayout.IntField(_enemy.Intelligence);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Enemy Charisma");
        _enemy.Charisma = EditorGUILayout.IntField(_enemy.Charisma);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Enemy Magic Type");
        _enemy.Magic = (MagicType)EditorGUILayout.EnumPopup(_enemy.Magic);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        if (GUILayout.Button("Save to File"))
        {
            SaveToResources();
        }
    }

    void SaveToResources()
    {
        if (_enemy.Name == string.Empty)
            return;

        _enemy.CurrentHealth = _enemy.MaxHealth;

        //create GUID if it doesnt already have one
        if (_enemy.ID == System.Guid.Empty)
        {
            _enemy.ID = System.Guid.NewGuid();
        }

        //serlize 
        var data = PersistantData.SerializeToBytes(_enemy);
        PersistantData.SaveBytesToFile(Application.dataPath + "/Resources/Data/Enemies/", _enemy.Name, data);
    }

    void LoadEnemy(string name)
    {
        string path = Application.dataPath + "/Resources/Data/Enemies/";
        if(System.IO.File.Exists(path + name))
        {
            var data = PersistantData.ReadBytesFromFile(path, name);
            _enemy = PersistantData.DeserializeToType<Character>(data);
        }
    }
}

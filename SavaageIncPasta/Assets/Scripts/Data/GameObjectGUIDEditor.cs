using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(GameObjectGUID))]
public class GameObjectGUIDInspector : Editor
{
    private GameObjectGUID id;

    // assign GameObjectGUID instance to this inspector
    void OnEnable()
    {
        id = (GameObjectGUID)target;

        // generate new guid when you create a new game object
        if (id.GameObjectID == 0) id.GameObjectID = new System.Random().Next(1000000, 9999999);

        // generate new guid if guid already exists
        else
        {
            GameObjectGUID[] objects = Array.ConvertAll(GameObject.FindObjectsOfType(typeof(GameObjectGUID)), x => x as GameObjectGUID);
            int idCount = 0;
            for (int i = 0; i < objects.Length; i++)
            {
                if (id.GameObjectID == objects[i].GameObjectID)
                    idCount++;
            }
            if (idCount > 1) id.GameObjectID = new System.Random().Next(1000000, 9999999);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CircleCollider2D))]
public class SceneDoor : MonoBehaviour
{
    public string SceneName;
    public Vector2 PositionInNewScene;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.E))
        {
            var scene = SceneManager.GetSceneByName(SceneName);
            if (!scene.IsValid())
            {
                //Save Data
                var worldManager = FindObjectOfType<WorldManager>();
                if (worldManager != null)
                {
                    worldManager.SaveWorld();
                }

                //Get the player
                PersistantData.SetPlayerPositionInNextScene(PositionInNewScene);
                SceneManager.LoadScene(SceneName);
            }
        }
    }
}

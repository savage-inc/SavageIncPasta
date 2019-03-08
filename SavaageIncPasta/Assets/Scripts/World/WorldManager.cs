using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Vector2 SpawnPos;

    private PartyInventory _partyInventory;
    private GameObject _playerObject;

    void Awake()
    {
        _playerObject = Instantiate(PlayerPrefab, SpawnPos, Quaternion.identity);
        //Set camera to follow the player
        Camera.main.gameObject.GetComponent<CameraFollow>().Target = _playerObject.transform;

        //find the party inventory 
        _partyInventory = FindObjectOfType<PartyInventory>();

        LoadWorld();
        //check wether the player has a custom location to be loaded in the new scene
        if (PersistantData.HasPositionInScene())
        {
            _playerObject.transform.position = PersistantData.GetPlayerPositionInScene();
        }
    }

    public void SaveWorld()
    {
        PersistantData.SaveSceneData(SceneManager.GetActiveScene().name, _playerObject.transform.position);
        PersistantData.SavePartyData(_partyInventory);
    }

    public void LoadWorld()
    {
        //Attempt to load Scene
        PersistantData.LoadSceneData(SceneManager.GetActiveScene().name, _playerObject.transform);
        //load party
        var itemDatabase = FindObjectOfType<ItemDatabase>();
        PersistantData.LoadPartyData(_partyInventory,itemDatabase);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

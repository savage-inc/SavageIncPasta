using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Vector2 SpawnPos;

    private PartyInventory _partyInventory;
    private PlayerManager _playerManager;
    private ClanManager _clanManager;

    private GameObject _playerObject;

    void Awake()
    {
        _playerObject = Instantiate(PlayerPrefab, SpawnPos, Quaternion.identity);
        //Set camera to follow the player
        Camera.main.gameObject.GetComponent<CameraFollow>().Target = _playerObject.transform;

        //find the party inventory 
        _partyInventory = FindObjectOfType<PartyInventory>();

        _playerManager = FindObjectOfType<PlayerManager>();
        _clanManager = FindObjectOfType<ClanManager>();
    }

    void Start()
    {
        LoadWorld();
        //check wether the player has a custom location to be loaded in the new scene
        if (PersistantData.HasPositionInScene())
        {
            _playerObject.transform.position = PersistantData.GetPlayerPositionInScene();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveWorld();
        }
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            LoadWorld();
        }
    }

    public void SaveWorld()
    {
        PersistantData.SaveSceneData(SceneManager.GetActiveScene().name, _playerObject.transform.position,FindObjectsOfType<Shop>());
        PersistantData.SavePartyData(_partyInventory, _playerManager,_clanManager);
    }

    public void LoadWorld()
    {

        //Attempt to load Scene
        PersistantData.LoadSceneData(SceneManager.GetActiveScene().name, _playerObject.transform, FindObjectsOfType<Shop>());
        //load party
        PersistantData.LoadPartyData(_partyInventory,_playerManager, _clanManager);
    }
}

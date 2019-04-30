/* Attach to BarracksMenu */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class BarracksControl : MonoBehaviour
{
    public GameObject FirstSelected;
    public Text PartyGoldText;
        
    public GameObject BarracksMember1;
    public GameObject BarracksMember2;
    public GameObject BarracksMember3;
    public GameObject BarracksMember4;
    public Image[] PartyImages;

    private BarracksManager _barracksManager;
    private PartyInventory _partyInventory;
    private PlayerManager _playerManager;

    // Use this for initialization
    void Start()
    {
        SetCharacterData();
    }

    private void Update()
    {
        SetCharacterData();
    }

    private void Awake()
    {
        _barracksManager = FindObjectOfType<BarracksManager>();
        _partyInventory = FindObjectOfType<PartyInventory>();
        _playerManager = FindObjectOfType<PlayerManager>();
    }

    private void OnEnable()
    {
        if (FirstSelected != null)
        {
            SetCharacterData();
            var eventSystem = FindObjectOfType<EventSystem>();
            eventSystem.SetSelectedGameObject(FirstSelected);
        }
    }

    private void OnDisable()
    {
        PersistantData.SavePartyData(FindObjectOfType<PartyInventory>(), _playerManager, FindObjectOfType<ClanManager>());
    }

    private void SetCharacterData()
    {
        //get party images
        for (int i = 0; i < 4; i++)
        {
            if (i < _playerManager.Characters.Count)
            {
                PartyImages[i].sprite = FindObjectOfType<SpriteManager>().GetSprite(_playerManager.Characters[i].SpritePreviewName);
                PartyImages[i].enabled = true;
            }
            else
            {
                PartyImages[i].enabled = false;
            }
        }

        //get gold
        PartyGoldText.text = "Party Gold = " + _partyInventory.Gold;

        // Generate random character
        BarracksMember1.transform.GetChild(0).GetComponent<Image>().sprite =
            FindObjectOfType<SpriteManager>().GetSprite(_barracksManager.RandomCharacterPool[0].SpritePreviewName);
        BarracksMember1.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Price: " + _barracksManager.Prices[0];

        BarracksMember2.transform.GetChild(0).GetComponent<Image>().sprite =
            FindObjectOfType<SpriteManager>().GetSprite(_barracksManager.RandomCharacterPool[1].SpritePreviewName);
        BarracksMember2.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Price: " + _barracksManager.Prices[1];

        BarracksMember3.transform.GetChild(0).GetComponent<Image>().sprite =
            FindObjectOfType<SpriteManager>().GetSprite(_barracksManager.RandomCharacterPool[2].SpritePreviewName);
        BarracksMember3.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Price: " + _barracksManager.Prices[2];
        
        BarracksMember4.transform.GetChild(0).GetComponent<Image>().sprite =
            FindObjectOfType<SpriteManager>().GetSprite(_barracksManager.RandomCharacterPool[3].SpritePreviewName);
        BarracksMember4.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Price: " + _barracksManager.Prices[3];
    }

    public void DisplayStats(int index)
    {
        FindObjectOfType<CharacterComparison>().character = _barracksManager.RandomCharacterPool[index];
    }
}
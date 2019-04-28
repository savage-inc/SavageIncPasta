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

    private BarracksManager _barracksManager;
    private PartyInventory _partyInventory;

    // Use this for initialization
    void Start()
    {
        _barracksManager = FindObjectOfType<BarracksManager>();
    }

    private void Update()
    {
        SetCharacterData();
    }

    private void Awake()
    {
        _partyInventory = FindObjectOfType<PartyInventory>();
    }

    private void OnEnable()
    {
        if (FirstSelected != null)
        {
            var eventSystem = FindObjectOfType<EventSystem>();
            eventSystem.SetSelectedGameObject(FirstSelected);
        }
    }

    private void SetCharacterData()
    {
        //get gold
        PartyGoldText.text = "Party Gold = " + _partyInventory.Gold;

        // Generate random character
        BarracksMember1.transform.GetChild(0).GetComponent<Image>().sprite =
            FindObjectOfType<SpriteManager>().GetSprite(_barracksManager.RandomCharacterPool[0].SpritePreviewName);
        BarracksMember1.transform.GetChild(1).GetComponent<Text>().text = "Price: " + _barracksManager.Prices[0];

        BarracksMember2.transform.GetChild(0).GetComponent<Image>().sprite =
            FindObjectOfType<SpriteManager>().GetSprite(_barracksManager.RandomCharacterPool[1].SpritePreviewName);
        BarracksMember2.transform.GetChild(1).GetComponent<Text>().text = "Price: " + _barracksManager.Prices[1];

        BarracksMember3.transform.GetChild(0).GetComponent<Image>().sprite =
            FindObjectOfType<SpriteManager>().GetSprite(_barracksManager.RandomCharacterPool[2].SpritePreviewName);
        BarracksMember3.transform.GetChild(1).GetComponent<Text>().text = "Price: " + _barracksManager.Prices[2];
        
        BarracksMember4.transform.GetChild(0).GetComponent<Image>().sprite =
            FindObjectOfType<SpriteManager>().GetSprite(_barracksManager.RandomCharacterPool[3].SpritePreviewName);
        BarracksMember4.transform.GetChild(1).GetComponent<Text>().text = "Price: " + _barracksManager.Prices[3];
    }

    public void DisplayStats(int index)
    {
        FindObjectOfType<CharacterComparison>().character = _barracksManager.RandomCharacterPool[index];
    }
}
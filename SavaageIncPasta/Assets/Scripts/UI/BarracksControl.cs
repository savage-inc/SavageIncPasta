using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class BarracksControl : MonoBehaviour
{
    public CharacterComparison CharacterCompare;
    public GameObject FirstSelected;
    public Text PartyGoldText;
    public RectTransform PartyInventoryContent;
    public GameObject BarracksContent;

    private List<SelectCharacter> Characters;
    private BarracksManager _barracksManager;
    private PartyInventory _partyInventory;


    [SerializeField]
    private GameObject buttonTemplate;
    [SerializeField]
    private GridLayoutGroup gridGroup;

    // Use this for initialization
    void Start()
    {
        GenBarracks();
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

    void GenBarracks()
    {        
        //If number of clan members less than 5, then constraint count of clanMember
        if (_barracksManager.RandomCharacterPool.Count < 5)
        {
            gridGroup.constraintCount = _barracksManager.RandomCharacterPool.Count;
        }
        else
        {
            // Set column to 4
            gridGroup.constraintCount = 4;
        }

        foreach (Character newCharacter in _barracksManager.RandomCharacterPool)
        {
            GameObject newButton = Instantiate(buttonTemplate) as GameObject;
            newButton.SetActive(true);

            BuyButton buyButton = newButton.AddComponent<BuyButton>();
            buyButton.Character = newCharacter; // Add new character
            buyButton.CharacterCompare = CharacterCompare; // Add character compare
            newButton.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<SpriteManager>().GetSprite(newCharacter.SpritePreviewName);

            // Add set to compare mode to newbutton when on click
            newButton.GetComponent<Button>().onClick.AddListener(buyButton.Buy);

            newButton.transform.SetParent(gridGroup.transform, false);
        }
    }


    // We need to randomise characters to be able to be bought
    // Need them to be compared to current party
    // Need an if statement that if party is full go to clan
    // We need to be able to display the following in the UI the character, the price, 
    // the party gold 

    void UpdatePartyItemUI(InventoryItem inventoryItem, int amount)
    {
        foreach (Transform child in PartyInventoryContent.transform)
        {
            //get child with the item
            ShopItemButton shopItemButton = child.GetComponent<ShopItemButton>();
            if (shopItemButton.Item == inventoryItem)
            {
                //get item panel
                child.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "x" + (shopItemButton.Item.Amount + amount);
            }
        }
        PartyGoldText.text = "Party Gold " + _partyInventory.Gold;
    }

    void UpdateBarracksUI(InventoryItem inventoryItem, int amount)
    {
        foreach (Transform child in BarracksContent.transform)
        {
            //get child with the item
            ShopItemButton shopItemButton = child.GetComponent<ShopItemButton>();
            if (shopItemButton.Item == inventoryItem)
            {
                //get item panel
                child.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "x" + (shopItemButton.Item.Amount + amount);
            }
        }
        PartyGoldText.text = "Party Gold " + _partyInventory.Gold;
    }

    public struct SelectCharacter
    {
        public Sprite characterSprite;
    }

}

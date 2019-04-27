/* Attach to BarracksMenu */
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
    public RectTransform BarracksContent;

    public GameObject BarracksMember1;
    public GameObject BarracksMember2;
    public GameObject BarracksMember3;
    public GameObject BarracksMember4;

    private BarracksManager _barracksManager;
    private PartyInventory _partyInventory;

    [SerializeField]
    private GameObject buttonTemplate;
    [SerializeField]
    private GridLayoutGroup gridGroup;
    [SerializeField]
    private Sprite[] characterSprite; // Character Sprite array
    // Use this for initialization
    void Start()
    {
        _barracksManager = FindObjectOfType<BarracksManager>();
        BarracksMember();
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

    private void BarracksMember()
    {
        // Generate random character
        BarracksMember1.transform.GetChild(0).GetComponent<Image>().sprite =
            FindObjectOfType<SpriteManager>().GetSprite(_barracksManager.RandomCharacterPool[0].SpritePreviewName);
        BarracksMember2.transform.GetChild(0).GetComponent<Image>().sprite =
            FindObjectOfType<SpriteManager>().GetSprite(_barracksManager.RandomCharacterPool[1].SpritePreviewName);
        BarracksMember3.transform.GetChild(0).GetComponent<Image>().sprite =
            FindObjectOfType<SpriteManager>().GetSprite(_barracksManager.RandomCharacterPool[2].SpritePreviewName);
        BarracksMember4.transform.GetChild(0).GetComponent<Image>().sprite =
            FindObjectOfType<SpriteManager>().GetSprite(_barracksManager.RandomCharacterPool[3].SpritePreviewName);
    }

    /* We need to randomise characters to be able to be bought
     * We need to be able to display the following in the UI the character, the price, 
       the party gold */

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
}

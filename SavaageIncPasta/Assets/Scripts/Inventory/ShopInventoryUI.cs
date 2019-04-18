using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopInventoryUI : MonoBehaviour
{
    public Shop Shop;
    //Shop Shop UI 
    public GameObject ItemPanelObject;
    public GameObject ShopInventoryObject;
    public RectTransform ShopInventoryContent;
    public GameObject PartyInventoryObject;
    public RectTransform PartyInventoryContent;
    public Text PartyGoldText;
    public GameObject firstItem;


    private PartyInventory _partyInventory;

    // Use this for initialization
    void Awake ()
    {
        _partyInventory = FindObjectOfType<PartyInventory>();
    }

    void Update()
    {
        if (Input.GetButton("B") || Input.GetKeyDown(KeyCode.E))
        {
            gameObject.SetActive(false);
        }
        else if (Input.GetButton("LB"))
        {
            ShowShop();
        }
        else if (Input.GetButton("RB"))
        {
            ShowParty();
        }
    }

    void OnDisable()
    {
        Time.timeScale = 1.0f;

        _partyInventory.Inventory.OnItemAdd -= AddPartyUIItem;
        _partyInventory.Inventory.OnItemRemove -= RemovePartyItemUI;
        _partyInventory.Inventory.OnItemUpdate -= UpdatePartyItemUI;

        Shop.Inventory.OnItemAdd -= AddShopUIItem;
        Shop.Inventory.OnItemRemove -= RemoveShoptemUI;
        Shop.Inventory.OnItemUpdate -= UpdateShopItemUI;

        ClearShop();
        ClearParty();
    }

    void OnEnable()
    {
        Time.timeScale = 0f;

        _partyInventory.Inventory.OnItemAdd += AddPartyUIItem;
        _partyInventory.Inventory.OnItemRemove += RemovePartyItemUI;
        _partyInventory.Inventory.OnItemUpdate += UpdatePartyItemUI;

        Shop.Inventory.OnItemAdd += AddShopUIItem;
        Shop.Inventory.OnItemRemove += RemoveShoptemUI;
        Shop.Inventory.OnItemUpdate += UpdateShopItemUI;

        ShowShop();
    }

    void Start()
    {
        FindObjectOfType<EventSystem>().SetSelectedGameObject(firstItem);
    }

    public void ShowShop()
    {
        ClearShop();
        PartyInventoryObject.SetActive(false);
        ShopInventoryObject.SetActive(true);
        SyncShop();

        if(firstItem == null)
        {
            firstItem = ShopInventoryContent.GetChild(0).gameObject;
        }
        FindObjectOfType<EventSystem>().SetSelectedGameObject(firstItem);
    }

    public void ShowParty()
    {
        ClearParty();
        ShopInventoryObject.SetActive(false);
        PartyInventoryObject.SetActive(true);
        SyncParty();
        FindObjectOfType<EventSystem>().SetSelectedGameObject(firstItem);

    }

    void SyncShop()
    {
        //Get shop Inventory
        foreach (var item in Shop.Inventory.GetItems())
        {
            AddShopUIItem(item);
        }

        PartyGoldText.text = "Party Gold " + _partyInventory.Gold;
    }

    void SyncParty()
    {
        //Get party Inventory
        foreach (var item in _partyInventory.Inventory.GetItems())
        {
            AddPartyUIItem(item);
        }
    }

    void ClearShop()
    {
        firstItem = null;
        //Clear all shop inventory
        foreach (Transform child in ShopInventoryContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void ClearParty()
    {
        firstItem = null;

        //Clear all party inventory items
        foreach (Transform child in PartyInventoryContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void AddPartyUIItem(InventoryItem item)
    {
        var itemPanel = Instantiate(ItemPanelObject);
        itemPanel.transform.SetParent(PartyInventoryContent.transform);

        itemPanel.transform.GetChild(0).GetComponent<Image>().sprite = item.Item.PreviewSprite;
        itemPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "x" + item.Amount;
        itemPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = item.Item.Name;
        itemPanel.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "Value = " + item.Item.BaseMoneyValue / 2;

        var itemButton = itemPanel.GetComponent<ShopItemButton>();
        itemButton.Shop = Shop;
        itemButton.Item = item;

        itemPanel.GetComponent<Button>().onClick.AddListener(() => itemButton.SellItem());

        PartyGoldText.text = "Party Gold " + _partyInventory.Gold;

        if (firstItem == null)
        {
            firstItem = itemButton.gameObject;
        }
    }

    void AddShopUIItem(InventoryItem item)
    {
        var itemPanel = Instantiate(ItemPanelObject);
        itemPanel.transform.SetParent(ShopInventoryContent.transform);

        itemPanel.transform.GetChild(0).GetComponent<Image>().sprite = item.Item.PreviewSprite;
        itemPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "x" + item.Amount;
        itemPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = item.Item.Name;
        itemPanel.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "Value = " + item.Item.BaseMoneyValue;


        var itemButton = itemPanel.GetComponent<ShopItemButton>();
        itemButton.Shop = Shop;
        itemButton.Item = item;

        itemPanel.GetComponent<Button>().onClick.AddListener(() => itemButton.BuyItem());

        PartyGoldText.text = "Party Gold " + _partyInventory.Gold;

        if (firstItem == null)
        {
            firstItem = itemButton.gameObject;
        }
    }

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

    void UpdateShopItemUI(InventoryItem inventoryItem, int amount)
    {
        foreach (Transform child in ShopInventoryContent.transform)
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

    void RemovePartyItemUI(InventoryItem item)
    {
        foreach (Transform child in PartyInventoryContent)
        {
            ShopItemButton shopItemButton = child.GetComponent<ShopItemButton>();
            if (item == shopItemButton.Item)
            {
                Destroy(child.gameObject);
                FindObjectOfType<EventSystem>().SetSelectedGameObject(PartyInventoryContent.transform.GetChild(0).gameObject);
            }
        }
        PartyGoldText.text = "Party Gold " + _partyInventory.Gold;
    }

    void RemoveShoptemUI(InventoryItem item)
    {
        foreach (Transform child in ShopInventoryContent)
        {
            ShopItemButton shopItemButton = child.GetComponent<ShopItemButton>();
            if (item == shopItemButton.Item)
            {
                Destroy(child.gameObject);
                FindObjectOfType<EventSystem>().SetSelectedGameObject(ShopInventoryContent.transform.GetChild(0).gameObject);

            }
        }
        PartyGoldText.text = "Party Gold " + _partyInventory.Gold;
    }
}

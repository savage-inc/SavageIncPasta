using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyInventoryUI : MonoBehaviour
{
    //Party PartyInventory UI 
    public GameObject ItemPanelObject;
    public RectTransform InventoryContent;
    public Text ItemCountText;

    private Inventory _inventory;
    private CharacterEquipment _characterInventory;

    // Use this for initialization
    void Awake ()
    {
        var partyInventory = FindObjectOfType<PartyInventory>();
        _inventory = partyInventory.Inventory;
        _inventory.OnItemAdd += AddUIItem;
        _inventory.OnItemRemove += RemoveItemUI;
        _inventory.OnItemUpdate += UpdateItemUI;

        _characterInventory = FindObjectOfType<CharacterInventory>().CurrentCharacterEquipment;
    }

    void OnDisable()
    {
        //Clear all inventory items
        foreach (Transform child in InventoryContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void OnEnable()
    {
        SyncInventory();
    }

    void SyncInventory()
    {
        foreach (var item in _inventory.GetItems())
        {
            AddUIItem(item);
        }
    }

    void AddUIItem(InventoryItem item)
    {
        var itemPanel = Instantiate(ItemPanelObject);
        itemPanel.transform.SetParent(InventoryContent.transform);

        itemPanel.transform.GetChild(0).GetComponent<Image>().sprite = item.Item.PreviewSprite;
        itemPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "x" + item.Amount;
        itemPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = item.Item.Name;
        
        var itemButton = itemPanel.GetComponent<InventoryItemButton>();
        itemButton.Inventory = _inventory;
        itemButton.Item = item;

        //On button click
        switch (item.Item.ItemType)
        {
            case ItemType.eCONSUMABLE:
                itemPanel.GetComponent<Button>().onClick.AddListener(() => itemButton.ConsumeItem());
                break;
            case ItemType.eARMOUR:
                itemPanel.GetComponent<Button>().onClick.AddListener(() => itemButton.EquipItem(_characterInventory));
                break;
            case ItemType.eWEAPON:
                itemPanel.GetComponent<Button>().onClick.AddListener(() => itemButton.EquipItem(_characterInventory));
                break;
        }

        ItemCountText.text = _inventory.GetItems().Count + "/" + _inventory.InventoryCapacity;
    }

    void RemoveItemUI(InventoryItem item)
    {
        foreach (Transform child in InventoryContent)
        {
            InventoryItemButton inventoryItemButton = child.GetComponent<InventoryItemButton>();
            if (item == inventoryItemButton.Item)
            {
                Destroy(child.gameObject);
            }
        }
        ItemCountText.text = _inventory.GetItems().Count-1 + "/" + _inventory.InventoryCapacity;

    }

    void UpdateItemUI(InventoryItem inventoryItem, int amount)
    {
        foreach (Transform child in InventoryContent.transform)
        {
            //get child with the item
            InventoryItemButton inventoryItemButton = child.GetComponent<InventoryItemButton>();
            if(inventoryItemButton.Item == inventoryItem)
            {
                //get item panel
                child.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "x" + (inventoryItemButton.Item.Amount + amount);
            }
        }
    }
}

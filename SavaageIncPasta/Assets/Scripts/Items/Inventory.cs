using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public BaseItem Item;
    public int Amount;
    public int ViewIndex;
}

public class Inventory : MonoBehaviour
{
    public float Money = 0.0f;
    public int InventorySize = 10;
    public List<InventoryItem> InventoryItems;


    private ItemDatabase _itemDatabase;

    void Awake()
    {
        InventoryItems = new List<InventoryItem>();
        _itemDatabase = FindObjectOfType<ItemDatabase>();
    }

    void Start()
    {
        AddItem("Health Potion");
        AddItem("Health Potion");
        DropItem(InventoryItems[0], Vector2.zero);
    }

    //Add a new item to the inventory from the item database
    public void AddItem(string itemName)
    {
        BaseItemData itemData = _itemDatabase.GetItem(itemName);

        if (itemData == null)
        {
            Debug.LogWarning("Can't add item: " + itemName + " to inventory as it doesn't exist in the item database");
        }

        BaseItem baseItem = null;
        switch (itemData.ItemType)
        {
            case ItemType.eCONSUMABLE:
                baseItem = new ConsumableItem((ConsumableItemData)itemData);
                break;
            case ItemType.eARMOUR:
                baseItem = new ArmourItem((ArmourItemData)itemData);
                break;
            case ItemType.eWEAPON:
                baseItem = new WeaponItem((WeaponItemData)itemData);
                break;
            default:
                break;
        }  

        AddItem(baseItem);
    }

    //Add an already existing item to the inventory
    public void AddItem(BaseItem item)
    {
        //check if the item already exists
        foreach (var inventoryItem in InventoryItems)
        {
            if (inventoryItem.Item.ItemData.Name == item.ItemData.Name)
            {
                if (inventoryItem.Amount < item.ItemData.StackSize)
                {
                    //Item already exists so increment amount and return
                    inventoryItem.Amount++;
                    return;
                }
            }   
        }


        InventoryItem newInventoryItem = new InventoryItem();
        newInventoryItem.Item = item;
        newInventoryItem.Amount = 1;
        newInventoryItem.ViewIndex = InventoryItems.Count;

        InventoryItems.Add(newInventoryItem);
    }

    public void RemoveItem(string itemName)
    {
        InventoryItem itemToRemove = null;

        //check if the item exists
        foreach (var inventoryItem in InventoryItems)
        {
            if (inventoryItem.Item.ItemData.Name == itemName)
            {
                if (inventoryItem.Amount > 1)
                {
                    //Item already exists so increment amount and return
                    inventoryItem.Amount--;
                    return;
                }
                else
                {
                    itemToRemove = inventoryItem;
                    break;
                }
            }
        }

        //Remove tagged item
        if (itemToRemove != null)
        {
            InventoryItems.Remove(itemToRemove);
        }
    }

    public void RemoveItem(InventoryItem item)
    {
        if (item.Amount <= 1)
        {
            InventoryItems.Remove(item);
        }
        else
        {
            //check if the item exists
            foreach (var invItem in InventoryItems)
            {
                if (invItem == item)
                {
                    if (invItem.Amount > 1)
                    {
                        //Item already exists so increment amount and return
                        invItem.Amount--;
                        return;
                    }
                }
            }
        }
    }

    public void DropItem(InventoryItem item, Vector2 position)
    {
        if(item == null)
            return;

        createDroppedItem(item.Item, position);

        if (item.Amount <= 1)
        {
            InventoryItems.Remove(item);
        }
        else
        {
            //check if the item exists
            foreach (var invItem in InventoryItems)
            {
                if (invItem == item)
                {
                    if (invItem.Amount > 1)
                    {
                        //Item already exists so increment amount and return
                        invItem.Amount--;
                        break;
                    }
                }
            }
        }
    }

    private void createDroppedItem(BaseItem item, Vector2 position)
    {
        GameObject itemObject = new GameObject("DroppedItem");
        itemObject.transform.position = position;
        DroppedItem droppedItem = itemObject.AddComponent<DroppedItem>();
        droppedItem.Item = item;
    }
}
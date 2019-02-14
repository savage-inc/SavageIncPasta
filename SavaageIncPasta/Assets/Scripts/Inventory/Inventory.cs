using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public BaseItemData Item;
    public int Amount;
    public int ViewIndex;
}

public class Inventory
{
    public Inventory(int capacity)
    {
        _inventorySize = capacity;
        _inventoryItems = new List<InventoryItem>(_inventorySize);
    }

    public delegate void AddItemAction(BaseItemData item);
    public event AddItemAction OnItemAdd;

    public delegate void RemoveItemAction(BaseItemData item, int amount);
    public event RemoveItemAction OnItemRemove;

    public delegate void UpdateItemAction(BaseItemData item,int viewIndex, int amount);
    public event UpdateItemAction OnItemUpdate;

    private int _inventorySize = 10;
    private List<InventoryItem> _inventoryItems;

    //Add an already existing item to the inventory
    public void AddItem(BaseItemData item)
    {
        //check if the item already exists
        foreach (var inventoryItem in _inventoryItems)
        {
            if (inventoryItem.Item.Name == item.Name)
            {
                if (inventoryItem.Amount < item.StackSize)
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
        newInventoryItem.ViewIndex = _inventoryItems.Count;

        _inventoryItems.Add(newInventoryItem);

        //call add event
        if (OnItemAdd != null)
        {
            OnItemAdd(item);
        }
    }

    public void RemoveItem(string itemName)
    {
        InventoryItem itemToRemove = null;

        //check if the item exists
        foreach (var inventoryItem in _inventoryItems)
        {
            if (inventoryItem.Item.Name == itemName)
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
            _inventoryItems.Remove(itemToRemove);
        }
    }

    public void RemoveItem(InventoryItem item)
    {
        if (item.Amount <= 1)
        {
            _inventoryItems.Remove(item);
            if (OnItemRemove != null)
            {
                OnItemRemove(item.Item, 0);
            }
        }
        else
        {
            //check if the item exists
            foreach (var invItem in _inventoryItems)
            {
                if (invItem == item)
                {
                    if (invItem.Amount > 1)
                    {
                        //Item already exists so increment amount and return
                        invItem.Amount--;
                        if (OnItemRemove != null)
                        {
                            OnItemRemove(item.Item, 1);
                        }
                        return;
                    }
                }
            }
        }
    }
}
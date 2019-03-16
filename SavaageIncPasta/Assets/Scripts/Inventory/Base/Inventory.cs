using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Analytics;

[System.Serializable]
public class InventoryItem
{
    public BaseItemData Item;
    public int Amount;
}

[System.Serializable]
public class Inventory : ISerializable
{
    protected Inventory(SerializationInfo info, StreamingContext context)
    {
        _inventoryCapacity = info.GetInt32("capacity");
        _inventoryItems = new List<InventoryItem>();
        int itemCount = info.GetInt32("itemCount");
        for (int i = 0; i < itemCount; i++)
        {
            int amount = info.GetInt32("itemAmount" + i);
            BaseItemData item = ItemDatabase.GetItemInstance(info.GetString("itemName" + i));

            InventoryItem inventoryItem = new InventoryItem();
            inventoryItem.Item = item;
            inventoryItem.Amount = amount;
            _inventoryItems.Add(inventoryItem);
        }
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("capacity", _inventoryCapacity);
        info.AddValue("itemCount", _inventoryItems.Count);
        for (int i = 0; i < _inventoryItems.Count; i++)
        {
            info.AddValue("itemAmount" + i, _inventoryItems[i].Amount);
            info.AddValue("itemName" + i, _inventoryItems[i].Item.Name);
        }
    }

    public Inventory(int capacity, bool unlimtedStackSize = false)
    {
        _inventoryCapacity = capacity;
        _inventoryItems = new List<InventoryItem>(InventoryCapacity);
        UnlimitedStackSize = unlimtedStackSize;
    }


    public delegate void AddItemAction(InventoryItem item);
    public event AddItemAction OnItemAdd;

    public delegate void RemoveItemAction(InventoryItem item);
    public event RemoveItemAction OnItemRemove;

    public delegate void UpdateItemAction(InventoryItem item, int amount);
    public event UpdateItemAction OnItemUpdate;

    private readonly int _inventoryCapacity = 10;
    protected List<InventoryItem> _inventoryItems;

    public readonly bool UnlimitedStackSize;

    public int InventoryCapacity
    {
        get { return _inventoryCapacity; }
    }

    public void AddItem(string itemName)
    {
        var item = ItemDatabase.GetItemInstance(itemName);
        if (item != null)
        {
            AddItem(item);
        }
    }

    //Add an already existing item to the inventory
    public void AddItem(BaseItemData item)
    {
        //check if the item already exists
        foreach (var inventoryItem in _inventoryItems)
        {
            if (inventoryItem.Item.Name == item.Name)
            {
                if (inventoryItem.Amount < item.StackSize || UnlimitedStackSize)
                {
                    if(OnItemUpdate != null)
                    {
                        OnItemUpdate(inventoryItem, 1);
                    }
                    //Item already exists so increment amount and return
                    inventoryItem.Amount++;
                    return;
                }
            }   
        }

        //TODO change inventory to store a string as the item instead of the item itself
        InventoryItem newInventoryItem = new InventoryItem();
        newInventoryItem.Item = item;
        newInventoryItem.Amount = 1;

        _inventoryItems.Add(newInventoryItem);

        //call add event
        if (OnItemAdd != null)
        {
            OnItemAdd(newInventoryItem);
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
                itemToRemove = inventoryItem;
                break;
            }
        }

        if (itemToRemove != null)
        {
            RemoveItem(itemToRemove);
        }
    }

    public void RemoveItem(InventoryItem item)
    {
        //if there is only one item then remove it
        if (item.Amount <= 1)
        {
            if (OnItemRemove != null)
            {
                OnItemRemove(item);
            }

            _inventoryItems.Remove(item);
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
                        //TODO UPDATE item
                        if (OnItemUpdate != null)
                        {
                            OnItemUpdate(invItem, -1);
                        }
                        //Item already exists so increment amount and return
                        invItem.Amount--;
                        return;
                    }
                }
            }
        }
    }

    public List<InventoryItem> GetItems()
    {
        return _inventoryItems;
    }

    public void Clear()
    {
        if (_inventoryItems != null)
        {
            _inventoryItems.Clear();
        }
    }
}
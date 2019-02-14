using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyInventory : MonoBehaviour
{
    public int InventorySize = 10;
    public Inventory Inventory;

    private ItemDatabase _itemDatabase;

    void Awake()
    {
        Inventory = new Inventory(10);
        _itemDatabase = FindObjectOfType<ItemDatabase>();

        if (!_itemDatabase)
        {
            Debug.LogError("Failed to find item database! (Make sure there is an item database script in the scene)");
        }
    }

    void Start()
    {
        Inventory.AddItem(_itemDatabase.GetItemInstance("Health Potion"));
        Inventory.AddItem(_itemDatabase.GetItemInstance("Sword"));
        Inventory.AddItem(_itemDatabase.GetItemInstance("Magical Sword"));

    }
}

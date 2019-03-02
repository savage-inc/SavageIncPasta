using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyInventory : MonoBehaviour
{
    public int InventorySize = 10;
    public Inventory Inventory;
    [SerializeField]
    private int _gold;
    public List<BaseItemData> StartingItems;

    private ItemDatabase _itemDatabase;

    public int Gold
    {
        get { return _gold; }
        set { _gold = value; }
    }

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
        foreach (var item in StartingItems)
        {
            Inventory.AddItem(item);
        }
    }
}

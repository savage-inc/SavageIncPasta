using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    private Dictionary<string, BaseItemData> _items;

	// Use this for initialization
	void Awake ()
    {
		_items = new Dictionary<string, BaseItemData>();
        LoadItemsFromResources();
        GenerateItems();
    }

    public BaseItemData GetItemInstance(string databaseName)
    {
        if (!_items.ContainsKey(databaseName))
        {
            Debug.LogError("Can't add item: " + databaseName + " to inventory as it doesn't exist in the item database");
        }

        var itemData = _items[databaseName];

        return itemData;
    }

    private void LoadItemsFromResources()
    {
        var items = new List<BaseItemData>(Resources.LoadAll<BaseItemData>("Data/Items/Consumables"));
        items.AddRange(Resources.LoadAll<BaseItemData>("Data/Items/Weapons"));
        items.AddRange(Resources.LoadAll<BaseItemData>("Data/Items/Armour"));

        foreach (var item in items)
        {
            if (item.Name.Length == 0)
            {
                Debug.LogError("Item doesn't have a databaseName and can't be added to item database");
            }

            _items.Add(item.DatabaseName, item);
        }
    }

    void GenerateItems()
    {
        for (int i = 0; i < 500; i++)
        {
            var weapon = RandomItemGenerator.RandomWeapon();
            _items.Add(weapon.DatabaseName, weapon);
        }

        for (int i = 0; i < 250; i++)
        {
            var armour = RandomItemGenerator.RandomArmour();
            _items.Add(armour.DatabaseName, armour);
        }
    }
}


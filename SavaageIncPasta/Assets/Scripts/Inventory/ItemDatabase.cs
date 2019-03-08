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
    }

    public BaseItemData GetItemInstance(string itemName)
    {
        if (!_items.ContainsKey(itemName))
        {
            Debug.LogError("Can't add item: " + itemName + " to inventory as it doesn't exist in the item database");
        }

        var itemData = _items[itemName];

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
                Debug.LogError("Item doesn't have a name and can't be added to item database");
            }

            _items.Add(item.Name, item);
        }
    }
}


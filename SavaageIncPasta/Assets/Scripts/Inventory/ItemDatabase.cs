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
    }

    public BaseItemData GetItemInstance(string name)
    {
        if (!_items.ContainsKey(name))
        {
            Debug.LogError("Can't add item: " + name + " to inventory as it doesn't exist in the item database");
        }

        var itemData = _items[name];

        return itemData;
    }
}

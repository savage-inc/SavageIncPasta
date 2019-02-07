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

    private void LoadItemsFromResources()
    {
        var items = Resources.LoadAll<BaseItemData>("Items/Consumables");

        foreach (var item in items)
        {
            if (item.Name.Length == 0)
            {
                Debug.LogError("Item doesn't have a name and can't be added to item database");
            }

            _items.Add(item.Name,item);
        }
    }

    public BaseItemData GetItem(string name)
    {
        return _items[name];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance { get; private set; }

    private Dictionary<string, BaseItemData> _items;
    public List<WeaponItemData> Weapons { get; private set; }
    public List<ArmourItemData> Armour { get; private set; }


    // Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreateItems()
    {
        _items = new Dictionary<string, BaseItemData>();
        Weapons = new List<WeaponItemData>();
        Armour = new List<ArmourItemData>();

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
            Weapons.Add(weapon);
        }

        for (int i = 0; i < 250; i++)
        {
            var armour = RandomItemGenerator.RandomArmour();
            _items.Add(armour.DatabaseName, armour);
            Armour.Add(armour);
        }
    }

    public List<BaseItemData> ToList()
    {
        List<BaseItemData> items = new List<BaseItemData>();

        foreach (var item in _items)
        {
            items.Add(item.Value);
        }

        return items;
    }

    public void FromList(List<BaseItemData> items)
    {
        _items = new Dictionary<string, BaseItemData>();
        Weapons = new List<WeaponItemData>();
        Armour = new List<ArmourItemData>();

        foreach (var item in items)
        {
            _items.Add(item.DatabaseName, item);
            if(item.ItemType == ItemType.eARMOUR)
            {
                Armour.Add((ArmourItemData)item);
            }
            else if (item.ItemType == ItemType.eWEAPON)
            {
                Weapons.Add((WeaponItemData)item);
            }
        }
    }

    public bool HasItems()
    {
        return _items != null && _items.Count > 0;
    }
}


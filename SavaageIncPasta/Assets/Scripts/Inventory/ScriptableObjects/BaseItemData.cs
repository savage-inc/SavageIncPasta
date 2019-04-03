using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    eCONSUMABLE,
    eARMOUR,
    eWEAPON,
}

public enum ItemRarity
{
    eCOMMON,
    eUNCOMMON,
    eRARE,
    eEPIC,
    eLEGENDARY
}

[System.Serializable]
public abstract class BaseItemData : ScriptableObject
{
    [SerializeField]
    private string _name;

    [SerializeField]
    [TextArea(5, 10)]
    private string _description;

    [SerializeField]
    private int _baseMoneyValue;

    [SerializeField]
    public Sprite _previewSprite;

    [SerializeField]
    private ItemRarity _rarity;

    [SerializeField]
    private int _stackSize = 1;

    public ItemType ItemType
    {
        get { return _itemType; }
    }

    public string Name
    {
        get { return _name; }
    }

    public string Description
    {
        get { return _description; }
    }

    public Sprite PreviewSprite
    {
        get { return _previewSprite; }
    }

    public ItemRarity Rarity
    {
        get { return _rarity; }
    }

    public int StackSize
    {
        get { return _stackSize; }
    }

    public int BaseMoneyValue
    {
        get { return _baseMoneyValue; }
    }

    protected ItemType _itemType;

    protected BaseItemData(SerializationInfo info, StreamingContext context)
    {
        _name = info.GetString("name");
        _databaseName = info.GetString("databaseName");
        _description = info.GetString("description");
        _baseMoneyValue = info.GetInt32("money");
        //TODO sprite deserialization
        _previewSprite = Resources.Load<Sprite>("Sprites/Items/" + info.GetString("spriteName"));
        _rarity = (ItemRarity)info.GetInt32("rarity");
        _stackSize = info.GetInt32("stackSize");
        _itemType = (ItemType) info.GetInt32("itemType");
    }

    protected BaseItemData()
    {
    }

    public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("name", _name);
        info.AddValue("databaseName", _databaseName);
        info.AddValue("description", _description);
        info.AddValue("money", _baseMoneyValue);
        if (_previewSprite != null)
        {
            info.AddValue("spriteName", _previewSprite.name);
        }
        else
        {
            info.AddValue("spriteName", "");
        }

        info.AddValue("rarity", _rarity);
        info.AddValue("stackSize", _stackSize);
        info.AddValue("itemType", _itemType);
    }
}

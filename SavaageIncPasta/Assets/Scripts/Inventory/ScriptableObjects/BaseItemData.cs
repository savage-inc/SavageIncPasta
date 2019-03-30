using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
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
public abstract class BaseItemData : ScriptableObject, ISerializable
{
    [SerializeField]
    private string _name;

    [SerializeField]
    private string _databaseName;

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
        set { _name = value; }
    }

    public string Description
    {
        get { return _description; }
        set { _description = value; }
    }

    public Sprite PreviewSprite
    {
        get { return _previewSprite; }
        set { _previewSprite = value; }
    }

    public ItemRarity Rarity
    {
        get { return _rarity; }
        set { _rarity = value; }
    }

    public int StackSize
    {
        get { return _stackSize; }
    }

    public int BaseMoneyValue
    {
        get { return _baseMoneyValue; }
        set { _baseMoneyValue = value; }
    }

    public string DatabaseName
    {
        get { return _databaseName; }
        set { _databaseName = value; }
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

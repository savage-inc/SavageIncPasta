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
}

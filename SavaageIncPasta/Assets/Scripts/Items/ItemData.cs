using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    eCONSUMABLE,
    eARMOUR,
    eWEAPON
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
    public string Name;
    [TextArea(3, 10)]
    public string Description;
    public Sprite PreviewSprite;
    public ItemRarity Rarity;
    public int StackSize = 1;

    public ItemType ItemType
    {
        get { return _itemType; }
    }
        
    protected ItemType _itemType;
}

[CreateAssetMenu(fileName = "Data", menuName = "Items/ConsumableItem", order = 1)]
[System.Serializable]
public class ConsumableItemData : BaseItemData
{
    public enum Type
    {
        eHEALTH,
        eSTRENGTH,
        eCONSITUTION,
        eDEXTERITY
    }

    public ConsumableItemData()
    {
        base._itemType = ItemType.eCONSUMABLE;
    }

    public Type ConsumableType;
    public float EffectAmount;
    public int Charges = 1;
}

[CreateAssetMenu(fileName = "Data", menuName = "Items/ArmourItem", order = 1)]
[System.Serializable]
public class ArmourItemData : BaseItemData
{
    public enum Type
    {
        eHEAD,
        eCHEST,
        eLEGS,
    }

    public ArmourItemData()
    {
        base._itemType = ItemType.eARMOUR;
    }

    public Type ArmourType;
    public float Modifier;
}

[CreateAssetMenu(fileName = "Data", menuName = "Items/WeaponItem", order = 1)]
[System.Serializable]
public class WeaponItemData : BaseItemData
{
    public enum Type
    {
        eSWORD,
        eBOW,
        eSTAFF
    }

    public WeaponItemData()
    {
        base._itemType = ItemType.eWEAPON;
    }

    public Type WeaponType;
    public float MinDamage;
    public float MaxDamage;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    eCONSUMABLE,
    eARMOUR,
    eWEAPON,
    eMAGICWEAPON
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

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "Items/WeaponItem", order = 1)]
public class WeaponItemData : BaseItemData
{
    public enum Type
    {
        eSWORD,
        eBOW,
        eSTAFF
    }

    public enum StatType
    {
        eNONE,
        eSTRENGTH,
        eAGILTITY
    }

    public WeaponItemData()
    {
        base._itemType = ItemType.eWEAPON;
    }

    public Type WeaponType;
    public StatType statType;
    public float MinDamage;
    public float MaxDamage;
    public float MissFire;
    public bool IsMelee;
}

[CreateAssetMenu(fileName = "Data", menuName = "Items/MagicalWeaponItem", order = 1)]
[System.Serializable]
public class MagicWeaponItemData : WeaponItemData

{
    public enum MagicType
    {
        eNONE
    }

    public MagicWeaponItemData()
    {
        base._itemType = ItemType.eMAGICWEAPON;
    }

    public MagicType MagicalType;
    public float MagicalMin;
    public float MagicalMax;
}

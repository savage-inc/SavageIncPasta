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

public abstract class BaseItem : ScriptableObject
{
    public string Item;
    public string Description;
    public Sprite PreviewSprite;
    public ItemRarity Rarity;

    protected ItemType ItemType;
}

[CreateAssetMenu(fileName = "Data", menuName = "Items/ConsumableItem", order = 1)]
public class ConsumableItem : BaseItem
{
    public enum Type
    {
        eHEALTH,
        eSTRENGTH,
        eCONSITUTION,
        eDEXTERITY
    }

    public ConsumableItem()
    {
        ItemType = ItemType.eCONSUMABLE;
    }

    public Type ConsumableType;
    public float EffectAmount;
}

[CreateAssetMenu(fileName = "Data", menuName = "Items/ArmourItem", order = 1)]
public class ArmourItem : BaseItem
{
    public enum Type
    {
        eHEAD,
        eCHEST,
        eLEGS,
    }

    public Type ArmourType;
    public float Modifiier;
}

[CreateAssetMenu(fileName = "Data", menuName = "Items/WeaponItem", order = 1)]
public class WeaponItem : BaseItem
{
    public enum Type
    {
        eSWORD,
        eBOW,
        eSTAFF
    }

    public Type WeaponType;
    public float Damage;
}

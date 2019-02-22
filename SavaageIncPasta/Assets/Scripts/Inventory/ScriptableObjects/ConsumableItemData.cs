using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]
    private Type _consumableType;

    [SerializeField]
    private float _effectAmount;

    public Type ConsumableType
    {
        get { return _consumableType; }
    }

    public float EffectAmount
    {
        get { return _effectAmount; }
    }
}
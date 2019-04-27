using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Items/ConsumableItem", order = 1)]
[System.Serializable]
public class ConsumableItemData : BaseItemData
{
    public enum Type
    {
        eHEALTH,
        eMANA,
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

    protected ConsumableItemData(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        _consumableType = (Type)info.GetInt32("consumableType");
        _effectAmount = info.GetInt32("value");
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("consumableType", _consumableType);
        info.AddValue("value", _effectAmount);

    }
}
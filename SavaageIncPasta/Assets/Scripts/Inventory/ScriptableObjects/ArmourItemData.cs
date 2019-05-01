using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Items/ArmourItem", order = 1)]
[System.Serializable]
public class ArmourItemData : BaseItemData
{
    public enum SlotType
    {
        eHEAD,
        eCHEST,
        eLEGS,
    }

    public enum Type
    {
        eLIGHT,
        eMEDIUM,
        eHEAVY,
    }

    public ArmourItemData() : base()
    {
        base._itemType = ItemType.eARMOUR;
    }

    [SerializeField]
    private SlotType _armourSlotType;
    [SerializeField]
    private Type _armourType;
    [SerializeField]
    private float _value;
    [SerializeField]
    private MagicType _magicalType;

    public SlotType ArmourSlotType
    {
        get { return _armourSlotType; }
        set { _armourSlotType = value; }
    }

    public Type ArmourType
    {
        get { return _armourType; }
        set { _armourType = value; }
    }

    public float Value
    {
        get { return _value; }
        set { _value = value; }
    }

    public MagicType MagicalType
    {
        get { return _magicalType; }
        set { _magicalType = value; }
    }

    protected ArmourItemData(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        _armourSlotType = (SlotType)info.GetInt32("slotType");
        _armourType = (Type)info.GetInt32("armourType");
        _value = info.GetInt32("value");
        _magicalType = (MagicType)info.GetInt32("magicType");

        switch (_armourSlotType)
        {
            case SlotType.eHEAD:
                _previewSprite = Resources.Load<Sprite>("Sprites/Items/armour/helmet/" + info.GetString("spriteName"));
                break;
            case SlotType.eCHEST:
                _previewSprite = Resources.Load<Sprite>("Sprites/Items/armour/chest/" + info.GetString("spriteName"));
                break;
            case SlotType.eLEGS:
                _previewSprite = Resources.Load<Sprite>("Sprites/Items/armour/boots/" + info.GetString("spriteName"));
                break;
            default:
                break;
        }
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("slotType", _armourSlotType);
        info.AddValue("armourType", _armourType);
        info.AddValue("value", _value);
        info.AddValue("magicType", _magicalType);
    }
}
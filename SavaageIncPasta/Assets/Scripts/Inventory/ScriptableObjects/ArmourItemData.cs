using System.Collections;
using System.Collections.Generic;
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

    public ArmourItemData()
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
    }

    public Type ArmourType
    {
        get { return _armourType; }
    }

    public float Value
    {
        get { return _value; }
    }

    public MagicType MagicalType
    {
        get { return _magicalType; }
    }
}
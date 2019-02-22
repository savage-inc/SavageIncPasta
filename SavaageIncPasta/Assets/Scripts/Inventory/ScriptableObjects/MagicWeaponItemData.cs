using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Items/MagicalWeaponItem", order = 1)]
[System.Serializable]
public class MagicWeaponItemData : WeaponItemData
{
    public enum MagicType
    {
        eNONE,
        eRED,
        eWHITE,
        eGREEN
    }

    public MagicWeaponItemData()
    {
        base._itemType = ItemType.eMAGICWEAPON;
    }

    [SerializeField]
    private MagicType _magicalType;

    [SerializeField]
    private float _magicalMin;
    [SerializeField]
    private float _magicalMax;

    public MagicType MagicalType
    {
        get { return _magicalType; }
    }

    public float MagicalMin
    {
        get { return _magicalMin; }
    }

    public float MagicalMax
    {
        get { return _magicalMax; }
    }
}

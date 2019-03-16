using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "Items/WeaponItem", order = 1)]
public class WeaponItemData : BaseItemData
{
    public enum Type
    {
        eSWORD,
        eRANGE,
        eGUN
    }

    public enum SubType
    {
        eSHORTSWORD,
        eLONGSWORD,
        eWHIP,
        eRAPIER,
        eTHROWINGSTAR,
        eBOW,
        ePISTOL,
        eRIFLE
    }

    public enum StatTypes
    {
        eNONE,
        eSTRENGTH,
        eAGILTITY
    }

    public WeaponItemData()
    {
        base._itemType = ItemType.eWEAPON;
    }

    [SerializeField]
    private Type _weaponType;
    [SerializeField]
    private SubType _weaponSubType;
    [SerializeField]
    private StatTypes _statType;
    [SerializeField]
    private float _baseDamage;
    [SerializeField]
    private float _varianceDamage;
    [SerializeField]
    private float _minDamage;
    [SerializeField]
    private float _maxDamage;
    [SerializeField]
    private float _missFire;
    [SerializeField]
    private bool _isMelee;
    [SerializeField]
    private bool _isMainHand;
    [SerializeField]
    private MagicType _magicalType;
    [SerializeField]
    private float _magicalModifier;




    public float BaseDamage
    {
        get { return _baseDamage; }
        set { _baseDamage = value; }
    }

    public float VarianceDamage
    {
        get { return _varianceDamage; }
        set { _varianceDamage = value; }
    }

    public float MinDamage
    {
        get { return _minDamage; }
        set { _minDamage = value; }
    }

    public float MaxDamage
    {
        get { return _maxDamage; }
        set { _maxDamage = value; }
    }

    public float MissFire
    {
        get { return _missFire; }
        set { _missFire = value; }
    }

    public bool IsMelee
    {
        get { return _isMelee; }
        set { _isMelee = value; }
    }

    public bool IsMainHand
    {
        get { return _isMainHand; }
        set { _isMainHand = value; }
    }

    public MagicType MagicalType
    {
        get { return _magicalType; }
        set { _magicalType = value; }
    }

    public float MagicalModifier
    {
        get { return _magicalModifier; }
        set { _magicalModifier = value; }
    }

    public Type WeaponType
    {
        get { return _weaponType; }
        set { _weaponType = value; }
    }

    public StatTypes StatType
    {
        get { return _statType; }
        set { _statType = value; }
    }

    public SubType WeaponSubType
    {
        get { return _weaponSubType; }
        set { _weaponSubType = value; }
    }

    protected WeaponItemData(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        _weaponType = (Type)info.GetInt32("weaponType");
        _weaponSubType = (SubType)info.GetInt32("weaponSubType");
        _statType = (StatTypes)info.GetInt32("statType");
        _baseDamage = info.GetInt32("baseDamage");
        _minDamage = info.GetInt32("minDamage");
        _maxDamage = info.GetInt32("maxDamage");
        _varianceDamage = info.GetInt32("varianceDamage");
        _missFire = info.GetSingle("missfire");
        _isMelee = info.GetBoolean("melee");
        _isMainHand = info.GetBoolean("mainhand");
        _magicalType = (MagicType) info.GetInt32("magicalType");
        _magicalModifier = info.GetInt32("magicalModifer");
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info,context);
        info.AddValue("weaponType", _weaponType);
        info.AddValue("weaponSubType", _weaponSubType);
        info.AddValue("statType", _statType);
        info.AddValue("baseDamage", _baseDamage);
        info.AddValue("minDamage", _minDamage);
        info.AddValue("maxDamage", _maxDamage);
        info.AddValue("varianceDamage", _varianceDamage);
        info.AddValue("missFire", MissFire);
        info.AddValue("melee", _isMelee);
        info.AddValue("mainhand", _isMainHand);
        info.AddValue("magicalType", _magicalType);
        info.AddValue("magicalModifer", _magicalModifier);
    }
}
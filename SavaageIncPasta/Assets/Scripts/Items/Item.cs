using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseItem
{
    public BaseItem(BaseItemData data)
    {
        _itemData = data;
    }

    protected BaseItemData _itemData;

    public BaseItemData ItemData
    {
        get { return _itemData; }
    }

    public abstract void Use();
}

[System.Serializable]
public class ConsumableItem : BaseItem
{
    public int CurrentCharges;
    public ConsumableItem(ConsumableItemData data) : base(data)
    {
        CurrentCharges = data.Charges;
    }

    public new ConsumableItemData ItemData
    {
        get { return (ConsumableItemData) _itemData; }
    }

    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}

[System.Serializable]
public class ArmourItem : BaseItem
{
    public int Durability;
    public ArmourItem(ArmourItemData data) : base(data)
    {

    }

    public new ArmourItemData ItemData
    {
        get { return (ArmourItemData)_itemData; }
    }

    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}

[System.Serializable]
public class WeaponItem : BaseItem
{
    public int Durability;
    public WeaponItem(WeaponItemData data) : base(data)
    {

    }

    public new WeaponItemData ItemData
    {
        get { return (WeaponItemData)_itemData; }
    }

    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}


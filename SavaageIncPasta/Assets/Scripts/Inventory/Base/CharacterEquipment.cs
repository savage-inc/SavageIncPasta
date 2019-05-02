using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
public class CharacterEquipment : Inventory
{
    [System.NonSerialized]  public Character Character;
    private MagicType _armourMagicType;
    private MagicType _weaponMagicType;

    protected CharacterEquipment(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public CharacterEquipment() : base(5)
    {

    }

    public void EquipArmour(ArmourItemData armourItem ,Inventory partyInventory)
    {
        //check if the magic type of the item is the same as the rest
        if(armourItem.MagicalType != _armourMagicType && hasArmour())
        {
            return;
        }


        //Remove the item from the part inventory
        partyInventory.RemoveItem(armourItem.Name);

        BaseItemData itemToRemove = null;
        //check if a armour is already in the equipment 
        foreach (var inventoryItem in _inventoryItems)
        {
            if (inventoryItem.Item.ItemType == ItemType.eARMOUR)
            {
                ArmourItemData inventoryArmour = (ArmourItemData) inventoryItem.Item;
                if (inventoryArmour.ArmourSlotType == armourItem.ArmourSlotType)
                {
                    //armour item already equipped, add it back to the party inventory
                    partyInventory.AddItem(inventoryItem.Item);
                    //remove armour piece from equipment
                    itemToRemove = inventoryItem.Item;
                    break;
                }
            }
        }

        if(itemToRemove != null)
        {
            RemoveItem(itemToRemove.Name);
        }

        //Add it to the equipment
        AddItem(armourItem);

        _armourMagicType = armourItem.MagicalType;
    }

    public void EquipWeapon(WeaponItemData weaponItem, Inventory partyInventory)
    {
        //check if the magic type of the item is the same as the rest
        if (weaponItem.MagicalType != _weaponMagicType)
        {
            //first check if the chracter has a magic type
            if(Character.Magic != MagicType.eNONE)
            {
                //character has a magic type, check if the weapon is the same as the characters magic
                if(Character.Magic != weaponItem.MagicalType)
                {
                    return;
                }
            }
            else if(weaponItem.MagicalType != _weaponMagicType && hasWeapon())
            {
                return;
            }
        }

        //Remove the item from the part inventory
        partyInventory.RemoveItem(weaponItem.Name);

        BaseItemData itemToRemove = null;
        //check if a weapon is already in the equipment 
        foreach (var inventoryItem in _inventoryItems)
        {
            if (inventoryItem.Item.ItemType == ItemType.eWEAPON)
            {
                WeaponItemData inventoryWeaponItem = (WeaponItemData)inventoryItem.Item;
                if ((inventoryWeaponItem.IsMainHand == weaponItem.IsMainHand))
                {

                    //Weapon already equipped, add it back to the party inventory
                    partyInventory.AddItem(inventoryItem.Item);
                    //remove weapon from equipment
                    itemToRemove = inventoryItem.Item;
                    break;
                }
            }
        }

        if (itemToRemove != null)
        {
            RemoveItem(itemToRemove.Name);
        }

        //Add it to the equipment
        AddItem(weaponItem);

        _weaponMagicType = weaponItem.MagicalType;

    }

    public WeaponItemData GetEquippedWeapon()
    {
        foreach (var inventoryItem in _inventoryItems)
        {
            if (inventoryItem.Item.ItemType == ItemType.eWEAPON)
            {
                return (WeaponItemData)inventoryItem.Item;
            }
        }

        return null;
    }

    public int GetEquippedWeaponDamage()
    {
        int damage = 0;
        foreach (var inventoryItem in _inventoryItems)
        {
            if (inventoryItem.Item.ItemType == ItemType.eWEAPON)
            {
                damage += (int)((WeaponItemData)inventoryItem.Item).BaseDamage;
            }
        }

        return damage;
    }

    public int GetEquippedWeaponVarDamage()
    {
        int damage = 0;
        foreach (var inventoryItem in _inventoryItems)
        {
            if (inventoryItem.Item.ItemType == ItemType.eWEAPON)
            {
                damage += (int)((WeaponItemData)inventoryItem.Item).VarianceDamage;
            }
        }

        return damage;
    }

    public ArmourItemData GetEquippedArmour(ArmourItemData.SlotType armourSlotType)
    {
        foreach (var inventoryItem in _inventoryItems)
        {
            if (inventoryItem.Item.ItemType == ItemType.eARMOUR)
            {
                ArmourItemData inventoryArmour = (ArmourItemData)inventoryItem.Item;
                if (inventoryArmour.ArmourSlotType == armourSlotType)
                {
                    return (ArmourItemData) inventoryItem.Item;
                }
            }
        }

        return null;
    }

    public float GetArmourValue()
    {
        float value = 0;
        foreach (var item in _inventoryItems)
        {
            if (item.Item.ItemType == ItemType.eARMOUR)
            {
                ArmourItemData armourItem = (ArmourItemData)item.Item;
                value += armourItem.Value;
            }
        }
        return value;
    }

    public MagicType GetArmourMagicType()
    {
        foreach (var item in _inventoryItems)
        {
            if (item.Item.ItemType == ItemType.eARMOUR)
            {
                ArmourItemData armourItem = (ArmourItemData)item.Item;
                return armourItem.MagicalType;
            }
        }
        return MagicType.eNONE;
    }

    public MagicType GetWeaponMagicType()
    {
        foreach (var item in _inventoryItems)
        {
            if (item.Item.ItemType == ItemType.eWEAPON)
            {
                WeaponItemData weaponData = (WeaponItemData)item.Item;
                return weaponData.MagicalType;
            }
        }
        return MagicType.eNONE;
    }

    bool hasArmour()
    {
        foreach (var item in _inventoryItems)
        {
            if(item.Item.ItemType == ItemType.eARMOUR)
            {
                return true;
            }
        }
        return false;
    }

    bool hasWeapon()
    {
        foreach (var item in _inventoryItems)
        {
            if (item.Item.ItemType == ItemType.eWEAPON)
            {
                return true;
            }
        }
        return false;
    }
}

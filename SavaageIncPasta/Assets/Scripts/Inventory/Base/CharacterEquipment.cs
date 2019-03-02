using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment : Inventory
{

    public CharacterEquipment() : base(5)
    {

    }

    public void EquipArmour(ArmourItemData armourItem ,Inventory partyInventory)
    {
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
    }

    public void EquipWeapon(WeaponItemData weaponItem, Inventory partyInventory)
    {
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
    }

    public WeaponItemData GetEquipedWeapon()
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

    public ArmourItemData GetEquipedArmour(ArmourItemData.SlotType armourSlotType)
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
}

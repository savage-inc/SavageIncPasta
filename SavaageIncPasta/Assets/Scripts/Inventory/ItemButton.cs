using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public InventoryItem Item;
    public Inventory Inventory;

    public void TransferItem(Inventory to)
    {
        if(Inventory == null || to == null || Item.Item == null)
            return;

        to.AddItem(Item.Item);
        Inventory.RemoveItem(Item.Item.Name);

    }

    public void EquipItem(CharacterEquipment to)
    {
        if (Inventory == null || to == null || Item.Item == null)
            return;

        switch (Item.Item.ItemType)
        {
            case ItemType.eCONSUMABLE:
                break;
            case ItemType.eARMOUR:
                to.EquipArmour((ArmourItemData)Item.Item, Inventory);
                break;
            case ItemType.eWEAPON:
                to.EquipWeapon((WeaponItemData)Item.Item, Inventory);
                break;
            case ItemType.eMAGICWEAPON:
                to.EquipWeapon((WeaponItemData)Item.Item, Inventory);
                break;
        }
    }

    public void ConsumeItem()
    {
        if (Inventory == null || Item.Item == null)
            return;

        Inventory.RemoveItem(Item);
    }

    public void RemoveItem()
    {
        if (Inventory == null || Item.Item == null)
            return;

        Inventory.RemoveItem(Item);
    }
}

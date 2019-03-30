using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    public BaseItemData Item;
    public Text NameText;
    public Text Description;
    public Text Detail1;
    public Text Detail2;
    public Text Detail3;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        NameText.text = Item.Name;
        Description.text = Item.Description;

        switch (Item.ItemType)
        {
            case ItemType.eCONSUMABLE:
                var consumable = (ConsumableItemData) Item;
                Detail1.text = consumable.ConsumableType.ToString().Remove(0,1);
                Detail2.text = "Amount = " + consumable.EffectAmount;
                break;
            case ItemType.eARMOUR:
                var armour = (ArmourItemData)Item;
                Detail1.text = armour.ArmourType.ToString().Remove(0, 1) + " - " + armour.ArmourSlotType.ToString().Remove(0, 1);
                Detail2.text = "Amount = " + armour.Value;
                if (armour.MagicalType != MagicType.eNONE)
                {
                    Detail3.text = "Magic Type = " + armour.MagicalType.ToString().Remove(0, 1);
                }
                break;
            case ItemType.eWEAPON:
                var weapon = (WeaponItemData) Item;
                Detail1.text = weapon.WeaponType.ToString().Remove(0, 1) + " - " + weapon.WeaponSubType.ToString().Remove(0, 1);
                Detail2.text = weapon.MinDamage + " - " + weapon.MaxDamage;
                if (weapon.MagicalType != MagicType.eNONE)
                {
                    Detail3.text = "Magic Type = " + weapon.MagicalType.ToString().Remove(0, 1) + " - " + weapon.MagicalModifier;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        //Set name colour
        switch (Item.Rarity)
        {
            case ItemRarity.eCOMMON:
                NameText.color = Color.white;
                break;
            case ItemRarity.eUNCOMMON:
                NameText.color = Color.green;
                break;
            case ItemRarity.eRARE:
                NameText.color = Color.blue;
                break;
            case ItemRarity.eEPIC:
                NameText.color = Color.magenta;
                break;
            case ItemRarity.eLEGENDARY:
                NameText.color = Color.yellow;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

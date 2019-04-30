using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleInfoUI : MonoBehaviour
{
    public InventoryItem Item;
    public AbilityData Ability;

    public Text TextBox1;
    public Text TextBox2;
    public Text TextBox3;
    public Text TextBox4;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Ability != null)
        {
            DisplayAbilty(Ability);
        }
        else if (Item != null && Item.Item != null)
        {
            DisplayItem(Item);
        }
        else
        {
            Clear();
        }
	}

    public void Clear()
    {
        TextBox1.text = "";
        TextBox2.text = "";
        TextBox3.text = "";
        TextBox4.text = "";
    }

    void DisplayItem(InventoryItem item)
    {
        TextBox1.text = item.Item.Name + "x" + item.Amount;
        TextBox2.text = item.Item.Description;
        switch (Item.Item.ItemType)
        {
            case ItemType.eCONSUMABLE:
                var consumable = (ConsumableItemData)Item.Item;
                TextBox3.text = consumable.ConsumableType.ToString().Remove(0, 1);
                TextBox4.text = "Effect = " + consumable.EffectAmount;
                TextBox4.color = Color.black;
                break;
            case ItemType.eARMOUR:
                break;
            case ItemType.eWEAPON:
                break;
            default:
                break;
        }
    }

    void DisplayAbilty(AbilityData ability)
    {
        TextBox1.text = ability.AbilityName;
        TextBox2.text = "Level: " + ability.Level + " - Mana: " + ability.ManaCost;
        TextBox3.text = ability.AbilityDescription;
    }
}

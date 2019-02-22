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

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        NameText.text = Item.Name;
        Description.text = Item.Description;

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

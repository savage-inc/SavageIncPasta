using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    public CharacterComparison CharacterCompare;
    public Character Character;
    public int CharacterIndex = 0;

    private PartyInventory _partyInventory;


    private void Awake()
    {
        Character = FindObjectOfType<BarracksManager>().RandomCharacterPool[CharacterIndex];

    }

    public void Buy()
    {
        // Compares barracks stats
        CharacterCompare.character = Character;
        // CharacterCompare.BuyButton = this;


        /*
         * Gold is taken of party gold
         * Call BuyCharacter to allow for member to go to either party or clan
         * 
         */

    }
}

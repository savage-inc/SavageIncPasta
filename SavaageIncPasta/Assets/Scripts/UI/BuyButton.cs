/* Attach to BarracksMenu Content */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyButton : MonoBehaviour
{
    public CharacterComparison CharacterCompare;
    public Character Character;
    public int CharacterIndex = 0;


    private void Awake()
    {
        Character = FindObjectOfType<BarracksManager>().RandomCharacterPool[CharacterIndex];
    }

    public void Buy()
    {
        // Compares barracks stats
        CharacterCompare.character = Character;
    }
}
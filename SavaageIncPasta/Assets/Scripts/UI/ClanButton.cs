using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClanButton : MonoBehaviour
{
    public CharacterComparison CharacterCompare;
    public Character Character;
    public int CharacterIndex = 0;


    private void Awake()
    {
        Character = FindObjectOfType<ClanManager>().SpareCharacterPool[CharacterIndex];
    }

    public void SetToCompareMode()
    {
        // Compares clan stats
        CharacterCompare.character = Character;
        CharacterCompare.ClanButton = this;

    }
}
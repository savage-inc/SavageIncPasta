using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour {

    //TODO: Put character equipment in the character class. (HARD CODED FOR NOW)
    public CharacterEquipment CurrentCharacterEquipment;

    void Awake()
    {
        CurrentCharacterEquipment = new CharacterEquipment();
    }
}

/* Attach to buttons in Menu */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyButton : MonoBehaviour {

    [SerializeField]
    private Image myCharacter;

    public void SetIcon(Sprite mySprite)
    {
        myCharacter.sprite = mySprite;
    }
}

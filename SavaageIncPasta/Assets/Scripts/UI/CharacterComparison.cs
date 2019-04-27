using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterComparison : MonoBehaviour {

    public Character character;
    public Text Name;
    public Text Level;
    public Text Health;
    public Text Comfort;
    public Text Strength;
    public Text Dexterity;
    public Text Constitution;
    public Text Intelligence;
    public Text Charisma;

    public CharacterButton CharaterButton; // The character button that was used to compare with
    public ClanButton ClanButton; // The clan button that was used to compare with

    private void OnDisable()
    {
        Name.text = "Name: ";
        Level.text = "Level: ";
        Health.text = "Health: ";
        Comfort.text = "Comfort: ";
        Strength.text = "Strength: ";
        Dexterity.text = "Dexterity: ";
        Constitution.text = "Constitution: ";
        Charisma.text = "Charisma: ";
    }

    // Update is called once per frame
    void Update ()
    {
        if(character == null)
        {
            return;
        }

        Name.text = "Name: " + character.Name;
        Level.text = "Level: " + character.Level.ToString();
        Health.text = "Health: " + character.CurrentHealth.ToString();
        Comfort.text = "Comfort: " + character.Comfort.ToString();
        Strength.text = "Strength: " + character.Strength.ToString();
        Dexterity.text = "Dexterity: " + character.Dexterity.ToString();
        Constitution.text = "Constitution: " + character.Constitution.ToString();
        Intelligence.text = "Intelligence: " + character.Intelligence.ToString();
        Charisma.text = "Charisma: " + character.Charisma.ToString();
    }
}

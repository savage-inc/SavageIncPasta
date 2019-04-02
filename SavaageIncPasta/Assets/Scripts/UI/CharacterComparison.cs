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


    // Update is called once per frame
    void Update ()
    {
        Name.text = "Name: " + character.name;
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

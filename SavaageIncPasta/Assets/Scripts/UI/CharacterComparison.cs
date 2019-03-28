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
        Name.text = "name: " + character.name;
        Level.text = character.Level.ToString();
        Health.text = character.CurrentHealth.ToString();
        Comfort.text = character.Comfort.ToString();
        Strength.text = character.Strength.ToString();
        Dexterity.text = character.Dexterity.ToString();
        Constitution.text = character.Constitution.ToString();
        Intelligence.text = character.Intelligence.ToString();
        Charisma.text = character.Charisma.ToString();
    }
}

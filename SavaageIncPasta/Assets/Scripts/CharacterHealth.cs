using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour {

    public Text CharacterHealthText;
    public Character character;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        CharacterHealthText.text = character.CurrentHealth.ToString() + "/" + character.MaxHealth.ToString();
	}
}

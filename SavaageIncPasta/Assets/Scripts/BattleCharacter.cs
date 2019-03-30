using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class BattleCharacter : MonoBehaviour {

    public Character Character;
    public Text HealthText;
    public Sprite PreviewSprite;

    private void Awake()
    {
        GetComponent<Image>().sprite = PreviewSprite;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        HealthText.text = "Health: " + Character.CurrentHealth;
	}
}

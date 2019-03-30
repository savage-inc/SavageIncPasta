using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class BattleCharacter : MonoBehaviour {

    public Character Character;
    public Text HealthText;


    private void Awake()
    {
        GetComponent<Image>().sprite = PreviewSprite;
    }
    // Use this for initialization
    void Start () {
        GetComponent<SpriteRenderer>().sprite = FindObjectOfType<SpriteManager>().GetSprite(Character.SpritePreviewName);

    }

    // Update is called once per frame
    void Update () {
        HealthText.text = "Health: " + Character.CurrentHealth;
	}
}

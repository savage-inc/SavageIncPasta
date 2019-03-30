﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class BattleCharacter : MonoBehaviour {

    public Character Character;
    public Text HealthText;


    private void Awake()
    {
    }
    // Use this for initialization
    void Start () {
        GetComponent<Image>().sprite = FindObjectOfType<SpriteManager>().GetSprite(Character.SpritePreviewName);
    }

    // Update is called once per frame
    void Update () {
        HealthText.text = "Health: " + Character.CurrentHealth;
	}
}

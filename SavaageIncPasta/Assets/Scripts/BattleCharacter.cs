using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ActionChoice
{
    ePrimary,
    eSecondary
}

[RequireComponent(typeof(Image))]
public class BattleCharacter : MonoBehaviour {

    public Character Character;
    public Text HealthText;
    public ActionChoice CurrentAction = ActionChoice.ePrimary;
    public bool PrimaryAction = false;
    public bool SecondaryAction = false;
    public bool StartTurn = true;
    public int Initiative = 0;
    public float Defending = 1.0f;
    public int DamageTaken = 0;
    public int ClassModifier = 0;
    public int ChanceToHitModifier = 0;
        
    private void Awake()
    {
        switch (Character.Class)
        {
            case ClassType.eWARRIOR:
                ClassModifier = Character.Strength;
                break;
            case ClassType.eRANGER:
                ClassModifier = Character.Dexterity;
                break;
            case ClassType.eSHAMAN:
            case ClassType.eWIZARD:
                ClassModifier = Character.Intelligence;
                break;
            case ClassType.eENEMY:
                ClassModifier = Character.Strength;
                break;
        }

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

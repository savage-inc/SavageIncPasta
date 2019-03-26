using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterButton : MonoBehaviour
{

    public Button SwitchButton, CompareButton, CloseButton;
    public CharacterComparison CharacterCompare;
    public CharacterComparison CharacterSwitch;
    public Character Character;
    public int CharacterIndex = 0;



    private void Awake()
    {
        Character = FindObjectOfType<PlayerManager>().Characters[CharacterIndex];
    }

    public void ShowButtons()
    {
        if (!SwitchButton.gameObject.activeInHierarchy)
        {
            SwitchButton.gameObject.SetActive(true);
            CompareButton.gameObject.SetActive(true);
            CloseButton.gameObject.SetActive(true);
        }
        else
        {
            SwitchButton.gameObject.SetActive(false);
            CompareButton.gameObject.SetActive(false);
            CloseButton.gameObject.SetActive(false);
        }
    }

    public void SetToCompareMode()
    {
        // Compares party stats
        CharacterCompare.character = Character;
    }

    public void SetToSwitchMode()
    {
        // Switch party character
        CharacterSwitch.character = Character;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterButton : MonoBehaviour
{
    public Button SwitchButton, CloseButton;

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
            CloseButton.gameObject.SetActive(true);
        }
        else
        {
            SwitchButton.gameObject.SetActive(false);
            CloseButton.gameObject.SetActive(false);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterButton : MonoBehaviour
{
    public Button SwapButton, CompareButton, CloseButton;
    public CharacterComparison CharacterCompare;
    public CharacterComparison CharacterSwap;
    public Character Character;
    public int CharacterIndex = 0;


    private void Awake()
    {
        Character = FindObjectOfType<PlayerManager>().Characters[CharacterIndex];
    }

    private void Update()
    {
        Character = FindObjectOfType<PlayerManager>().Characters[CharacterIndex];
    }

    public void ShowButtons()
    {
        if (!SwapButton.gameObject.activeInHierarchy)
        {
            SwapButton.gameObject.SetActive(true);
            CompareButton.gameObject.SetActive(true);
            CloseButton.gameObject.SetActive(true);
        }
        else
        {
            SwapButton.gameObject.SetActive(false);
            CompareButton.gameObject.SetActive(false);
            CloseButton.gameObject.SetActive(false);
        }
    }

    public void SetToCompareMode()
    {
        // Compares party stats
        CharacterCompare.character = Character;    }



}
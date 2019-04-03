using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterButton : MonoBehaviour
{
    public Button SwapButton, CompareButton, CloseButton;
    public CharacterComparison PartyCharacterCompare;
    public CharacterComparison ClanCharacterCompare;
    public Character Character;
    public int CharacterIndex = 0;

    private PlayerManager _playerManager;
    private ClanManager _clanManager;


    private void Awake()
    {
        var playerManager = FindObjectOfType<PlayerManager>();
        Character = playerManager.Characters[CharacterIndex];
        //set sprite
        transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<SpriteManager>().GetSprite(Character.SpritePreviewName);

        _playerManager = FindObjectOfType<PlayerManager>();
        _clanManager = FindObjectOfType<ClanManager>();

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

        PartyCharacterCompare.character = Character;
        PartyCharacterCompare.CharaterButton = this;

    }

    public void SetToSwapMode()
    {
        // Switch party character
        //ClanCharacterCompare.character = Character;
        if (ClanCharacterCompare.character != null && PartyCharacterCompare.character != null)
        {
            Character character1 = PartyCharacterCompare.character;
            Character character2 = ClanCharacterCompare.character;

            //swap character data
            _clanManager.SwapCharacters(character1, character2);

            //swap button data
            Character = character2;
            ClanCharacterCompare.ClanButton.Character = character1;

            //swap sprites
            transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<SpriteManager>().GetSprite(character2.SpritePreviewName);
            ClanCharacterCompare.ClanButton.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<SpriteManager>().GetSprite(character1.SpritePreviewName);

            //update compare
            PartyCharacterCompare.character = character2;
            ClanCharacterCompare.character = character1;
        }
    }

}
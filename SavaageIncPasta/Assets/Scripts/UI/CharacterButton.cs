using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CharacterButton : MonoBehaviour
{
    public Button SwapButton, CompareButton, ColumnButton, CloseButton;
    public CharacterComparison PartyCharacterCompare;
    public CharacterComparison ClanCharacterCompare;
    public Character Character;
    public int CharacterIndex = 0;
    public Transform ClanContent;
    public GameObject ColumnConroller;

    private ClanManager _clanManager;
    private PlayerManager _playerManager;
    private EventSystem _eventSystem;

    private void OnDisable()
    {
        SwapButton.gameObject.SetActive(false);
        CompareButton.gameObject.SetActive(false);
        ColumnButton.gameObject.SetActive(false);
        CloseButton.gameObject.SetActive(false);
        _eventSystem.SetSelectedGameObject(transform.GetChild(0).gameObject);
        transform.GetChild(0).GetComponent<Button>().interactable = true;

        PartyCharacterCompare.character = null;
        ClanCharacterCompare.character = null;
    }

    private void Awake()
    {
        var playerManager = FindObjectOfType<PlayerManager>();

        if (CharacterIndex < playerManager.Characters.Count)
        {
            Character = playerManager.Characters[CharacterIndex];
            //set sprite
            transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<SpriteManager>().GetSprite(Character.SpritePreviewName);
            transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
        else
        {
            transform.GetChild(0).GetComponent<Image>().enabled = false;
        }
        _clanManager = FindObjectOfType<ClanManager>();
        _eventSystem = FindObjectOfType<EventSystem>();

    }

    private void Update()
    {
        if (_playerManager != null && CharacterIndex < _playerManager.Characters.Count)
        {
            Character = FindObjectOfType<PlayerManager>().Characters[CharacterIndex];
        }
    }

    public void ShowButtons()
    {
        if (!SwapButton.gameObject.activeInHierarchy)
        {
            SwapButton.gameObject.SetActive(true);
            CompareButton.gameObject.SetActive(true);
            ColumnButton.gameObject.SetActive(true);
            CloseButton.gameObject.SetActive(true);
            _eventSystem.SetSelectedGameObject(SwapButton.gameObject);
            transform.GetChild(0).GetComponent<Button>().interactable = false;
        }
        else
        {
            SwapButton.gameObject.SetActive(false);
            CompareButton.gameObject.SetActive(false);
            ColumnButton.gameObject.SetActive(false);
            CloseButton.gameObject.SetActive(false);
            _eventSystem.SetSelectedGameObject(transform.GetChild(0).gameObject);
            transform.GetChild(0).GetComponent<Button>().interactable = true;
        }
    }

    public void SetToCompareMode()
    {
        // Compares party stats

        PartyCharacterCompare.character = Character;
        PartyCharacterCompare.CharaterButton = this;

        //set event system to the clan selection
        _eventSystem.SetSelectedGameObject(ClanContent.GetChild(0).gameObject);

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

    public void SetToColumnMode()
    {
        SwapButton.gameObject.SetActive(false);
        CompareButton.gameObject.SetActive(false);
        ColumnButton.gameObject.SetActive(false);
        CloseButton.gameObject.SetActive(false);
        transform.GetChild(0).GetComponent<Button>().interactable = true;
        _eventSystem.SetSelectedGameObject(transform.GetChild(0).gameObject);


        var clanControl = FindObjectOfType<ClanControl>();
        clanControl.SelectedCharacter = Character;

        _eventSystem.SetSelectedGameObject(null, null);
        _eventSystem.SetSelectedGameObject(ColumnConroller);
    }

    public void GetPlayersColumn()
    {
        var clanControl = FindObjectOfType<ClanControl>();
        clanControl.SelectedColumn = Character.CurrCol;
        clanControl.ColumnText.text = clanControl.SelectedColumn.ToString();
    }

}
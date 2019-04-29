using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicLevelling : MonoBehaviour
{
    public GUIStyle FontStyle;
    private List<Character> _characters;
    private List<Character> _levelledUpCharacters;
    private PlayerManager _playerManager;
    private GameObject _choiceMenu;

    private void Awake()
    {
        _choiceMenu = GameObject.Find("Choice Menu");
        _levelledUpCharacters = new List<Character>();
        _playerManager = FindObjectOfType<PlayerManager>();
    }

    // Use this for initialization
    void Start()
    {
        _characters = _playerManager.Characters;
        CheckLevels();
    }

    public Character GetLevelledUpCharacter()
    {
        return _levelledUpCharacters[0];
    }

    public void CheckLevels()
    {
        //_characters[2].Experience += 20;

        foreach (Character character in _characters)
        {
            character.Experience += 20;
            //int xpNeededForNextLevel = 11 * (character.Level * character.Level) + 30;
            int xpNeededForNextLevel = 19;

            if (character.Experience >= xpNeededForNextLevel)
            {
                character.Level++;

                _levelledUpCharacters.Add(character);
            }
        }
    }

    private void Update()
    {
        if (_levelledUpCharacters.Count > 0 && (Input.GetKeyDown(KeyCode.Joystick1Button6)))
        {
            GameObject.Find("UICanvas").GetComponent<LevelUpMenu>().LevelledUp();
        }
    }

    public void RemoveFirstLevelledUpCharacterFromList()
    {
        _levelledUpCharacters.RemoveAt(0);
    }

    private void OnGUI()
    {
        if (_levelledUpCharacters.Count > 0)
        {
            GUI.Box(new Rect(Screen.width - 350, 50, 350, 50), "A character has levelled up!", FontStyle);

            if (_choiceMenu.activeInHierarchy)
            {
                GUI.Box(new Rect(Screen.width / 2 - 100, 100, 250, 50), _levelledUpCharacters[0].Name, FontStyle);
                GUI.Box(new Rect(Screen.width / 2 - 100, 170, 250, 50), "Welcome to level " + _levelledUpCharacters[0].Level, FontStyle);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicLevelling : MonoBehaviour
{
    private List<Character> _characters;
    private List<Character> _levelledUpCharacters;
    private PlayerManager _playerManager;

    //Checks if any character has levelled up when the scene changes
    private void Awake()
    {
        _levelledUpCharacters = new List<Character>();
        _playerManager = FindObjectOfType<PlayerManager>();
        _characters = _playerManager.Characters;
        CheckLevels();
    }

    public Character GetLevelledUpCharacter()
    {
        return _levelledUpCharacters[0];
    }

    public int GetNumOfCharactersLevelledUp()
    {
        return _levelledUpCharacters.Count;
    }

    //Checks the amount of xp the character has and how much they need to level up
    public void CheckLevels()
    {
        foreach (Character character in _characters)
        {
            int xpNeededForNextLevel = 11 * (character.Level * character.Level) + 30;
            character.Experience = 250;
            if (character.Experience >= xpNeededForNextLevel)
            {
                character.MaxComfort += character.Constitution * 5 + character.Level * 2;
                character.Comfort = character.MaxComfort;

                character.MaxHealth += 2 * character.Constitution + 3 * character.Level;
                character.CurrentHealth = character.MaxHealth;

                if (character.Class == ClassType.eSHAMAN || character.Class == ClassType.eWIZARD)
                {
                    character.MaxMana += character.Level + character.Intelligence - 3;
                    character.CurrentMana = character.MaxMana;
                }

                _levelledUpCharacters.Add(character);
            }
        }
    }

    public void RemoveFirstLevelledUpCharacterFromList()
    {
        _levelledUpCharacters.RemoveAt(0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicLevelling : MonoBehaviour
{
    private List<Character> _characters;
    private List<Character> _levelledUpCharacters;
    private PlayerManager _playerManager;

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

    public void CheckLevels()
    {
        foreach (Character character in _characters)
        {
            int xpNeededForNextLevel = 11 * (character.Level * character.Level) + 30;
            character.Experience = 250;
            if (character.Experience >= xpNeededForNextLevel)
            {
                character.Level++;
                character.MaxHealth = 11 * (character.Level * character.Level) + 30;
                character.CurrentHealth = character.MaxHealth;

                if (character.Class == ClassType.eSHAMAN || character.Class == ClassType.eWIZARD)
                {
                    character.MaxMana = 11 * (character.Level * character.Level) + 30;
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

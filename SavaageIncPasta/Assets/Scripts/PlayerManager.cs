using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public List<Character> Characters;
    public int NumofCharacters;

    public void AddCharacter(Character c)
    {
        Characters.Add(c);
    }

    public void RemoveCharacter(Character c)
    {
        Characters.Remove(c);
    }


    private void Awake()
    {
        Characters = new List<Character>();

        for(int i = 0; i < NumofCharacters; i++)
        {
            Character character = GenerateRandomCharacter.GenerateCharacter();

            AddCharacter(character);
        }
    }

    public bool IsAlive()
    {
        foreach (var character in Characters)
        {
            if (character.Alive)
            {
                return true;
            }
        }
        return false;
    }
}

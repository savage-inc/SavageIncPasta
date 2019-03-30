﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public List<Character> Characters;
    public int NumofCharacters;

    void AddCharacter(Character c)
    {
        Characters.Add(c);
    }

    void RemoveCharacter(Character c)
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

}

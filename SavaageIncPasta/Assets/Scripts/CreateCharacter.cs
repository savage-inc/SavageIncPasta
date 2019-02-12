﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Data", menuName = "Character")]

public class CharacterData : ScriptableObject
{
    public string CharacterName;
    public ClassType Class;
    public int Health;
    public int Mana;
    public int Strength;
    public int Constitution;
    public int Dexterity;
    public int Intelligence;
    public int Charisma;
    public int Agility;
    public int StartColumn;
    public int CurrentColumn;
}

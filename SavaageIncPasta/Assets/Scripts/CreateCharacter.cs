using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Data", menuName = "Class")]

public class CharacterData : ScriptableObject
{
    public string ClassName;
    public int Health;
    public int Mana;
    public int Strength;
    public int Constitution;
    public int Dexterity;
    public int Intelligence;
    public int Charisma;
}

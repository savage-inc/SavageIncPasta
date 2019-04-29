﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Character
{
    public System.Guid ID;
    public string Name;
    public string SpritePreviewName;

    public ClassType Class;
    public int Level;
    public int MaxHealth;
    public int CurrentHealth;
    public int Strength;
    public int Constitution;
    public int Dexterity;
    public int Intelligence;
    public int Charisma;
    public MagicType Magic;
    public int Comfort;
    public int MaxComfort;


    public bool Player = true;
    public bool Alive = true;
    public CharacterEquipment Equipment;
    public int BaseAttack = 5;
    public int BaseArmour = 0;
    public int CurrCol = 1;
    public int MaxMana;
    public int CurrentMana;
    public List<int> Abilities = new List<int>() { 1,2 };

    public int GoldDrop = 5;
    public int Experience = 0;

    public Character()
    {
        Equipment = new CharacterEquipment
        {
            Character = this
        };
        CurrentHealth = MaxHealth;
        CurrentMana = MaxMana;
    }

    public void ChangeHealth(int health)
    {
        CurrentHealth += health;

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Alive = false;
        }
        else if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }

    public void ChangeMana(int mana)
    {
        CurrentMana += mana;

        if (CurrentMana <= 0)
        {
            CurrentHealth = 0;
        }
        else if (CurrentMana > MaxMana)
        {
            CurrentMana = MaxMana;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour
{
    public ClassType Class { get; private set; }
    public int Level { get; set; }
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public int Strength { get; set; }
    public int Constitution { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }
    public int Charisma { get; set; }
    public MagicType Magic { get; set; }

    public bool Player = true;
    public bool Alive { get; set; }

    private void Awake()
    {
        GenerateStats();
    }

    private void GenerateStats()
    {
        Class = (ClassType)Random.Range(0, 3);
        Alive = true;
        switch(Class)
        {
            case ClassType.eWARRIOR:
                {
                    Level = 1;
                    Strength = Random.Range(4, 7);
                    Constitution = Random.Range(4, 7);
                    Dexterity = Random.Range(1, 4);
                    Intelligence = Random.Range(1, 4);
                    Charisma = Random.Range(1, 8);
                    MaxHealth = 4 * Constitution + 6 * Level;
                    CurrentHealth = MaxHealth;
                    Magic = MagicType.eNONE;
                    break;
                }
            case ClassType.eRANGER:
                {
                    Level = 1;
                    Strength = Random.Range(1, 4);
                    Constitution = Random.Range(4, 7);
                    Dexterity = Random.Range(4, 7);
                    Intelligence = Random.Range(1, 4);
                    Charisma = Random.Range(1, 8);
                    MaxHealth = 4 * Constitution + 6 * Level;
                    CurrentHealth = MaxHealth;
                    Magic = MagicType.eNONE;
                    break;
                }
            case ClassType.eWIZARD:
                {
                    Level = 1;
                    Strength = Random.Range(1, 4);
                    Constitution = Random.Range(1, 4);
                    Dexterity = Random.Range(1, 4);
                    Intelligence = Random.Range(4, 7);
                    Charisma = Random.Range(1, 8);
                    MaxHealth = 4 * Constitution + 6 * Level;
                    CurrentHealth = MaxHealth;
                    Magic = (MagicType)Random.Range(1, 3);
                    break;
                }
            case ClassType.eSHAMAN:
                {
                    Level = 1;
                    Strength = Random.Range(1, 4);
                    Constitution = Random.Range(1, 4);
                    Dexterity = Random.Range(1, 4);
                    Intelligence = Random.Range(4, 7);
                    Charisma = Random.Range(1, 8);
                    MaxHealth = 4 * Constitution + 6 * Level;
                    CurrentHealth = MaxHealth;
                    Magic = (MagicType)Random.Range(1, 3);
                    break;
                }
        }
    }

    public void ChangeHealth(int health)
    {
        CurrentHealth += health;

        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Alive = false;
        }
        else if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }
}

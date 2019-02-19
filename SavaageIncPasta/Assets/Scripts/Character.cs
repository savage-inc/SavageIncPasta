using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour
{
    public ClassType Class;
    public int Level = 1;
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public int Strength { get; set; }
    public int Constitution { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }
    public int Charisma { get; set; }
    public MagicType Magic;

    // Use this for initialization
    void Start ()
    {
        GenerateStats();		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void GenerateStats()
    {
        Class = (ClassType)Random.Range(0, 3);

        switch(Class)
        {
            case ClassType.WARRIOR:
                {
                    Strength = Random.Range(4, 7);
                    Constitution = Random.Range(4, 7);
                    Dexterity = Random.Range(1, 4);
                    Intelligence = Random.Range(1, 4);
                    Charisma = Random.Range(1, 8);
                    MaxHealth = 4 * Constitution + 6 * Level;
                    CurrentHealth = MaxHealth;
                    Magic = MagicType.NONE;
                    break;
                }
            case ClassType.RANGER:
                {
                    Strength = Random.Range(1, 4);
                    Constitution = Random.Range(4, 7);
                    Dexterity = Random.Range(4, 7);
                    Intelligence = Random.Range(1, 4);
                    Charisma = Random.Range(1, 8);
                    MaxHealth = 4 * Constitution + 6 * Level;
                    CurrentHealth = MaxHealth;
                    Magic = MagicType.NONE;
                    break;
                }
            case ClassType.WIZARD:
                {
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
            case ClassType.SHAMAN:
                {
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
}

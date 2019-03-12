using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRandomCharacter
{ 


    public static Character GenerateCharacter()
    {
        Character character = new Character();

        character.Class = (ClassType)Random.Range(0, 3);
        character.Alive = true;
        switch (character.Class)
        {
            case ClassType.eWARRIOR:
                {
                    character.Level = 1;
                    character.Strength = Random.Range(4, 7);
                    character.Constitution = Random.Range(4, 7);
                    character.Dexterity = Random.Range(1, 4);
                    character.Intelligence = Random.Range(1, 4);
                    character.Charisma = Random.Range(1, 8);
                    character.MaxHealth = 4 * character.Constitution + 6 * character.Level;
                    character.CurrentHealth = character.MaxHealth;
                    character.Magic = MagicType.eNONE;
                    break;
                }
            case ClassType.eRANGER:
                {
                    character.Level = 1;
                    character.Strength = Random.Range(1, 4);
                    character.Constitution = Random.Range(4, 7);
                    character.Dexterity = Random.Range(4, 7);
                    character.Intelligence = Random.Range(1, 4);
                    character.Charisma = Random.Range(1, 8);
                    character.MaxHealth = 4 * character.Constitution + 6 * character.Level;
                    character.CurrentHealth = character.MaxHealth;
                    character.Magic = MagicType.eNONE;
                    break;
                }
            case ClassType.eWIZARD:
                {
                    character.Level = 1;
                    character.Strength = Random.Range(1, 4);
                    character.Constitution = Random.Range(1, 4);
                    character.Dexterity = Random.Range(1, 4);
                    character.Intelligence = Random.Range(4, 7);
                    character.Charisma = Random.Range(1, 8);
                    character.MaxHealth = 4 * character.Constitution + 6 * character.Level;
                    character.CurrentHealth = character.MaxHealth;
                    character.Magic = (MagicType)Random.Range(1, 3);
                    break;
                }
            case ClassType.eSHAMAN:
                {
                    character.Level = 1;
                    character.Strength = Random.Range(1, 4);
                    character.Constitution = Random.Range(1, 4);
                    character.Dexterity = Random.Range(1, 4);
                    character.Intelligence = Random.Range(4, 7);
                    character.Charisma = Random.Range(1, 8);
                    character.MaxHealth = 4 * character.Constitution + 6 * character.Level;
                    character.CurrentHealth = character.MaxHealth;
                    character.Magic = (MagicType)Random.Range(1, 3);
                    break;
                }
        }

        return character;
    }

   
}





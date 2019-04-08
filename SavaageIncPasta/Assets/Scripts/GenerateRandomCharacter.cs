using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRandomCharacter
{
    public static Character GenerateCharacter()
    {
        Character character = new Character();

        character.ID = System.Guid.NewGuid();
        character.Name = "Character " + character.ID;
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
                    character.Comfort = character.Constitution * 10 + character.Level * 4;
                    character.Magic = MagicType.eNONE;

                    //random sprite
                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            character.SpritePreviewName = "Warrior1";
                            break;
                        case 1:
                            character.SpritePreviewName = "Warrior2";
                            break;
                        case 2:
                            character.SpritePreviewName = "Warrior3";
                            break;
                    }
                   
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
                    character.Comfort = character.Constitution * 10 + character.Level * 4;           
                    character.Magic = MagicType.eNONE;

                    //random sprite
                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            character.SpritePreviewName = "Ranger1";
                            break;
                        case 1:
                            character.SpritePreviewName = "Ranger2";
                            break;
                        case 2:
                            character.SpritePreviewName = "Ranger3";
                            break;
                    }
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
                    character.Comfort = character.Constitution * 10 + character.Level * 4;
                    character.Magic = (MagicType)Random.Range(1, 3);
                    character.Mana = character.Intelligence * 2;

                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            character.SpritePreviewName = "Wizard1";
                            break;
                        case 1:
                            character.SpritePreviewName = "Wizard2";
                            break;
                        case 2:
                            character.SpritePreviewName = "Wizard3";
                            break;
                    }
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
                    character.Comfort = character.Constitution * 10 + character.Level * 4;
                    character.Magic = (MagicType)Random.Range(1, 3);
                    character.Mana = character.Intelligence * 2;

                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            character.SpritePreviewName = "Shaman1";
                            break;
                        case 1:
                            character.SpritePreviewName = "Shaman2";
                            break;
                        case 2:
                            character.SpritePreviewName = "Shaman3";
                            break;
                    }
                    break;
                }
        }

        return character;
    }

   
}





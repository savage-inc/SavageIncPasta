using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRandomCharacter
{
    public static Character GenerateCharacter()
    {
        List<string> names = new List<string>()
        {
            "Joe",
            "Jo",
            "Joseph",
            "Josef",
            "Josephine",
            "Josie",
            "JoJo",
            "JoeJoe",
            "JoJoe",
            "JoeJo",
            "Jolene",
            "Josy",
            "Joel",
            "Jolie",
            "Joly",
            "Jose",
            "José",
            "Joey",
            "Joan",
            "Jone",
            "Juan",
            "Joe'dan",
            "Jody",
            "Josiah",
            "Josias",
            "Joseba",
            "Josephus",
            "Joanne",
            "Joanna",
            "Jonas",
            "Jones",
            "Josephina"
        };

        Character character = new Character();

        character.ID = System.Guid.NewGuid();
        character.Name = names[Random.Range(0,names.Count-1)];
        character.Class = (ClassType)Random.Range(0, 4);
        character.Alive = true;
        switch (character.Class)
        {
            case ClassType.eWARRIOR:
                {
                    character.Level = 1;
                    character.Strength = Random.Range(4, 8);
                    character.Constitution = Random.Range(4, 8);
                    character.Dexterity = Random.Range(1, 5);
                    character.Intelligence = Random.Range(1, 5);
                    character.Charisma = Random.Range(1, 9);
                    character.MaxHealth = 4 * character.Constitution + 6 * character.Level;
                    character.CurrentHealth = character.MaxHealth;
                    character.Comfort = character.Constitution * 10 + character.Level * 4;
                    character.MaxComfort = character.Comfort;
                    character.Magic = MagicType.eNONE;
                    character.MaxMana = 0;

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
                    character.Strength = Random.Range(1, 5);
                    character.Constitution = Random.Range(4, 8);
                    character.Dexterity = Random.Range(4, 8);
                    character.Intelligence = Random.Range(1, 5);
                    character.Charisma = Random.Range(1, 9);
                    character.MaxHealth = 4 * character.Constitution + 6 * character.Level;
                    character.CurrentHealth = character.MaxHealth;
                    character.Comfort = character.Constitution * 10 + character.Level * 4;           
                    character.Magic = MagicType.eNONE;
                    character.MaxMana = 0;

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
                    character.Strength = Random.Range(1, 5);
                    character.Constitution = Random.Range(1, 5);
                    character.Dexterity = Random.Range(1, 5);
                    character.Intelligence = Random.Range(4, 8);
                    character.Charisma = Random.Range(1, 9);
                    character.MaxHealth = 4 * character.Constitution + 6 * character.Level;
                    character.CurrentHealth = character.MaxHealth;
                    character.Comfort = character.Constitution * 10 + character.Level * 4;
                    character.Magic = (MagicType)Random.Range(1, 4);
                    character.MaxMana = character.Intelligence * 2;
                    character.CurrentMana = character.MaxMana;

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
                    character.Strength = Random.Range(1, 5);
                    character.Constitution = Random.Range(1, 5);
                    character.Dexterity = Random.Range(1, 5);
                    character.Intelligence = Random.Range(4, 8);
                    character.Charisma = Random.Range(1, 9);
                    character.MaxHealth = 4 * character.Constitution + 6 * character.Level;
                    character.CurrentHealth = character.MaxHealth;
                    character.Comfort = character.Constitution * 10 + character.Level * 4;
                    character.Magic = (MagicType)Random.Range(1, 4);
                    character.MaxMana = character.Intelligence * 2;
                    character.CurrentMana = character.MaxMana;

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





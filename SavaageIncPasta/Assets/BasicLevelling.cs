using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLevelling : MonoBehaviour
{
    public int Level; //Exists in character class
    public int CurrentXP; //^^
    private Character _character;

    // Use this for initialization
    void Start()
    {
        _character = gameObject.GetComponent<Character>();
        Level = _character.Level;
        CurrentXP = _character.Experience;
    }

    public void CheckLevel()
    {
        int xpNeededForNextLevel = 11 * (Level * Level) + 30;

        if (CurrentXP >= xpNeededForNextLevel)
        {
            Level++;

            switch (_character.Class)
            {
                case ClassType.eWARRIOR:
                    IncreaseStats(2, 2, 0, 0, 1);
                    WarriorsPathway(true);
                    break;
                case ClassType.eRANGER:
                    IncreaseStats(0, 2, 2, 0, 1);
                    break;
                case ClassType.eWIZARD:
                    IncreaseStats(0, 1, 1, 2, 1);
                    break;
                case ClassType.eSHAMAN:
                    IncreaseStats(1, 1, 1, 2, 0);
                    break;
                default:
                    IncreaseStats(1, 1, 1, 1, 1);
                    break;
            }
        }
    }

    private void IncreaseStats(int strengthIncreaseValue, int constitutionIncreaseValue, int dexterityIncreaseValue, int intelligenceIncreaseValue, int charismaIncreaseValue)
    {
        _character.Strength += strengthIncreaseValue;
        _character.Constitution += constitutionIncreaseValue;
        _character.Dexterity += dexterityIncreaseValue;
        _character.Intelligence += intelligenceIncreaseValue;
        _character.Charisma += charismaIncreaseValue;
    }

    private void WarriorsPathway(bool knight)
    {
        switch (_character.Level)
        {
            case 1:
                //way of the warrior
                break;
            case 2:
                if (knight)
                {
                    //shield master
                }
                else
                {
                    //charge
                }
                break;
            case 3:
                if (knight)
                {
                    //constitution up
                    IncreaseStats(0, 1, 0, 0, 0);
                    _character.Constitution++;
                }
                else
                {
                    //strength up
                    IncreaseStats(1, 0, 0, 0, 0);
                    _character.Strength++;
                }
                break;
            case 4:
                if (knight)
                {
                    //protector
                }
                else
                {
                    //rage
                }
                break;
            case 5:
                //brutal critical and strength up
                IncreaseStats(1, 0, 0, 0, 0);
                _character.Strength++;
                break;
            case 6:
                if (knight)
                {
                    //honour guard
                }
                else
                {
                    //frenzy
                }
                break;
            case 7:
                //constitution up
                IncreaseStats(0, 1, 0, 0, 0);
                _character.Constitution++;
                break;
            case 8:
                if (knight)
                {
                    //constitution up and noble rite
                    IncreaseStats(0, 1, 0, 0, 0);
                    _character.Constitution++;
                }
                else
                {
                    //strength up and fury
                    IncreaseStats(1, 0, 0, 0, 0);
                    _character.Strength++;
                }
                break;
            default:
                break;
        }
    }
}

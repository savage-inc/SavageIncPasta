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
                    break;
                case ClassType.eRANGER:
                    break;
                case ClassType.eWIZARD:
                    break;
                case ClassType.eSHAMAN:
                    break;
                default:
                    break;
            }
        }
    }

    private void WarriorsPathway(bool knight)
    {
        switch (_character.Level + 1)
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
                    _character.Constitution++;
                }
                else
                {
                    //strength up
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
                _character.Constitution++;
                break;
            case 8:
                if (knight)
                {
                    //constitution up and noble rite
                    _character.Constitution++;
                }
                else
                {
                    //strength up and fury
                    _character.Strength++;
                }
                break;
            default:
                break;
        }
    }
}

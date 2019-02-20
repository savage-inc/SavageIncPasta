using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnOption
{
    eNONE,
    eATTACK,
    eDEFEND,
    eMOVE
}

public class Battle : MonoBehaviour
{
    private TurnOption _optionChosen = TurnOption.eNONE;
    private TurnOption _optionChoosing = TurnOption.eATTACK;
    private List<Character> _characterList = new List<Character>(); // all players in battle
    private List<Character> _characterTurnOrder = new List<Character>(); // all players sorted into turn order
    private int _currentCharacter = 0;
    private int _targettedCharacter = -1;
    private int _targettingCharacter = 0;

    public Character Player1;
    public Character Enemy1;

    // Use this for initialization
    void Start ()
    {
        _characterList.Add(Player1);
        _characterList.Add(Enemy1);

        DecideTurnOrder();

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (_currentCharacter >= _characterTurnOrder.Count)
        {
            _currentCharacter = 0;
        }

        if (_optionChosen == 0 && _characterTurnOrder[_currentCharacter].Player)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if ((int)_optionChoosing < 3)
                {
                    _optionChoosing++;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if ((int)_optionChoosing > 1)
                {
                    _optionChoosing--;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                _optionChosen = _optionChoosing;
            }
        }
        else
        {
            _optionChosen = TurnOption.eATTACK;
        }

        if (_optionChosen > 0)
        {
            switch (_optionChosen)
            {
                case TurnOption.eATTACK:
                    Attack(_characterTurnOrder[_currentCharacter]);
                    break;
                case TurnOption.eDEFEND:
                    Defend(_characterTurnOrder[_currentCharacter]);
                    break;
                case TurnOption.eMOVE:
                    Move(_characterTurnOrder[_currentCharacter]);
                    break;
                default:
                    break;
            }
        }

        if (_currentCharacter > _characterList.Count)
        {
            DecideTurnOrder();
            _currentCharacter = 0;
        }
	}


    void DecideTurnOrder()
    {
        _characterTurnOrder.Clear();

        // Decides turn order
        foreach (Character p in _characterList)
        {
            //int rand = Random.Range(-2, 2);

            if (_characterTurnOrder.Count == 0)
            {
                _characterTurnOrder.Add(p);
            }
            else
            {
                bool added = false;
                for (int i = 0; i < _characterTurnOrder.Count; i++)
                {
                    if (p.Dexterity < _characterTurnOrder[i].Dexterity)
                    {
                        _characterTurnOrder.Insert(i, p);
                        added = true;
                    }
                    
                }
                if (!added)
                {
                    _characterTurnOrder.Add(p);
                }
            }

        }
    }

    void Attack(Character attacker)
    {
        if (attacker.Player)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (_targettingCharacter < _characterTurnOrder.Count)
                {
                    _targettingCharacter++;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (_targettingCharacter > 1)
                {
                    _targettingCharacter--;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                _targettedCharacter = _targettingCharacter;
            }

        }
        else
        {
            _targettedCharacter = 0;
        }
        if (_targettedCharacter > -1)
        {
            _optionChosen = 0;

            _characterList[_targettedCharacter].CurrentHealth -= 5;

            _targettingCharacter = 0;
            _targettedCharacter = -1;
            _currentCharacter++;
        }
        
    }

    void Defend(Character defender)
    {
        _optionChosen = 0;
        //defend
        _currentCharacter++;
    }

    void Move(Character mover)
    {
        _optionChosen = 0;
        //move
        _currentCharacter++;
    }

    public TurnOption GetOptionChoosing()
    {
        return _optionChoosing;
    }
}


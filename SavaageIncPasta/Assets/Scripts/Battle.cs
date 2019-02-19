using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    private int _optionChosen = 0;
    private int _optionChoosing = 0;
    private List<Character> _characterList = new List<Character>(); // all players in battle
    private List<Character> _characterTurnOrder = new List<Character>(); // all players sorted into turn order
    private int _currentCharacter = 0;
    private int _targettedCharacter = -1;
    private int _targettingCharacter = 0;

	// Use this for initialization
	void Start ()
    {
        Character player1 = new Character();
        Character enemy1 = new Character();
        _characterList.Add(player1);
        _characterList.Add(enemy1);


        DecideTurnOrder();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _optionChoosing++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _optionChoosing--;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            _optionChosen = _optionChoosing;
        }

		switch (_optionChosen)
        {
            case 1:
                Attack(_characterTurnOrder[_currentCharacter]);
                break;
            case 2:
                Defend(_characterTurnOrder[_currentCharacter]);
                break;
            case 3:
                Move(_characterTurnOrder[_currentCharacter]);
                break;
            default:
                break;
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
            int rand = Random.Range(-2, 3);
            for (int i = 0; i < _characterList.Count; i++)
            {
                if (_characterTurnOrder.Count == 0)
                {
                    _characterTurnOrder.Add(p);
                    break;
                }
                else if (p.Dexterity + rand < _characterTurnOrder[i].Dexterity + rand)
                {
                    _characterTurnOrder.Insert(i, p);
                }
            }
        }
    }

    void Attack(Character attacker)
    {
        _optionChosen = 0;

        while (_targettedCharacter == -1)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _targettingCharacter++;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _targettingCharacter--;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                _targettedCharacter = _targettingCharacter;
            }
        }

        _characterList[_targettedCharacter].CurrentHealth -= 5;

        _targettingCharacter = 0;
        _targettedCharacter = -1;
        _currentCharacter++;
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
}


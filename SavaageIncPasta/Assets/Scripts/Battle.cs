using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    private int _optionChosen { get; set; }
    private List<Player> _characterList = new List<Player>(); // all players in battle
    private List<Player> _characterTurnOrder = new List<Player>(); // all players sorted into turn order
    private int _currentCharacter = 0;
    private int _targettedCharacter = 0;

	// Use this for initialization
	void Start ()
    {
        DecideTurnOrder();
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		switch (_optionChosen)
        {
            case 1:
                Attack(_characterTurnOrder[_currentCharacter], _characterList[_targettedCharacter]);
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
        // Decides turn order
        foreach (Player p in _characterList)
        {
            int rand = Random.Range(-2, 3);
            for (int i = 0; i < _characterList.Count; i++)
            {
                if (_characterTurnOrder.Count == 0)
                {
                    _characterTurnOrder.Add(p);
                    break;
                }
                else if (p.GetCharacterData().Agility + rand < _characterTurnOrder[i].GetCharacterData().Agility + rand)
                {
                    _characterTurnOrder.Insert(i, p);
                }
            }

        }
    }

    void Attack(Player attacker, Player target)
    {
        _optionChosen = 0;
        //do attack
        _currentCharacter++;
    }

    void Defend(Player defender)
    {
        _optionChosen = 0;
        //defend
        _currentCharacter++;
    }

    void Move(Player mover)
    {
        _optionChosen = 0;
        //move
        _currentCharacter++;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private int _targettingCharacter = 4;
    private int deadEnemies = 0;
    private int deadPlayers = 0;

    public Character Player1;
    public Character Player2;
    public Character Player3;
    public Character Player4;
    public Character Enemy1;
    public Character Enemy2;
    public Character Enemy3;
    public Character Enemy4;

    // Use this for initialization
    void Start ()
    {
        _characterList.Add(Player1);
        _characterList.Add(Player2);
        _characterList.Add(Player3);
        _characterList.Add(Player4);

        _characterList.Add(Enemy1);
        _characterList.Add(Enemy2);
        _characterList.Add(Enemy3);
        _characterList.Add(Enemy4);

        DecideTurnOrder();

	}
	
	// Update is called once per frame
	void Update ()
    {
        
        while(!_characterTurnOrder[_currentCharacter].Alive)
        {
            if(deadPlayers >= 4)
            {
                // Player lose
                SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
            }
            else if(deadEnemies >= 4)
            {
                // Player wins
                SceneManager.LoadScene("GameWin", LoadSceneMode.Single);
            }

            _currentCharacter++;
            if (_currentCharacter >= _characterTurnOrder.Count)
            {
                _currentCharacter = 0;
            }

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

        if (_currentCharacter >= _characterList.Count)
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
                int count = _characterTurnOrder.Count;
                for (int i = 0; i < count; i++)
                {
                    if (p.Dexterity < _characterTurnOrder[i].Dexterity)
                    {
                        _characterTurnOrder.Insert(i, p);
                        added = true;
                        break;
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
            _characterList[_targettingCharacter].gameObject.GetComponent<SpriteRenderer>().color = Color.magenta; 
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (_targettingCharacter < _characterTurnOrder.Count - 1)
                {
                    _characterList[_targettingCharacter].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    _targettingCharacter++;
                    while(!_characterList[_targettingCharacter].Alive)
                    {
                        
                        _targettingCharacter++;
                        if(_targettingCharacter > _characterList.Count - 1)
                        {
                            _targettingCharacter = 4;
                        }
                    }
                    _characterList[_targettingCharacter].gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (_targettingCharacter > 0)
                {
                    _characterList[_targettingCharacter].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    _targettingCharacter--;

                    while (!_characterList[_targettingCharacter].Alive)
                    {

                        _targettingCharacter--;
                        if (_targettingCharacter > _characterList.Count - 1)
                        {
                            _targettingCharacter = 7;
                        }
                    }
                    _characterList[_targettingCharacter].gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                _targettedCharacter = _targettingCharacter;
                _characterList[_targettingCharacter].gameObject.GetComponent<SpriteRenderer>().color = Color.white;

            }

        }
        else
        {
            _targettedCharacter = Random.Range(0, 3);

            while (!_characterList[_targettingCharacter].Alive)
            {

                _targettingCharacter++;
                if (_targettingCharacter > _characterList.Count - 5)
                {
                    _targettingCharacter = 0;
                }
            }
        }
        if (_targettedCharacter > -1)
        {
            _optionChosen = 0;

            _characterList[_targettedCharacter].ChangeHealth(-5);

            if (!_characterList[_targettedCharacter].Alive)
            {
                if (_characterList[_targettedCharacter].Player)
                {
                    deadPlayers++;
                }
                else
                {
                    deadEnemies++;
                }
            }

            _targettingCharacter = 4;
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


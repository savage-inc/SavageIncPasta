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
    private int _currentCharacterIndex = 0;
    private int _targettedCharacterIndex = -1;
    private int _targettingCharacterIndex = 4;


    public Character Player1;
    public Character Player2;
    public Character Player3;
    public Character Player4;
    public Character Enemy1;
    public Character Enemy2;
    public Character Enemy3;
    public Character Enemy4;

    private int _deadEnemies = 0;
    private int _deadPlayers = 0;

    // Use this for initialization
    void Start()
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
    void Update()
    {
        while (!_characterTurnOrder[_currentCharacterIndex].Alive)
        {

            _currentCharacterIndex++;
            if (_currentCharacterIndex >= _characterTurnOrder.Count)
            {
                _currentCharacterIndex = 0;
            }

        }

        if (_optionChosen == 0 && _characterTurnOrder[_currentCharacterIndex].Player)
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
                    Attack(_characterTurnOrder[_currentCharacterIndex]);
                    break;
                case TurnOption.eDEFEND:
                    Defend(_characterTurnOrder[_currentCharacterIndex]);
                    break;
                case TurnOption.eMOVE:
                    Move(_characterTurnOrder[_currentCharacterIndex]);
                    break;
                default:
                    break;
            }
        }

        if (_currentCharacterIndex >= _characterList.Count)
        {
            DecideTurnOrder();
            _currentCharacterIndex = 0;
        }

        if (_deadPlayers >= 4)
        {
            // Player lose
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
        else if (_deadEnemies >= 4)
        {
            // Player wins
            SceneManager.LoadScene("GameWin", LoadSceneMode.Single);
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
            _characterList[_targettingCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (_targettingCharacterIndex < _characterTurnOrder.Count - 1)
                {
                    _characterList[_targettingCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    _targettingCharacterIndex++;
                    while (!_characterList[_targettingCharacterIndex].Alive)
                    {

                        _targettingCharacterIndex++;
                        if (_targettingCharacterIndex > _characterList.Count - 1)
                        {
                            _targettingCharacterIndex = 4;
                        }
                    }
                    _characterList[_targettingCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (_targettingCharacterIndex > 0)
                {
                    _characterList[_targettingCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    _targettingCharacterIndex--;

                    while (!_characterList[_targettingCharacterIndex].Alive)
                    {

                        _targettingCharacterIndex--;
                        if (_targettingCharacterIndex < 0)
                        {
                            _targettingCharacterIndex = 7;
                        }
                    }
                    _characterList[_targettingCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                _targettedCharacterIndex = _targettingCharacterIndex;
                _characterList[_targettingCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.white;

            }

        }
        else
        {
            _targettedCharacterIndex = Random.Range(0, 3);

            while (!_characterList[_targettedCharacterIndex].Alive)
            {
                _targettedCharacterIndex++;
                if (_targettedCharacterIndex > _characterList.Count - 5)
                {
                    _targettedCharacterIndex = 0;
                }
            }
        }
        if (_targettedCharacterIndex > -1)
        {
            _optionChosen = 0;

            _characterList[_targettedCharacterIndex].ChangeHealth(-5);

            if (!_characterList[_targettedCharacterIndex].Alive)
            {
                if (_characterList[_targettedCharacterIndex].Player)
                {
                    _deadPlayers++;
                }
                else
                {
                    _deadEnemies++;
                }
            }

            _targettingCharacterIndex = 4;

            while (!_characterList[_targettingCharacterIndex].Alive)
            {
                _targettingCharacterIndex++;
                if(_targettingCharacterIndex > _characterList.Count - 1)
                {
                    _targettingCharacterIndex = 0;
                }
            }
            _targettedCharacterIndex = -1;
            _currentCharacterIndex++;
        }

    }

    void Defend(Character defender)
    {
        _optionChosen = 0;
        //defend
        _currentCharacterIndex++;
    }

    void Move(Character mover)
    {
        _optionChosen = 0;
        //move
        _currentCharacterIndex++;
    }

    public TurnOption GetOptionChoosing()
    {
        return _optionChoosing;
    }
}


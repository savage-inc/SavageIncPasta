﻿using System.Collections;
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
    private List<Character> _characterList = new List<Character>(); // all players in battle
    private List<Character> _characterTurnOrder = new List<Character>(); // all players sorted into turn order
    private int _currentCharacterIndex = 0;
    private int _targettedCharacterIndex = -1;
    private int _targettingCharacterIndex = 4;

    public Character Enemy1;
    public Character Enemy2;
    public Character Enemy3;
    public Character Enemy4;

    private int _deadEnemies = 0;
    private int _deadPlayers = 0;

    // Use this for initialization
    void Start()
    {
        _characterList = FindObjectOfType<PlayerManager>().Characters;

        _characterList.Add(Enemy1);
        _characterList.Add(Enemy2);
        _characterList.Add(Enemy3);
        _characterList.Add(Enemy4);

        DecideTurnOrder();
        PlaceInColumns();

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

        if(!_characterTurnOrder[_currentCharacterIndex].Player)
        {
            EnemyAttack();
        }

        if (_optionChosen > 0)
        {
            switch (_optionChosen)
            {
                case TurnOption.eATTACK:
                    PlayerAttack();
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

        //_characterTurnOrder[_currentCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;

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

    void PlaceInColumns()
    {
        //previous code before merge

        //foreach (Character p in _characterList)
        //{
        //    if (p.Player)
        //    {
        //        switch (p.CurrCol)
        //        {
        //            case (1):
        //                p.gameObject.GetComponent<Transform>().position = new Vector3(-7f, p.gameObject.GetComponent<Transform>().position.y, p.gameObject.GetComponent<Transform>().position.z);
        //                break;
        //            case (2):
        //                p.gameObject.GetComponent<Transform>().position = new Vector3(-4.5f, p.gameObject.GetComponent<Transform>().position.y, p.gameObject.GetComponent<Transform>().position.z);
        //                break;
        //            case (3):
        //                p.gameObject.GetComponent<Transform>().position = new Vector3(-2f, p.gameObject.GetComponent<Transform>().position.y, p.gameObject.GetComponent<Transform>().position.z);
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        switch (p.CurrCol)
        //        {
        //            case (1):
        //                p.gameObject.GetComponent<Transform>().position = new Vector3(2f, p.gameObject.GetComponent<Transform>().position.y, p.gameObject.GetComponent<Transform>().position.z);
        //                break;
        //            case (2):
        //                p.gameObject.GetComponent<Transform>().position = new Vector3(4.5f, p.gameObject.GetComponent<Transform>().position.y, p.gameObject.GetComponent<Transform>().position.z);
        //                break;
        //            case (3):
        //                p.gameObject.GetComponent<Transform>().position = new Vector3(7f, p.gameObject.GetComponent<Transform>().position.y, p.gameObject.GetComponent<Transform>().position.z);
        //                break;
        //        }
        //    }
        //}
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

    void PlayerAttack()
    {
        //previous code from merge commented. colour change needs to be adjusted for sprites

        //_characterList[_targettingCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_targettingCharacterIndex < _characterTurnOrder.Count - 1)
            {
                //_characterList[_targettingCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                _targettingCharacterIndex++;
                while (!_characterList[_targettingCharacterIndex].Alive)
                {

                    _targettingCharacterIndex++;
                    if (_targettingCharacterIndex > _characterList.Count - 1)
                    {
                        _targettingCharacterIndex = 4;
                    }
                }
                //_characterList[_targettingCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_targettingCharacterIndex > 0)
            {
                //_characterList[_targettingCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                _targettingCharacterIndex--;

                while (!_characterList[_targettingCharacterIndex].Alive)
                {

                    _targettingCharacterIndex--;
                    if (_targettingCharacterIndex < 0)
                    {
                        _targettingCharacterIndex = 7;
                    }
                }
                //_characterList[_targettingCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            _targettedCharacterIndex = _targettingCharacterIndex;
            //_characterList[_targettingCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (_targettedCharacterIndex > -1)
        {
            DealDamage();
        }
    }

    void EnemyAttack()
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
        if (_targettedCharacterIndex > -1)
        {
            DealDamage();
        }
    }

    void DealDamage()
    {
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
            if (_targettingCharacterIndex > _characterList.Count - 1)
            {
                _targettingCharacterIndex = 0;
            }
        }
        EndTurn();
    }

    void Defend(Character defender)
    {
        //defend
        _currentCharacterIndex++;
    }

    void Move(Character mover)
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && mover.CurrCol < 3)
        {
            mover.CurrCol++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && mover.CurrCol > 1)
        {
            mover.CurrCol--;
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            EndTurn();
        }

        PlaceInColumns();
    }

    void EndTurn()
    {
        //_characterList[_currentCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        _optionChosen = TurnOption.eNONE;
        _targettedCharacterIndex = -1;
        _currentCharacterIndex++;
    }

    public void SetTurnOption(int turnOption)
    {
        _optionChosen = (TurnOption)turnOption;
    }
}


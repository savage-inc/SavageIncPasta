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
    private List<Character> _characterList = new List<Character>();
    private List<Character> _enemyList = new List<Character>();
    private List<BattleCharacter> _battleCharacterList = new List<BattleCharacter>(); // all players in battle
    private List<BattleCharacter> _characterTurnOrder = new List<BattleCharacter>(); // all players sorted into turn order
    private int _currentCharacterIndex = 0;
    private int _targettedCharacterIndex = -1;
    private int _targettingCharacterIndex = 4;

    public BattleCharacter Player1;
    public BattleCharacter Player2;
    public BattleCharacter Player3;
    public BattleCharacter Player4;

    public BattleCharacter Enemy1;
    public BattleCharacter Enemy2;
    public BattleCharacter Enemy3;
    public BattleCharacter Enemy4;

    private int _deadEnemies = 0;
    private int _deadPlayers = 0;

    // Use this for initialization
    void Start()
    {
        _characterList = FindObjectOfType<PlayerManager>().Characters;

        Player1.Character = _characterList[0];
        Player2.Character = _characterList[1];
        Player3.Character = _characterList[2];
        Player4.Character = _characterList[3];

        _enemyList = FindObjectOfType<EnemyManager>().EnemyGroups[0].Enemies;

        Enemy1.Character = _enemyList[0];
        Enemy2.Character = _enemyList[1];
        Enemy3.Character = _enemyList[2];
        Enemy4.Character = _enemyList[3];

        _battleCharacterList.Add(Player1);
        _battleCharacterList.Add(Player2);
        _battleCharacterList.Add(Player3);
        _battleCharacterList.Add(Player4);
        _battleCharacterList.Add(Enemy1);
        _battleCharacterList.Add(Enemy2);
        _battleCharacterList.Add(Enemy3);
        _battleCharacterList.Add(Enemy4);

        DecideTurnOrder();
        PlaceInColumns();

    }

    // Update is called once per frame
    void Update()
    {
        if (_currentCharacterIndex >= _battleCharacterList.Count)
        {
            DecideTurnOrder();
            _currentCharacterIndex = 0;
        }

        while (!_characterTurnOrder[_currentCharacterIndex].Character.Alive)
        {
            _currentCharacterIndex++;
            if (_currentCharacterIndex >= _characterTurnOrder.Count)
            {
                _currentCharacterIndex = 0;
            }
        }

        _characterTurnOrder[_currentCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;

        if (!_characterTurnOrder[_currentCharacterIndex].Character.Player)
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
        foreach (BattleCharacter p in _battleCharacterList)
        {
            if (p.Character.Player)
            {
                switch (p.Character.CurrCol)
                {
                    case (1):
                        p.gameObject.GetComponent<Transform>().position = new Vector3(-7f, p.gameObject.GetComponent<Transform>().position.y, p.gameObject.GetComponent<Transform>().position.z);
                        break;
                    case (2):
                        p.gameObject.GetComponent<Transform>().position = new Vector3(-4.5f, p.gameObject.GetComponent<Transform>().position.y, p.gameObject.GetComponent<Transform>().position.z);
                        break;
                    case (3):
                        p.gameObject.GetComponent<Transform>().position = new Vector3(-2f, p.gameObject.GetComponent<Transform>().position.y, p.gameObject.GetComponent<Transform>().position.z);
                        break;
                }
            }
            else
            {
                switch (p.Character.CurrCol)
                {
                    case (1):
                        p.gameObject.GetComponent<Transform>().position = new Vector3(2f, p.gameObject.GetComponent<Transform>().position.y, p.gameObject.GetComponent<Transform>().position.z);
                        break;
                    case (2):
                        p.gameObject.GetComponent<Transform>().position = new Vector3(4.5f, p.gameObject.GetComponent<Transform>().position.y, p.gameObject.GetComponent<Transform>().position.z);
                        break;
                    case (3):
                        p.gameObject.GetComponent<Transform>().position = new Vector3(7f, p.gameObject.GetComponent<Transform>().position.y, p.gameObject.GetComponent<Transform>().position.z);
                        break;
                }
            }
        }
    }

    void DecideTurnOrder()
    {
        _characterTurnOrder.Clear();

        // Decides turn order
        foreach (BattleCharacter p in _battleCharacterList)
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
                    if (p.Character.Dexterity < _characterTurnOrder[i].Character.Dexterity)
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

        _battleCharacterList[_targettingCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_targettingCharacterIndex < _characterTurnOrder.Count - 1)
            {
                _battleCharacterList[_targettingCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                _targettingCharacterIndex++;
                while (!_battleCharacterList[_targettingCharacterIndex].Character.Alive)
                {

                    _targettingCharacterIndex++;
                    if (_targettingCharacterIndex > _battleCharacterList.Count - 1)
                    {
                        _targettingCharacterIndex = 4;
                    }
                }
                _battleCharacterList[_targettingCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_targettingCharacterIndex > 0)
            {
                _battleCharacterList[_targettingCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                _targettingCharacterIndex--;

                while (!_battleCharacterList[_targettingCharacterIndex].Character.Alive)
                {

                    _targettingCharacterIndex--;
                    if (_targettingCharacterIndex < 0)
                    {
                        _targettingCharacterIndex = 7;
                    }
                }
                _battleCharacterList[_targettingCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            _targettedCharacterIndex = _targettingCharacterIndex;
            _battleCharacterList[_targettingCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (_targettedCharacterIndex > -1)
        {
            DealDamage();
        }
    }

    void EnemyAttack()
    {

        _targettedCharacterIndex = Random.Range(0, 3);

        while (!_battleCharacterList[_targettedCharacterIndex].Character.Alive)
        {
            _targettedCharacterIndex++;
            if (_targettedCharacterIndex > _battleCharacterList.Count - 5)
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
        _battleCharacterList[_targettedCharacterIndex].Character.ChangeHealth(-5);

        if (!_battleCharacterList[_targettedCharacterIndex].Character.Alive)
        {
            if (_battleCharacterList[_targettedCharacterIndex].Character.Player)
            {
                _deadPlayers++;
            }
            else
            {
                _deadEnemies++;
            }
        }

        _targettingCharacterIndex = 4;

        while (!_battleCharacterList[_targettingCharacterIndex].Character.Alive)
        {
            _targettingCharacterIndex++;
            if (_targettingCharacterIndex > _battleCharacterList.Count - 1)
            {
                _targettingCharacterIndex = 0;
            }
        }
        EndTurn();
    }

    void Defend(BattleCharacter defender)
    {
        //defend
        _currentCharacterIndex++;
    }

    void Move(BattleCharacter mover)
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && mover.Character.CurrCol < 3)
        {
            mover.Character.CurrCol++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && mover.Character.CurrCol > 1)
        {
            mover.Character.CurrCol--;
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            EndTurn();
        }

        PlaceInColumns();
    }

    void EndTurn()
    {
        _characterTurnOrder[_currentCharacterIndex].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        _optionChosen = TurnOption.eNONE;
        _targettedCharacterIndex = -1;
        _currentCharacterIndex++;
    }

    public void SetTurnOption(int turnOption)
    {
        _optionChosen = (TurnOption)turnOption;
    }
}


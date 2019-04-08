using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum TurnOption
{
    eNONE,
    eATTACK,
    eDEFEND,
    eMOVE,
    eACTION,
    eAbility1,
    eAbility2,
    eAbility3,
    eAbility4,
    eAbility5,
    eAbility6
}



public class Battle : MonoBehaviour
{
    private TurnOption _optionChosen = TurnOption.eNONE;
    private List<Character> _characterList = new List<Character>();
    private List<Character> _enemyList = new List<Character>();
    private List<BattleCharacter> _battleCharacterList = new List<BattleCharacter>(); // all players in battle
    private List<int> _characterTurnOrder = new List<int>();
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

        _enemyList = FindObjectOfType<EnemyManager>().CreateTeamInstance();

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

        while (!_battleCharacterList[_currentCharacterIndex].Character.Alive)
        {
            _currentCharacterIndex++;
            if (_currentCharacterIndex >= _battleCharacterList.Count)
            {
                _currentCharacterIndex = 0;
            }
        }

        if (_battleCharacterList[_currentCharacterIndex].StartTurn)
        {
            StartTurn();
        }

        if (!_battleCharacterList[_currentCharacterIndex].Character.Player)
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
                    Defend(_battleCharacterList[_currentCharacterIndex]);
                    break;
                case TurnOption.eMOVE:
                    Move(_battleCharacterList[_currentCharacterIndex]);
                    break;
                case TurnOption.eACTION:
                    SwitchAction(_battleCharacterList[_currentCharacterIndex]);
                    break;
                case TurnOption.eAbility1:
                    Ability(_battleCharacterList[_currentCharacterIndex].Character.Abilities[0]);
                    break;
                default:
                    break;
            }
        }

        if (_deadPlayers >= _characterList.Count)
        {
            // Player lose
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
        else if (_deadEnemies >= _enemyList.Count)
        {
            float experience = 0;
            // Player wins
            foreach (Character enemy in _enemyList)
            {
                //gold += enemy.GoldDrop; //Party gold needs to be added
                experience += enemy.Experience;
                
            }
            experience /= _characterList.Count;
            foreach (BattleCharacter player in _battleCharacterList)
            {
                if(player.Character.Player)
                {
                    player.Character.Experience += (int)experience;

                   player.Character.Comfort -= player.DamageTaken == 0 ? 1 : player.DamageTaken / 2;
                }
            }
            Vector2 newPos = new Vector2(PlayerPrefs.GetFloat("SceneOriginX"), PlayerPrefs.GetFloat("SceneOriginY"));
            PersistantData.SetPlayerPositionInNextScene(newPos);
            SceneManager.LoadScene(PlayerPrefs.GetInt("SceneOrigin"), LoadSceneMode.Single);
        }
    }

    void StartTurn()
    {
        BattleCharacter currentCharacter = _battleCharacterList[_currentCharacterIndex];
        currentCharacter.gameObject.GetComponent<Image>().color = Color.magenta;
        currentCharacter.Defending = 1.0f;
        currentCharacter.StartTurn = false;
    }

    void SwitchAction(BattleCharacter currentCharacter)
    {
        if (currentCharacter.CurrentAction == ActionChoice.ePrimary && currentCharacter.SecondaryAction == false)
        {
            currentCharacter.CurrentAction = ActionChoice.eSecondary;
        }
        else if (currentCharacter.CurrentAction == ActionChoice.eSecondary && currentCharacter.PrimaryAction == false)
        {
            currentCharacter.CurrentAction = ActionChoice.ePrimary;
        }
        else
        {
            Debug.Log("somehow not a correct action choice.");
        }
        _optionChosen = TurnOption.eNONE;
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
        for (int i = 0; i < _battleCharacterList.Count; i++)
        {
            int rand = Random.Range(-2, 2);
            _battleCharacterList[i].Initiative = _battleCharacterList[i].Character.Dexterity + rand;

            if (_characterTurnOrder.Count == 0)
            {
                _characterTurnOrder.Add(i);
            }
            else
            {
                bool added = false;
                int count = _characterTurnOrder.Count;
                for (int j = 0; j < count; j++)
                {
                    if (_battleCharacterList[i].Initiative < _battleCharacterList[_characterTurnOrder[j]].Initiative)
                    {
                        _characterTurnOrder.Insert(j, i);
                        added = true;
                        break;
                    }

                }
                if (!added)
                {
                    _characterTurnOrder.Add(i);
                }
            }

        }
    }


    void PlayerAttack()
    {
        if (_targettedCharacterIndex > -1)
        {
            DealDamage();
        }
    }

    void Ability(int ability)
    {
        switch (_battleCharacterList[_currentCharacterIndex].Character.Class)
        {
            case ClassType.eWARRIOR:
                //do stuff.
                break;
            case ClassType.eRANGER:
                break;
            case ClassType.eSHAMAN:
                break;
            case ClassType.eWIZARD:
                break;
        }
    }

    public void SetTargettedCharacter(int characterIndex)
    {
        _targettedCharacterIndex = characterIndex;
        SwitchInteractableCharacterButtons();
    }

    public void SwitchInteractableCharacterButtons()
    {
        if (_battleCharacterList[_currentCharacterIndex].Character.Class == ClassType.eWARRIOR)
        {
            int colToAttack = 1;
            bool characterAvailable = false;
            while (!characterAvailable && colToAttack <= 3)
            {
                for (int i = 0; i < _battleCharacterList.Count; i++)
                {
                    if ((_battleCharacterList[i].Character.CurrCol == colToAttack && _battleCharacterList[i].Character.Alive && !_battleCharacterList[i].Character.Player) || _battleCharacterList[i].Character.Player)
                    {
                        _battleCharacterList[i].gameObject.GetComponent<Button>().interactable = !_battleCharacterList[i].gameObject.GetComponent<Button>().interactable;
                        if(!_battleCharacterList[i].Character.Player)
                        {
                            characterAvailable = true;
                        }

                    }
                }
                if (!characterAvailable)
                {
                    colToAttack++;
                }
            }
        }
        else
        {

            for (int i = 0; i < _battleCharacterList.Count; i++)
            {
                if (_battleCharacterList[i].Character.Alive)
                {
                    _battleCharacterList[i].gameObject.GetComponent<Button>().interactable = !_battleCharacterList[i].gameObject.GetComponent<Button>().interactable;
                }

            }
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
        float chanceToHit = 0.5f;
        Character attacker = _battleCharacterList[_currentCharacterIndex].Character;
        Character defender = _battleCharacterList[_targettedCharacterIndex].Character;
        int damage = 0;
        int classModifier = 0;
        switch (attacker.Class)
        {
            case ClassType.eWARRIOR:
                classModifier = attacker.Strength;
                break;
            case ClassType.eRANGER:
                classModifier = attacker.Dexterity;
                break;
            case ClassType.eSHAMAN:
            case ClassType.eWIZARD:
                classModifier = attacker.Intelligence;
                break;
            case ClassType.eENEMY:
                classModifier = attacker.Strength;
                break;
        }
        var weapon = attacker.Equipment.GetEquipedWeapon();
        float magic = 0;

        if (weapon != null)
        {
            magic = weapon.MagicalModifier;
        }

        chanceToHit = (50.0f + classModifier * 10.0f - defender.BaseArmour * 2.0f + magic) / 100.0f;
        chanceToHit = Mathf.Clamp(chanceToHit, 0.05f, 0.95f);

        if (Random.value < chanceToHit)
        {
            if (weapon == null)
            {
                damage = (int)(attacker.BaseAttack * _battleCharacterList[_targettedCharacterIndex].Defending);

            }
            else
            {

                damage = (int)(weapon.BaseDamage + Random.Range(-(int)weapon.VarianceDamage + classModifier / 2, (int)weapon.VarianceDamage + classModifier) * _battleCharacterList[_targettedCharacterIndex].Defending);
            }
            _battleCharacterList[_targettedCharacterIndex].DamageTaken += damage;
            _battleCharacterList[_targettedCharacterIndex].Character.ChangeHealth(-damage);
        }
        else
        {
            //miss stuff
        }


        if (!_battleCharacterList[_targettedCharacterIndex].Character.Alive)
        {
            if (_battleCharacterList[_targettedCharacterIndex].Character.Player)
            {
                _deadPlayers++;
                //remove dead characters from list?
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
        if (defender.Defending < 1.0f)
        {
            defender.Defending = 0.25f;
        }
        else if (defender.CurrentAction == ActionChoice.ePrimary)
        {
            defender.Defending = 0.5f;
        }
        else if (defender.CurrentAction == ActionChoice.eSecondary)
        {
            defender.Defending = 0.75f;
        }
        EndTurn();
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
        BattleCharacter currentCharacter = _battleCharacterList[_currentCharacterIndex];
        
        if(currentCharacter.CurrentAction == ActionChoice.ePrimary)
        {
            currentCharacter.PrimaryAction = true;
            currentCharacter.CurrentAction = ActionChoice.eSecondary;
        }
        else if(currentCharacter.CurrentAction == ActionChoice.eSecondary)
        {
            currentCharacter.SecondaryAction = true;
            currentCharacter.CurrentAction = ActionChoice.ePrimary;
        }
        if (currentCharacter.PrimaryAction == true && currentCharacter.SecondaryAction == true)
        {
            currentCharacter.gameObject.GetComponent<Image>().color = Color.white;
            currentCharacter.StartTurn = true;
            currentCharacter.PrimaryAction = false;
            currentCharacter.SecondaryAction = false;
            _currentCharacterIndex++;
        }
        _optionChosen = TurnOption.eNONE;
        _targettedCharacterIndex = -1;
        
    }

    public void SetTurnOption(int turnOption)
    {
        _optionChosen = (TurnOption)turnOption;
    }
}


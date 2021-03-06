﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    eAbility6,
    eAbility7
}



public class Battle : MonoBehaviour
{
    private TurnOption _optionChosen = TurnOption.eNONE;
    private List<Character> _partyList = new List<Character>();
    private List<Character> _enemyList = new List<Character>();
    private List<BattleCharacter> _battleCharacterList = new List<BattleCharacter>(); // all players in battle
    private List<int> _characterTurnOrder = new List<int>();
    private int _currentCharacterIndex = 0;
    private int _targettedCharacterIndex = -1;
    private int _targettingCharacterIndex = 4;
    public List<Button> abilityButtons = new List<Button>();
    public List<BattleCharacter> Players = new List<BattleCharacter>();
    public List<BattleCharacter> Enemies = new List<BattleCharacter>();
    public List<GameObject> PlayerHealthBars = new List<GameObject>();
    public List<GameObject> EnemyHealthBars = new List<GameObject>();
    public List<GameObject> PlayerParticles = new List<GameObject>();
    public List<GameObject> EnemyParticles = new List<GameObject>();
    public GameObject ArrowPrefab;
    public GameObject SpellPrefab;

    public GameObject FirstSelected;

    private int _deadEnemies = 0;
    private int _deadPlayers = 0;
    private bool _selectingCharacter = false;
    private bool _skipFrame = false; //skip frame when selecting character to clear input
    private int _tempSelectedEnemy = 0;
    private bool _debugMode = false;
    private bool _runningAnimation = false;
    private int _selectedAbility = -1;

    private EventSystem _eventSystem;
    private PlayerManager _playerManager;
    private ClanManager _clanManager;

    private void Awake()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
        _playerManager = FindObjectOfType<PlayerManager>();
        _clanManager = FindObjectOfType<ClanManager>();

        PersistantData.LoadItemDatabase();
        //load party inventory from file
        PersistantData.LoadPartyData(FindObjectOfType<PartyInventory>(), _playerManager, _clanManager);

        if (!_playerManager.IsAlive())
        {
            Debug.LogWarning("Saved party is dead generating a new party (THIS SHOULD NEVER HAPPEN WITHIN THE WORLD)");
            for (int i = 0; i < 4; i++)
            {
                _partyList.Add(GenerateRandomCharacter.GenerateCharacter());
            }
            _debugMode = true;
        }
        else
        {
            //load party
            _partyList = _playerManager.Characters;
        }
    }

    // Use this for initialization
    void Start()
    {

        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].Character != null && i < _partyList.Count && _partyList[i] != null)
            {
                Players[i].Character = _partyList[i];
                _battleCharacterList.Add(Players[i]);
                PlayerHealthBars[i].SetActive(true);
                _battleCharacterList[i].LoadCharacter();

                if (Players[i].Character.Magic != MagicType.eNONE)
                {
                    ParticleSystem pSystem = PlayerParticles[i].GetComponent<ParticleSystem>();
                    PlayerParticles[i].SetActive(true);
                    switch (Players[i].Character.Magic)
                    {
                        case (MagicType.eCHEESE):
                            pSystem.startColor = Color.yellow;
                            break;
                        case (MagicType.ePESTO):
                            pSystem.startColor = Color.green;
                            break;
                        case (MagicType.eTOMATO):
                            pSystem.startColor = Color.red;
                            break;
                    }
                }
            }
        }

        _enemyList = FindObjectOfType<EnemyManager>().CreateTeamInstance();

        for (int i = 0; i < _enemyList.Count; i++)
        {
            if (Enemies[i].Character != null)
            {
                Enemies[i].Character = _enemyList[i];
                _battleCharacterList.Add(Enemies[i]);
                EnemyHealthBars[i].SetActive(true);
                _battleCharacterList[i].LoadCharacter();


                if (Enemies[i].Character.Magic != MagicType.eNONE)
                {
                    ParticleSystem pSystem = EnemyParticles[i].GetComponent<ParticleSystem>();
                    Renderer render = pSystem.GetComponent<Renderer>();
                    render.sortingLayerName = "Particle";

                    EnemyParticles[i].SetActive(true);
                    switch (Enemies[i].Character.Magic)
                    {
                        case (MagicType.eCHEESE):
                            pSystem.startColor = Color.yellow;
                            break;
                        case (MagicType.ePESTO):
                            pSystem.startColor = Color.green;
                            break;
                        case (MagicType.eTOMATO):
                            pSystem.startColor = Color.red;
                            break;
                    }
                }
            }
        }
        _tempSelectedEnemy = Players.Count;
        DecideTurnOrder();
        PlaceInColumns();

        IncreasePartyMana(100);
    }

    //Save the game on destroy (Scene change)
    void OnDestroy()
    {
        PersistantData.SavePartyData(FindObjectOfType<PartyInventory>(), FindObjectOfType<PlayerManager>(), null);
    }

    // Update is called once per frame
    void Update()
    {
        if (_runningAnimation)
        {
            return;
        }
        if (_skipFrame)
        {
            _skipFrame = false;
            return;
        }

        //clear selected player color
        for (int i = 0; i < 4; i++)
        {
            if (i < _battleCharacterList.Count)
            {
                _battleCharacterList[i].GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        if (_currentCharacterIndex >= _battleCharacterList.Count)
        {
            _currentCharacterIndex = 0;
            //DecideTurnOrder();
            //IncreasePartyMana(5);
        }

        //Set selected player to magenta
        _battleCharacterList[_characterTurnOrder[_currentCharacterIndex]].GetComponent<SpriteRenderer>().color = Color.magenta;

        //select enemy
        SelectEnemyState();
        if (_skipFrame)
        {
            _skipFrame = false;
            return;
        }
        SetAbilityButtons();

        while (!_battleCharacterList[_characterTurnOrder[_currentCharacterIndex]].Character.Alive)
        {
            _currentCharacterIndex++;
            if (_currentCharacterIndex >= _battleCharacterList.Count)
            {
                _currentCharacterIndex = 0;
            }
        }

        BattleCharacter currentCharacter = _battleCharacterList[_characterTurnOrder[_currentCharacterIndex]];

        if (currentCharacter.StartTurn)
        {
            StartTurn();
        }

        if (!currentCharacter.Character.Player)
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
                    Defend(currentCharacter);
                    break;
                case TurnOption.eMOVE:
                    Move(currentCharacter);
                    break;
                case TurnOption.eACTION:
                    SwitchAction(currentCharacter);
                    break;
                case TurnOption.eAbility1:
                    Ability(currentCharacter.Character.Abilities[0], currentCharacter);
                    break;
                case TurnOption.eAbility2:
                    Ability(currentCharacter.Character.Abilities[1], currentCharacter);
                    break;
                case TurnOption.eAbility3:
                    Ability(currentCharacter.Character.Abilities[2], currentCharacter);
                    break;
                case TurnOption.eAbility4:
                    Ability(currentCharacter.Character.Abilities[3], currentCharacter);
                    break;
                case TurnOption.eAbility5:
                    Ability(currentCharacter.Character.Abilities[4], currentCharacter);
                    break;
                case TurnOption.eAbility6:
                    Ability(currentCharacter.Character.Abilities[5], currentCharacter);
                    break;
                case TurnOption.eAbility7:
                    Ability(currentCharacter.Character.Abilities[6], currentCharacter);
                    break;
                default:
                    break;
            }
        }

        if (_deadPlayers >= _partyList.Count)
        {
            // Player lose
            FindObjectOfType<BattleInventoryUI>().Save();
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
        else if (_deadEnemies >= _enemyList.Count)
        {
            float experience = 0;
            int gold = 0;
            // Player wins
            foreach (Character enemy in _enemyList)
            {
                FindObjectOfType<PartyInventory>().Gold += enemy.GoldDrop;
                gold += enemy.GoldDrop;
                experience += enemy.Experience;

            }
            experience /= _partyList.Count;
            int playersLeft = 0;
            PlayerPrefs.SetString("Player0", "");
            PlayerPrefs.SetString("Player1", "");
            PlayerPrefs.SetString("Player2", "");
            foreach (BattleCharacter player in _battleCharacterList)
            {
                if (player.Character.Player)
                {
                    player.Character.Experience += (int)experience;

                    player.Character.Comfort -= player.DamageTaken == 0 ? 1 : player.DamageTaken / 2;

                    //check comfort
                    if(player.Character.Comfort <= 0 && player.Character.Alive)
                    {
                        PlayerPrefs.SetString("Player" + playersLeft, player.Character.Name);
                        _playerManager.RemoveCharacter(player.Character);
                        _clanManager.AddCharacter(player.Character);
                        playersLeft++;
                    }
                }
            }
            //only save not in debug mode (Generated characters upon battle load instead of saved party)
            if (!_debugMode)
            {
                FindObjectOfType<BattleInventoryUI>().Save();
            }
            PlayerPrefs.SetInt("Gold", gold);
            PlayerPrefs.SetInt("Experience", (int)experience);
            PlayerPrefs.Save();

            SceneManager.LoadScene("WinBattle", LoadSceneMode.Single);
        }
    }

    public List<BattleCharacter> GetBattleCharacters()
    {
        return _battleCharacterList;
    }

    void StartTurn()
    {
        BattleCharacter currentCharacter = _battleCharacterList[_characterTurnOrder[_currentCharacterIndex]];
        currentCharacter.gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
        currentCharacter.Defending = 1.0f;
        currentCharacter.StartTurn = false;
        currentCharacter.AttackBuffModifier = 1.0f;
        if (currentCharacter.Character.Abilities != null)
        {
            for (int i = 0; i < currentCharacter.Character.Abilities.Count; i++)
            {
                abilityButtons[i].gameObject.GetComponent<Button>().interactable = !abilityButtons[i].gameObject.GetComponent<Button>().interactable;
                //TODO: TURN OFF BUTTONS WHEN NOT ENOUGH MANA
            }
        }
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

    public void RunAway()
    {
        Vector2 newPos = new Vector2(PlayerPrefs.GetFloat("SceneOriginX"), PlayerPrefs.GetFloat("SceneOriginY"));
        PersistantData.SetPlayerPositionInNextScene(newPos);
        //only save not in debug mode (Generated characters upon battle load instead of saved party)
        if (!_debugMode)
        {
            FindObjectOfType<BattleInventoryUI>().Save();
        }
        SceneManager.LoadScene(PlayerPrefs.GetInt("SceneOrigin"), LoadSceneMode.Single);
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


    void PlayerAttack(bool ability = false)
    {
        if (_targettedCharacterIndex > -1)
        {
            if (hitChance())
            {
                DealDamage(CalculateDamage());
            }
            if (!ability)
            {
                EndTurn();
            }
        }
    }

    void WarriorAbility(int abilityNumber, BattleCharacter attacker)
    {
        switch (abilityNumber)
        {
            case 1:
                MacNSnack(attacker.Character);
                break;
            case 2:
                RecklessCharge();
                break;
            case 3:
                ConchiglieShell(attacker);
                break;
            case 4:
                RigatiBoomerang();
                break;
            case 5:
                SpikedBucatini(attacker);
                break;
            case 6:
                PenneStorm(attacker);
                break;
        }
    }

    void MacNSnack(Character attacker)
    {
        attacker.ChangeHealth(5);
        EndTurn();
    }

    void RecklessCharge()
    {
        if (_targettedCharacterIndex > -1)
        {
            if (hitChance())
            {
                DealDamage((int)(CalculateDamage() * 1.5));
            }

            int col = _battleCharacterList[_targettedCharacterIndex].Character.CurrCol;
            for (int i = 0; i < _battleCharacterList.Count; i++)
            {
                if (_battleCharacterList[i].Character.Alive && !_battleCharacterList[i].Character.Player && _battleCharacterList[i].Character.CurrCol <= col)
                {
                    if (hitChance())
                    {
                        _targettedCharacterIndex = _characterTurnOrder[_currentCharacterIndex];
                        DealDamage(3);
                    }
                }
            }
            EndTurn();
        }
    }
    void ConchiglieShell(BattleCharacter attacker)
    {
        Defend(attacker, true);
        Defend(attacker, true);
        EndTurn();
    }
    void RigatiBoomerang()
    {
        if (_targettedCharacterIndex > -1)
        {
            PlayerAttack(true);
            PlayerAttack(true);
            EndTurn();
        }
    }
    void SpikedBucatini(BattleCharacter attacker)
    {
        attacker.SpikedBucatini = true;
        EndTurn();
    }
    void PenneStorm(BattleCharacter attacker)
    {
        int missCount = 0;
        float hitCount = 0;
        if (_targettedCharacterIndex > -1)
        {
            while (missCount < 2)
            {
                if (hitChance(1 - hitCount / 10))
                {
                    hitCount++;
                    DealDamage(CalculateDamage());
                    do
                    {
                        _targettedCharacterIndex = Random.Range(0, _battleCharacterList.Count);
                    } while ((_battleCharacterList[_targettedCharacterIndex].Character.Player || !_battleCharacterList[_targettedCharacterIndex].Character.Alive) && _deadEnemies < _enemyList.Count);
                }
                else
                {
                    missCount++;
                }
            }
            attacker.PrimaryAction = true;
            attacker.SecondaryAction = true;
            EndTurn();
        }
    }

    void RangerAbility(int abilityNumber, BattleCharacter attacker)
    {
        switch (abilityNumber)
        {
            case 1:
                FusiliShot(attacker.Character);
                break;
            case 2:
                FarfalleShot(attacker.Character);
                break;
            case 3:
                LasagneCloak(attacker);
                break;
            case 4:
                StelleStars(attacker.Character);
                break;
            case 5:
                BackStab(attacker.Character);
                break;
            case 6:
                MacaroniFlurry(attacker);
                break;
        }
    }

    void FusiliShot(Character attacker)
    {
        if (_targettedCharacterIndex > -1)
        {
            if (hitChance(1.2f))
            {
                DealDamage(CalculateDamage() + attacker.Level);
            }
            EndTurn();
        }
    }
    void FarfalleShot(Character attacker)
    {
        if (_targettedCharacterIndex > -1)
        {
            if (hitChance())
            {
                DealDamage(CalculateDamage() / 2);
            }
            int col = _battleCharacterList[_targettedCharacterIndex].Character.CurrCol;
            do
            {
                _targettedCharacterIndex = Random.Range(0, _battleCharacterList.Count);
            } while ((_battleCharacterList[_targettedCharacterIndex].Character.CurrCol != col
                  && !_battleCharacterList[_targettedCharacterIndex].Character.Alive)
                  || _battleCharacterList[_targettedCharacterIndex].Character.Player);
            if (hitChance())
            {
                DealDamage(CalculateDamage() / 2);
            }
            EndTurn();
        }
    }
    void LasagneCloak(BattleCharacter attacker)
    {
        attacker.Initiative = (int)(attacker.Initiative * 1.7);
        EndTurn();
    }
    void StelleStars(Character attacker)
    {
        for (int i = 0; i < 3; i++)
        {
            do
            {
                _targettedCharacterIndex = Random.Range(0, _battleCharacterList.Count);
            } while (_battleCharacterList[_targettedCharacterIndex].Character.Player && _battleCharacterList[_targettedCharacterIndex].Character.Alive);
            if (hitChance())
            {
                DealDamage(CalculateDamage() / 2);
            }
        }
        EndTurn();
    }
    void BackStab(Character attacker)
    {
        if (_targettedCharacterIndex > -1)
        {
            if (hitChance(1.2f))
            {
                DealDamage((int)(CalculateDamage() * 1.5));
            }
            EndTurn();
        }
    }
    void MacaroniFlurry(BattleCharacter attacker)
    {
        for (int i = 0; i < 6; i++)
        {
            do
            {
                _targettedCharacterIndex = Random.Range(0, _battleCharacterList.Count);
            } while ((_battleCharacterList[_targettedCharacterIndex].Character.Player || !_battleCharacterList[_targettedCharacterIndex].Character.Alive) && _deadEnemies < _enemyList.Count);
            if (hitChance())
            {
                DealDamage(CalculateDamage());
            }
        }
        attacker.PrimaryAction = true;
        attacker.SecondaryAction = true;
        EndTurn();
    }

    void ShamanAbility(int abilityNumber, BattleCharacter attacker)
    {
        switch (abilityNumber)
        {
            case 1:
                Leftovers(attacker.Character);
                break;
            case 2:
                SpagSteal(attacker.Character);
                break;
            case 3:
                SpiceUp(attacker.Character);
                break;
            case 4:
                PastaSurprise(attacker.Character);
                break;
            case 5:
                SpiceCloud(attacker.Character);
                break;
            case 6:
                SpagHeistti(attacker);
                break;
        }
    }

    public bool HasEnoughMana(Character attacker, int mana)
    {
        if (mana <= attacker.CurrentMana)
        {
            return true;
        }
        else
        {
            _optionChosen = TurnOption.eNONE;
            FindObjectOfType<BattleInfoUI>().TextBox4.text = "Not enough mana!";
            FindObjectOfType<BattleInfoUI>().TextBox4.color = Color.red;

            return false;
        }
    }

    void Leftovers(Character attacker)
    {
        if (HasEnoughMana(attacker, 1))
        {
            if (_targettedCharacterIndex > -1)
            {
                if (hitChance())
                {
                    _battleCharacterList[_targettedCharacterIndex].Character.ChangeHealth(attacker.Intelligence);
                }
                attacker.ChangeMana(-1);
                EndTurn();
            }
        }
    }
    void SpagSteal(Character attacker)
    {
        if (HasEnoughMana(attacker, 3))
        {
            if (_targettedCharacterIndex > -1)
            {
                if (hitChance())
                {
                    int damage = CalculateDamage();
                    DealDamage(damage);
                    attacker.ChangeHealth(damage);
                }
                attacker.ChangeMana(-3);
                EndTurn();
            }
        }
    }
    void SpiceUp(Character attacker)
    {
        if (HasEnoughMana(attacker, 3))
        {
            if (_targettedCharacterIndex > -1)
            {
                _battleCharacterList[_targettedCharacterIndex].AttackBuffModifier += .5f;
                attacker.ChangeMana(-3);
                EndTurn();
            }
        }
    }
    void PastaSurprise(Character attacker)
    {
        if (HasEnoughMana(attacker, 5))
        {
            for (int i = 0; i < 2; i++)
            {
                do
                {
                    _targettedCharacterIndex = Random.Range(0, _battleCharacterList.Count);
                } while (!_battleCharacterList[_targettedCharacterIndex].Character.Player || !_battleCharacterList[_targettedCharacterIndex].Character.Alive);

                _battleCharacterList[_targettedCharacterIndex].Character.ChangeHealth(Random.Range(5, attacker.Intelligence * 2));
            }
            attacker.ChangeMana(-5);
            EndTurn();
        }
    }
    void SpiceCloud(Character attacker)
    {
        if (HasEnoughMana(attacker, 5))
        {
            foreach (BattleCharacter character in _battleCharacterList)
            {
                if (character.Character.Player)
                {
                    character.AttackBuffModifier += .5f;
                }
            }
            attacker.ChangeMana(-5);
            EndTurn();
        }
    }
    void SpagHeistti(BattleCharacter attacker)
    {
        if (HasEnoughMana(attacker.Character, 10))
        {
            int totalDamage = 0;
            int noOfPlayers = 0;
            for (int i = 0; i < _battleCharacterList.Count; i++)
            {
                if (!_battleCharacterList[_characterTurnOrder[i]].Character.Player && _battleCharacterList[_characterTurnOrder[i]].Character.Alive)
                {
                    _targettedCharacterIndex = _characterTurnOrder[i];
                    if (hitChance())
                    {
                        int damage = CalculateDamage();
                        DealDamage(damage);
                        totalDamage += damage;
                    }
                }
                else
                {
                    noOfPlayers++;
                }
            }

            for (int i = 0; i < _battleCharacterList.Count; i++)
            {
                if (_battleCharacterList[_characterTurnOrder[i]].Character.Player)
                {
                    _battleCharacterList[_characterTurnOrder[i]].Character.ChangeHealth(totalDamage / noOfPlayers);
                }
            }
            attacker.Character.ChangeMana(-10);
            attacker.PrimaryAction = true;
            attacker.SecondaryAction = true;
            EndTurn();
        }
    }

    void WizardAbility(int abilityNumber, BattleCharacter attacker)
    {
        switch (abilityNumber)
        {
            case 1:
                ScaldingSauce(attacker.Character);
                break;
            case 2:
                Meatball(attacker.Character);
                break;
            case 3:
                Slow(attacker.Character);
                break;
            case 4:
                Teleport(attacker.Character);
                break;
            case 5:
                FlourAttack(attacker.Character);
                break;
            case 6:
                RavioliBomb(attacker.Character);
                break;
            case 7:
                SpaghettiWhip(attacker);
                break;
        }
    }
    void ScaldingSauce(Character attacker)
    {
        if (HasEnoughMana(attacker, 1))
        {
            if (_targettedCharacterIndex > -1)
            {
                if (hitChance())
                {
                    DealDamage((int)(attacker.Intelligence + 1.5 * attacker.Level));
                }
                attacker.ChangeMana(-1);
                EndTurn();
            }
        }
    }
    void Meatball(Character attacker)
    {
        if (HasEnoughMana(attacker, 3))
        {
            if (_targettedCharacterIndex > -1)
            {
                if (hitChance())
                {
                    DealDamage(attacker.Intelligence);

                    //get alive enemies
                    var aliveEnemies = GetAliveEnemies();
                    if (aliveEnemies.Count > 1)
                    {
                        int firstEnemyIndex = _targettedCharacterIndex;

                        foreach (var aliveEnemy in aliveEnemies)
                        {
                            if (aliveEnemy != _battleCharacterList[firstEnemyIndex])
                            {
                                _targettedCharacterIndex = GetIndexOfCharacter(aliveEnemy);
                                DealDamage(attacker.Intelligence);
                                break;
                            }
                        }
                    }
                }
                attacker.ChangeMana(-3);
                EndTurn();
            }
        }
    }
    void Slow(Character attacker)
    {
        if (HasEnoughMana(attacker, 3))
        {
            if (_targettedCharacterIndex > -1)
            {
                if (hitChance())
                {
                    _battleCharacterList[_targettedCharacterIndex].SecondaryAction = true;
                }
                attacker.ChangeMana(-3);
                EndTurn();
            }
        }
    }
    void Teleport(Character attacker)
    {
        if (HasEnoughMana(attacker, 3))
        {
            if (_targettedCharacterIndex > -1)
            {
                if (Move(_battleCharacterList[_targettedCharacterIndex]))
                {
                    attacker.ChangeMana(-3);
                }
            }
        }
    }
    void FlourAttack(Character attacker)
    {
        if (HasEnoughMana(attacker, 5))
        {
            if (_targettedCharacterIndex > -1)
            {
                int col = _battleCharacterList[_targettedCharacterIndex].Character.CurrCol;
                for (int i = 0; i < _battleCharacterList.Count; i++)
                {
                    if (_battleCharacterList[i].Character.Alive && !_battleCharacterList[i].Character.Player && _battleCharacterList[i].Character.CurrCol == col)
                    {
                        if (hitChance())
                        {
                            _battleCharacterList[i].ChanceToHitModifier -= 5 * attacker.Intelligence;
                        }
                    }
                }
                attacker.ChangeMana(-5);
                EndTurn();
            }
        }
    }
    void RavioliBomb(Character attacker)
    {
        if (HasEnoughMana(attacker, 5))
        {
            if (_targettedCharacterIndex > -1)
            {
                int col = _battleCharacterList[_targettedCharacterIndex].Character.CurrCol;
                for (int i = 0; i < _battleCharacterList.Count; i++)
                {
                    if (_battleCharacterList[i].Character.Alive && !_battleCharacterList[i].Character.Player && _battleCharacterList[i].Character.CurrCol == col)
                    {
                        if (hitChance())
                        {
                            _targettedCharacterIndex = i;
                            DealDamage(attacker.Intelligence * Random.Range(1, 3));
                        }
                    }
                }
                attacker.ChangeMana(-5);
                EndTurn();
            }
        }
    }
    void SpaghettiWhip(BattleCharacter attacker)
    {
        if (HasEnoughMana(attacker.Character, 10))
        {
            for (int i = 0; i < _battleCharacterList.Count; i++)
            {
                if (_battleCharacterList[i].Character.Alive && !_battleCharacterList[i].Character.Player)
                {
                    _targettedCharacterIndex = i;
                    DealDamage((int)(attacker.Character.Intelligence * (1.5 + attacker.Character.Level)));
                }
            }
            attacker.Character.ChangeMana(-10);
            attacker.PrimaryAction = true;
            attacker.SecondaryAction = true;
            EndTurn();
        }
    }


    void Ability(int ability, BattleCharacter attacker)
    {
        switch (attacker.Character.Class)
        {
            case ClassType.eWARRIOR:
                WarriorAbility(ability, attacker);
                break;
            case ClassType.eRANGER:
                RangerAbility(ability, attacker);
                break;
            case ClassType.eSHAMAN:
                ShamanAbility(ability, attacker);
                break;
            case ClassType.eWIZARD:
                WizardAbility(ability, attacker);
                break;
        }
    }

    public void SetTargettedCharacter(int characterIndex)
    {
        //check if the character is alive
        if (!_battleCharacterList[characterIndex].Character.Alive)
        {
            return;
        }

        _targettedCharacterIndex = characterIndex;
        _selectingCharacter = false;
    }



    void EnemyAttack()
    {
        //check if party is alive
        if (!partyAlive())
        {
            EndTurn();
            return;
        }

        //play animation


        _targettedCharacterIndex = Random.Range(0, _battleCharacterList.Count);

        while (!_battleCharacterList[_targettedCharacterIndex].Character.Alive || !_battleCharacterList[_targettedCharacterIndex].Character.Player)
        {
            _targettedCharacterIndex = Random.Range(0, _battleCharacterList.Count);
        }
        if (_targettedCharacterIndex > -1)
        {
            if (hitChance())
            {
                DealDamage(CalculateDamage());

                if (_battleCharacterList[_targettedCharacterIndex].SpikedBucatini)
                {
                    int currChar = _characterTurnOrder[_currentCharacterIndex];
                    _characterTurnOrder[_currentCharacterIndex] = _targettedCharacterIndex;
                    _targettedCharacterIndex = _characterTurnOrder[_currentCharacterIndex];
                    if (hitChance())
                    {
                        DealDamage(CalculateDamage());
                    }
                    _characterTurnOrder[_currentCharacterIndex] = currChar;
                }

            }
        }
        EndTurn();
    }

    int CalculateDamage()
    {
        BattleCharacter attacker = _battleCharacterList[_characterTurnOrder[_currentCharacterIndex]];
        BattleCharacter defender = _battleCharacterList[_targettedCharacterIndex];
        int damage = 0;

        var weapon = attacker.Character.Equipment.GetEquippedWeapon();
        var weaponDamage = attacker.Character.Equipment.GetEquippedWeaponDamage();
        var weaponVarDamage = attacker.Character.Equipment.GetEquippedWeaponVarDamage();
        var Armour = defender.Character.Equipment.GetArmourMagicType();
            
        float magicEffect = 1.0f;

        if (weapon != null)
        {            
            if (weapon.MagicalType == MagicType.eCHEESE)
            {
                if (Armour == MagicType.ePESTO || defender.Character.Magic == MagicType.ePESTO)
                {
                    magicEffect = 2.0f;
                }
                else if (Armour == MagicType.eTOMATO || defender.Character.Magic == MagicType.eTOMATO)
                {
                    magicEffect = 0.5f;
                }
            }
            else if (weapon.MagicalType == MagicType.ePESTO)
            {
                if (Armour == MagicType.eTOMATO || defender.Character.Magic == MagicType.eTOMATO)
                {
                    magicEffect = 2.0f;
                }
                else if (Armour == MagicType.eCHEESE || defender.Character.Magic == MagicType.eCHEESE)
                {
                    magicEffect = 0.5f;
                }
            }
            else if (weapon.MagicalType == MagicType.eTOMATO)
            {
                if (Armour == MagicType.eCHEESE || defender.Character.Magic == MagicType.eCHEESE)
                {
                    magicEffect = 2.0f;
                }
                else if (Armour == MagicType.ePESTO || defender.Character.Magic == MagicType.ePESTO)
                {
                    magicEffect = 0.5f;
                }
            }
        }
        else
        {
            if (attacker.Character.Magic == MagicType.eCHEESE)
            {
                if (Armour == MagicType.ePESTO || defender.Character.Magic == MagicType.ePESTO)
                {
                    magicEffect = 2.0f;
                }
                else if (Armour == MagicType.eTOMATO || defender.Character.Magic == MagicType.eTOMATO)
                {
                    magicEffect = 0.5f;
                }
            }
            else if (attacker.Character.Magic == MagicType.ePESTO)
            {
                if (Armour == MagicType.eTOMATO || defender.Character.Magic == MagicType.eTOMATO)
                {
                    magicEffect = 2.0f;
                }
                else if (Armour == MagicType.eCHEESE || defender.Character.Magic == MagicType.eCHEESE)
                {
                    magicEffect = 0.5f;
                }
            }
            else if (attacker.Character.Magic == MagicType.eTOMATO)
            {
                if (Armour == MagicType.eCHEESE || defender.Character.Magic == MagicType.eCHEESE)
                {
                    magicEffect = 2.0f;
                }
                else if (Armour == MagicType.ePESTO || defender.Character.Magic == MagicType.ePESTO)
                {
                    magicEffect = 0.5f;
                }
            }
        }

        if (weapon == null)
        {

            return damage = (int)(attacker.Character.BaseAttack * _battleCharacterList[_targettedCharacterIndex].Defending * magicEffect);

        }
        else
        {
            return damage = (int)(((weaponDamage + Random.Range(-weaponVarDamage + attacker.ClassModifier / 2, weaponVarDamage + attacker.ClassModifier)) * _battleCharacterList[_targettedCharacterIndex].Defending) * magicEffect);
        }
    }

    bool hitChance(float hitModifier = 1)
    {
        float chanceToHit = 0.5f;
        BattleCharacter attacker = _battleCharacterList[_characterTurnOrder[_currentCharacterIndex]];
        Character defender = _battleCharacterList[_characterTurnOrder[_targettedCharacterIndex]].Character;
        var weapon = attacker.Character.Equipment.GetEquippedWeapon();
        float magic = 0;

        if (weapon != null)
        {
            magic = weapon.MagicalModifier;
        }

        chanceToHit = (((30.0f + attacker.ClassModifier * 10.0f - defender.BaseArmour * 2.0f + magic) / 100.0f) - attacker.ChanceToHitModifier) * hitModifier;
        if (attacker.CurrentAction == ActionChoice.eSecondary)
        {
            chanceToHit *= 0.75f;
        }
        chanceToHit = Mathf.Clamp(chanceToHit, 0.05f, 0.95f);

        if (Random.value < chanceToHit)
        {
            return true;
        }

        //Play miss animation
        StartCoroutine(_battleCharacterList[_characterTurnOrder[_currentCharacterIndex]].MissAnimation());

        return false;
    }

    void DealDamage(int damage)
    {
        StartCoroutine(dealDamageCoroutine(damage));
    }

    IEnumerator dealDamageCoroutine(int damage)
    {
        damage = (int)(damage * _battleCharacterList[_characterTurnOrder[_currentCharacterIndex]].AttackBuffModifier);

        int targetIndex = _targettedCharacterIndex;
        int currentCharacter = _characterTurnOrder[_currentCharacterIndex];

        //get ability used if any
        AbilityData ability = null;
        if (_selectedAbility >= 0)
        {
            ability = AbilityManager.Instance.GetAbility(_battleCharacterList[currentCharacter].Character.Class, _selectedAbility);
        }

        //play animations
        //ability animations
        if (ability != null && ability.Projectile != null)
        {
            _runningAnimation = true;
            //fire projectile animation
            Vector2 target = _battleCharacterList[targetIndex].transform.position;

            yield return StartCoroutine(_battleCharacterList[currentCharacter].FireProjectile(ability.Projectile, target, 1.0f));
            _battleCharacterList[targetIndex].DamageTaken += damage;
            _battleCharacterList[targetIndex].Character.ChangeHealth(-damage);
            _runningAnimation = false;
        }
        else if (_battleCharacterList[currentCharacter].Character.Class == ClassType.eRANGER)
        {
            _runningAnimation = true;
            //fire projectile animation
            Vector2 target = _battleCharacterList[targetIndex].transform.position;

            yield return StartCoroutine(_battleCharacterList[currentCharacter].FireProjectile(ArrowPrefab, target, 1.0f));
            _battleCharacterList[targetIndex].DamageTaken += damage;
            _battleCharacterList[targetIndex].Character.ChangeHealth(-damage);
            _runningAnimation = false;
        }
        else if (_battleCharacterList[currentCharacter].Character.Class == ClassType.eWIZARD)
        {
            _runningAnimation = true;
            //fire projectile animation
            Vector2 target = _battleCharacterList[targetIndex].transform.position;

            yield return StartCoroutine(_battleCharacterList[currentCharacter].FireProjectile(SpellPrefab, target, 1.0f));
            _battleCharacterList[targetIndex].DamageTaken += damage;
            _battleCharacterList[targetIndex].Character.ChangeHealth(-damage);
            _runningAnimation = false;
        }
        else
        {
            //mellee animation
            Vector2 target = _battleCharacterList[targetIndex].transform.position;
            Vector2 start = _battleCharacterList[currentCharacter].transform.position;
            _runningAnimation = true;
            yield return _battleCharacterList[currentCharacter].MoveToAnimation(target, 1.0f);

            _battleCharacterList[targetIndex].DamageTaken += damage;
            _battleCharacterList[targetIndex].Character.ChangeHealth(-damage);

            //return back to start pos
            yield return _battleCharacterList[currentCharacter].MoveToAnimation(start, 1.0f);
            _runningAnimation = false;
        }

        if (!_battleCharacterList[targetIndex].Character.Alive)
        {
            if (_battleCharacterList[targetIndex].Character.Player)
            {
                _deadPlayers++;
                //remove dead characters from list?
            }
            else
            {
                _deadEnemies++;
            }
        }

        _targettingCharacterIndex = 0;

        while (!_battleCharacterList[_targettingCharacterIndex].Character.Alive)
        {
            _targettingCharacterIndex++;
            if (_targettingCharacterIndex > _battleCharacterList.Count - 1)
            {
                _targettingCharacterIndex = 0;
            }
        }

    }

    void Defend(BattleCharacter defender, bool ability = false)
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
        if (!ability)
        {
            EndTurn();
        }
    }

    bool Move(BattleCharacter mover)
    {
        _eventSystem.SetSelectedGameObject(null, null);

        //move left
        if (Input.GetAxis("Horizontal") > 0.0f)
        {
            if (_axisInUse == false)
            {
                mover.Character.CurrCol = Mathf.Clamp(mover.Character.CurrCol + 1, 1, 3);
                _axisInUse = true;
            }
        }
        else if (Input.GetAxis("Horizontal") == 0.0f)
        {
            _axisInUse = false;
        }


        if (Input.GetAxis("Horizontal") < 0.0f)
        {
            if (_axisInUse == false)
            {
                mover.Character.CurrCol = Mathf.Clamp(mover.Character.CurrCol - 1, 1, 3);
                _axisInUse = true;
            }
        }
        else if (Input.GetAxis("Horizontal") == 0.0f)
        {
            _axisInUse = false;
        }


        if (Input.GetButtonDown("A"))
        {
            EndTurn();
            return true;
        }

        PlaceInColumns();
        return false;
    }

    void EndTurn()
    {
        BattleCharacter currentCharacter = _battleCharacterList[_characterTurnOrder[_currentCharacterIndex]];

        if (currentCharacter.CurrentAction == ActionChoice.ePrimary)
        {
            currentCharacter.PrimaryAction = true;
            currentCharacter.CurrentAction = ActionChoice.eSecondary;
        }
        else if (currentCharacter.CurrentAction == ActionChoice.eSecondary)
        {
            currentCharacter.SecondaryAction = true;
            currentCharacter.CurrentAction = ActionChoice.ePrimary;
        }
        if (currentCharacter.PrimaryAction == true && currentCharacter.SecondaryAction == true)
        {
            currentCharacter.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            currentCharacter.StartTurn = true;
            currentCharacter.PrimaryAction = false;
            currentCharacter.SecondaryAction = false;
            if (currentCharacter.Character.Abilities != null)
            {
                for (int i = 0; i < currentCharacter.Character.Abilities.Count; i++)
                {
                    abilityButtons[i].gameObject.GetComponent<Button>().interactable = !abilityButtons[i].gameObject.GetComponent<Button>().interactable;
                }
            }
            _currentCharacterIndex++;
        }
        _optionChosen = TurnOption.eNONE;
        _targettedCharacterIndex = -1;

        _eventSystem.SetSelectedGameObject(null, null);
        _eventSystem.SetSelectedGameObject(FirstSelected);

    }

    public void SetTurnOption(int turnOption)
    {
        _selectedAbility = -1;
        _skipFrame = true;
        _optionChosen = (TurnOption)turnOption;

        if (_optionChosen != TurnOption.eNONE && _optionChosen != TurnOption.eDEFEND && _optionChosen != TurnOption.eACTION && _optionChosen != TurnOption.eMOVE)
        {
            if (_optionChosen == TurnOption.eATTACK)
            {
                _selectingCharacter = true;
            }
            else
            {
                //option must be an ability, we need to check if the ability selected requires a targer
                BattleCharacter currentCharacter = _battleCharacterList[_characterTurnOrder[_currentCharacterIndex]];
                int abilityID = 1;
                switch (_optionChosen)
                {
                    case TurnOption.eAbility1:
                        abilityID = currentCharacter.Character.Abilities[0];
                        break;
                    case TurnOption.eAbility2:
                        abilityID = currentCharacter.Character.Abilities[1]; ;
                        break;
                    case TurnOption.eAbility3:
                        abilityID = currentCharacter.Character.Abilities[2]; ;
                        break;
                    case TurnOption.eAbility4:
                        abilityID = currentCharacter.Character.Abilities[3]; ;
                        break;
                    case TurnOption.eAbility5:
                        abilityID = currentCharacter.Character.Abilities[4]; ;
                        break;
                    case TurnOption.eAbility6:
                        abilityID = currentCharacter.Character.Abilities[5]; ;
                        break;
                    case TurnOption.eAbility7:
                        abilityID = currentCharacter.Character.Abilities[6]; ;
                        break;
                }
                _selectedAbility = abilityID;
                //If the current players selected ability requires a target then enter select character state
                var ability = AbilityManager.Instance.GetAbility(GetCurrentPlayer().Character.Class, abilityID);
                if (ability.RequiresTarget && HasEnoughMana(currentCharacter.Character, ability.ManaCost))
                {
                    _selectingCharacter = true;
                }
            }
        }
    }

    public List<BattleCharacter> GetEnemiesInCol(int col)
    {
        List<BattleCharacter> enemies = new List<BattleCharacter>();

        for (int i = 0; i < _battleCharacterList.Count; i++)
        {
            if ((_battleCharacterList[i].Character.CurrCol == col && _battleCharacterList[i].Character.Alive && !_battleCharacterList[i].Character.Player))
            {
                enemies.Add(_battleCharacterList[i]);
            }
        }

        return enemies;
    }

    public List<BattleCharacter> GetSelectableEnemiesFromPlayer(BattleCharacter player)
    {
        List<BattleCharacter> enemies = new List<BattleCharacter>();

        if (player.Character.Class == ClassType.eWARRIOR && _selectedAbility != 2)
        {
            int colToAttack = 1;
            bool enemyAvailable = false;
            while (!enemyAvailable && colToAttack <= 3)
            {
                for (int i = 0; i < _battleCharacterList.Count; i++)
                {
                    if ((_battleCharacterList[i].Character.CurrCol == colToAttack && _battleCharacterList[i].Character.Alive && !_battleCharacterList[i].Character.Player || _battleCharacterList[i].Character.Player))
                    {
                        if (!_battleCharacterList[i].Character.Player)
                        {
                            enemyAvailable = true;
                        }
                        enemies.Add(_battleCharacterList[i]);
                    }
                }
                if (!enemyAvailable)
                {
                    colToAttack++;
                }
            }
        }
        else
        {
            //player can attack all as it is not a warrior
            for (int i = 0; i < _battleCharacterList.Count; i++)
            {
                if (_battleCharacterList[i].Character.Alive)
                {
                    enemies.Add(_battleCharacterList[i]);
                }
            }
        }

        return enemies;
    }

    public BattleCharacter GetFirstAliveEnemy()
    {
        foreach (var enemy in Enemies)
        {
            if (enemy != null && enemy.Character.Alive)
            {
                return enemy;
            }
        }
        return Enemies[0];
    }

    bool _axisInUse = false;
    void SelectEnemyState()
    {
        if (_targettedCharacterIndex > -1 || !_selectingCharacter)
        {
            if (!_selectingCharacter)
            {
                for (int i = _partyList.Count; i < _battleCharacterList.Count; i++)
                {
                    _battleCharacterList[i].GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
            return;
        }


        var enemies = GetSelectableEnemiesFromPlayer(_battleCharacterList[_characterTurnOrder[_currentCharacterIndex]]);

        for (int i = _partyList.Count; i < _battleCharacterList.Count; i++)
        {
            _battleCharacterList[i].GetComponent<SpriteRenderer>().color = Color.white;
        }


        if (Input.GetButtonDown("A"))
        {
            _skipFrame = true;
            SetTargettedCharacter(GetIndexOfCharacter(enemies[_tempSelectedEnemy]));
        }

        //move up
        if (Input.GetAxis("Vertical") < 0.000f)
        {
            if (_axisInUse == false)
            {
                _tempSelectedEnemy = Mathf.Clamp(_tempSelectedEnemy + 1, 0, enemies.Count - 1);
                _axisInUse = true;
            }
        }
        else if (Input.GetAxis("Vertical") == 0.0f)
        {
            _axisInUse = false;
        }

        //move down
        if (Input.GetAxis("Vertical") > 0.0f)
        {
            if (_axisInUse == false)
            {
                _tempSelectedEnemy = Mathf.Clamp(_tempSelectedEnemy - 1, 0, enemies.Count - 1);
                _axisInUse = true;
            }
        }
        else if (Input.GetAxis("Vertical") == 0.0f)
        {
            _axisInUse = false;
        }
        FindObjectOfType<EventSystem>().enabled = true;

        _battleCharacterList[GetIndexOfCharacter(enemies[Mathf.Clamp(_tempSelectedEnemy, 0, enemies.Count - 1)])].GetComponent<SpriteRenderer>().color = Color.red;
    }

    public BattleCharacter GetCurrentPlayer()
    {
        return _battleCharacterList[_characterTurnOrder[_currentCharacterIndex]];
    }

    bool partyAlive()
    {
        foreach (var character in _partyList)
        {
            if (character.Alive)
            {
                return true;
            }
        }
        return false;
    }

    int GetIndexOfCharacter(BattleCharacter character)
    {
        for (int i = 0; i < _battleCharacterList.Count; i++)
        {
            if (_battleCharacterList[i] == character)
            {
                return i;
            }
        }
        return 0;
    }
    List<BattleCharacter> GetAliveEnemies()
    {
        List<BattleCharacter> enemies = new List<BattleCharacter>();
        foreach (var enemy in Enemies)
        {
            if (enemy != null && enemy.Character.Alive)
            {
                enemies.Add(enemy);
            }
        }
        return enemies;
    }

    IEnumerator skipFrame()
    {
        yield return new WaitForSeconds(1);
    }

    void IncreasePartyMana(int amount)
    {
        foreach (var player in Players)
        {
            if (player != null && player.Character.Alive)
            {
                player.Character.ChangeMana(amount);
            }
        }
    }

    void SetAbilityButtons()
    {
        var player = _battleCharacterList[_characterTurnOrder[_currentCharacterIndex]];
        if (player != null && player.Character.Player)
        {
            //get abiliies
            for (int j = 0; j < player.Character.Abilities.Count; j++)
            {
                abilityButtons[j].GetComponent<AbilltyButton>().Ability = AbilityManager.Instance.GetAbility(player.Character.Class, player.Character.Abilities[j]);
            }
        }
    }
}


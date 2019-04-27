using System.Collections;
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
    private List<Character> _characterList = new List<Character>();
    private List<Character> _enemyList = new List<Character>();
    private List<BattleCharacter> _battleCharacterList = new List<BattleCharacter>(); // all players in battle
    private List<int> _characterTurnOrder = new List<int>();
    private int _currentCharacterIndex = 0;
    private int _targettedCharacterIndex = -1;
    private int _targettingCharacterIndex = 4;
    public List<Button> abilitiyButtons = new List<Button>();

    public BattleCharacter Player1;
    public BattleCharacter Player2;
    public BattleCharacter Player3;
    public BattleCharacter Player4;

    public BattleCharacter Enemy1;
    public BattleCharacter Enemy2;
    public BattleCharacter Enemy3;
    public BattleCharacter Enemy4;
    public GameObject FirstSelected;

    private int _deadEnemies = 0;
    private int _deadPlayers = 0;
    private bool _selectingCharacter = false;
    private bool _skipFrame = false; //skip frame when selecting character to clear input

    private EventSystem _eventSystem;

    private void Awake()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
    }

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

    //Save the game on destroy (Scene change)
    void OnDestroy()
    {
        PersistantData.SavePartyData(FindObjectOfType<PartyInventory>(), FindObjectOfType<PlayerManager>());
    }

    // Update is called once per frame
    void Update()
    {
        if (_skipFrame)
        {
            _skipFrame = false;
            return;
        }

        //select enemy
        SelectEnemyState();
        //TODO SKIP FRAME WHEN BUTTON PRESSED

        if (_currentCharacterIndex >= _battleCharacterList.Count)
        {
            DecideTurnOrder();
            _currentCharacterIndex = 0;
        }


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

        if (_deadPlayers >= _characterList.Count)
        {
            // Player lose
            FindObjectOfType<BattleInventoryUI>().Save();
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
        else if (_deadEnemies >= _enemyList.Count)
        {
            float experience = 0;
            // Player wins
            foreach (Character enemy in _enemyList)
            {
                //gold += enemy.GoldDrop; //Party gold needs to be added TIERAN - GIT GUD
                experience += enemy.Experience;

            }
            experience /= _characterList.Count;
            foreach (BattleCharacter player in _battleCharacterList)
            {
                if (player.Character.Player)
                {
                    player.Character.Experience += (int)experience;

                    player.Character.Comfort -= player.DamageTaken == 0 ? 1 : player.DamageTaken / 2;
                }
            }
            Vector2 newPos = new Vector2(PlayerPrefs.GetFloat("SceneOriginX"), PlayerPrefs.GetFloat("SceneOriginY"));
            PersistantData.SetPlayerPositionInNextScene(newPos);
            FindObjectOfType<BattleInventoryUI>().Save();
            SceneManager.LoadScene(PlayerPrefs.GetInt("SceneOrigin"), LoadSceneMode.Single);
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
                abilitiyButtons[i].gameObject.GetComponent<Button>().interactable = !abilitiyButtons[i].gameObject.GetComponent<Button>().interactable;
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
        if(_targettedCharacterIndex > -1)
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
                SpiceUp();
                break;
            case 4:
                PastaSurprise(attacker.Character);
                break;
            case 5:
                SpiceCloud();
                break;
            case 6:
                SpagHeistti(attacker.Character);
                break;
        }
    }

    void Leftovers(Character attacker)
    {
        if (_targettedCharacterIndex > -1)
        {
            if (hitChance())
            {
                _battleCharacterList[_targettedCharacterIndex].Character.ChangeHealth(attacker.Intelligence);
            }
            EndTurn();
        }
    }
    void SpagSteal(Character attacker)
    {
        if (_targettedCharacterIndex > -1)
        {
            if (hitChance())
            {
                int damage = CalculateDamage();
                DealDamage(damage);
                attacker.ChangeHealth(damage);
            }
            EndTurn();
        }
    }
    void SpiceUp()
    {
        if (_targettedCharacterIndex > -1)
        {
            _battleCharacterList[_targettedCharacterIndex].AttackBuffModifier += .5f;
            EndTurn();
        }
    }
    void PastaSurprise(Character attacker)
    {
        for (int i = 0; i < 2; i++)
        {
            do
            {
                _targettedCharacterIndex = Random.Range(0, _battleCharacterList.Count);
            } while (!_battleCharacterList[_targettedCharacterIndex].Character.Player || !_battleCharacterList[_targettedCharacterIndex].Character.Alive);

            _battleCharacterList[_targettedCharacterIndex].Character.ChangeHealth(Random.Range(5, attacker.Intelligence * 2));
        }
        EndTurn();
    }
    void SpiceCloud()
    {
        foreach (BattleCharacter character in _battleCharacterList)
        {
            if (character.Character.Player)
            {
                character.AttackBuffModifier += .5f;
            }
        }
        EndTurn();
    }
    void SpagHeistti(Character attacker)
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
        EndTurn();
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
        if (_targettedCharacterIndex > -1)
        {
            if (hitChance())
            {
                DealDamage((int)(attacker.Intelligence + 1.5 * attacker.Level));
            }
            attacker.Mana -= 1;
            EndTurn();
        }
    }
    void Meatball(Character attacker)
    {
        if (_targettedCharacterIndex > -1)
        {
            if (hitChance())
            {
                DealDamage(attacker.Intelligence);

                do
                {
                    _targettedCharacterIndex = Random.Range(0, _battleCharacterList.Count);
                } while (_battleCharacterList[_targettedCharacterIndex].Character.Player);

                if (hitChance())
                {
                    DealDamage(attacker.Intelligence);
                }
            }
            attacker.Mana -= 3;
            EndTurn();
        }
    }
    void Slow(Character attacker)
    {
        if (_targettedCharacterIndex > -1)
        {
            if (hitChance())
            {
                _battleCharacterList[_targettedCharacterIndex].SecondaryAction = true;
            }
            attacker.Mana -= 3;
            EndTurn();
        }
    }
    void Teleport(Character attacker)
    { 
        if (_targettedCharacterIndex > -1)
        {
            Move(_battleCharacterList[_targettedCharacterIndex]);
            attacker.Mana -= 5;
        }
    }
    void FlourAttack(Character attacker)
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
            attacker.Mana -= 5;
            EndTurn();
        }
    }
    void RavioliBomb(Character attacker)
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
            attacker.Mana -= 5;
            EndTurn();
        }
    }
    void SpaghettiWhip(BattleCharacter attacker)
    {
        for (int i = 0; i < _battleCharacterList.Count; i++)
        {
            if (_battleCharacterList[i].Character.Alive && !_battleCharacterList[i].Character.Player)
            {
                _targettedCharacterIndex = i;
                DealDamage((int)(attacker.Character.Intelligence * (1.5 + attacker.Character.Level)));
            }
        }
        attacker.Character.Mana -= 10;
        attacker.PrimaryAction = true;
        attacker.SecondaryAction = true;
        EndTurn();
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
        if(!_battleCharacterList[characterIndex].Character.Alive)
        {
            return;
        }

        _targettedCharacterIndex = characterIndex;
        _selectingCharacter = false;
    }



    void EnemyAttack()
    {

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
        var chest = defender.Character.Equipment.GetEquippedArmour(ArmourItemData.SlotType.eCHEST); // change all refewrences to armour
        
        float magicEffect = 1.0f;

        if (weapon != null)
        {
            if (weapon.MagicalType == MagicType.eCHEESE || attacker.Character.Magic == MagicType.eCHEESE)
            {
                if (chest.MagicalType == MagicType.ePESTO || defender.Character.Magic == MagicType.ePESTO)
                {
                    magicEffect = 2.0f;
                }
                else if (chest.MagicalType == MagicType.eTOMATO || defender.Character.Magic == MagicType.eTOMATO)
                {
                    magicEffect = 0.5f;
                }
            }
            else if (weapon.MagicalType == MagicType.ePESTO || attacker.Character.Magic == MagicType.ePESTO)
            {
                if (chest.MagicalType == MagicType.eTOMATO || defender.Character.Magic == MagicType.eTOMATO)
                {
                    magicEffect = 2.0f;
                }
                else if (chest.MagicalType == MagicType.eCHEESE || defender.Character.Magic == MagicType.eCHEESE)
                {
                    magicEffect = 0.5f;
                }
            }
            else if (weapon.MagicalType == MagicType.eTOMATO || attacker.Character.Magic == MagicType.eTOMATO)
            {
                if (chest.MagicalType == MagicType.eCHEESE || defender.Character.Magic == MagicType.eCHEESE)
                {
                    magicEffect = 2.0f;
                }
                else if (chest.MagicalType == MagicType.ePESTO || defender.Character.Magic == MagicType.ePESTO)
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
            return damage = (int)((weapon.BaseDamage + Random.Range(-(int)weapon.VarianceDamage + attacker.ClassModifier / 2, (int)weapon.VarianceDamage + attacker.ClassModifier) * _battleCharacterList[_targettedCharacterIndex].Defending) * magicEffect);
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

        chanceToHit = (((50.0f + attacker.ClassModifier * 10.0f - defender.BaseArmour * 2.0f + magic) / 100.0f) - attacker.ChanceToHitModifier) * hitModifier;
        if (attacker.CurrentAction == ActionChoice.eSecondary)
        {
            chanceToHit *= 0.75f;
        }
        chanceToHit = Mathf.Clamp(chanceToHit, 0.05f, 0.95f);

        if (Random.value < chanceToHit)
        {
            return true;
        }

        return false;
    }

    void DealDamage(int damage)
    {
        damage = (int)(damage * _battleCharacterList[_characterTurnOrder[_currentCharacterIndex]].AttackBuffModifier);
        _battleCharacterList[_targettedCharacterIndex].DamageTaken += damage;
        _battleCharacterList[_targettedCharacterIndex].Character.ChangeHealth(-damage);


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
        if(!ability)
        {
            EndTurn();
        }
    }

    void Move(BattleCharacter mover)
    {
        _eventSystem.SetSelectedGameObject(null,null);
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
        }

        PlaceInColumns();
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
                    abilitiyButtons[i].gameObject.GetComponent<Button>().interactable = !abilitiyButtons[i].gameObject.GetComponent<Button>().interactable;
                }
            }
            _currentCharacterIndex++;
        }
        _optionChosen = TurnOption.eNONE;
        _targettedCharacterIndex = -1;

        _eventSystem.SetSelectedGameObject(null,null);
        _eventSystem.SetSelectedGameObject(FirstSelected);

    }

    public void SetTurnOption(int turnOption)
    {
        _skipFrame = true;
        _optionChosen = (TurnOption)turnOption;

        if (_optionChosen != TurnOption.eNONE && _optionChosen != TurnOption.eDEFEND && _optionChosen != TurnOption.eACTION)
        {
            if (_optionChosen != TurnOption.eMOVE)
            {
                _selectingCharacter = true;
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

        if (player.Character.Class == ClassType.eWARRIOR)
        {
            int colToAttack = 1;
            bool enemyAvalible = false;
            while (!enemyAvalible && colToAttack <= 3)
            {
                for (int i = 0; i < _battleCharacterList.Count; i++)
                {
                    if ((_battleCharacterList[i].Character.CurrCol == colToAttack && _battleCharacterList[i].Character.Alive && !_battleCharacterList[i].Character.Player) || _battleCharacterList[i].Character.Player)
                    {
                        if (!_battleCharacterList[i].Character.Player)
                        {
                            enemyAvalible = true;
                        }
                    }
                }
                if (!enemyAvalible)
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
                if (_battleCharacterList[i].Character.Alive && !_battleCharacterList[i].Character.Player)
                {
                    enemies.Add(_battleCharacterList[i]);
                }
            }
        }

        return enemies;
    }

    public BattleCharacter GetFirstAliveEnemy()
    {
        foreach (var character in _battleCharacterList)
        {
            if (!character.Character.Player && character.Character.Alive)
            {
                return character;
            }
        }
        return _battleCharacterList[4];
    }

    int _tempSelectedEnemy = 4;
    bool _axisInUse = false;
    void SelectEnemyState()
    {
        if(_targettedCharacterIndex > -1 || !_selectingCharacter)
        {
            if (!_selectingCharacter)
            {
                for (int i = 4; i < 8; i++)
                {
                    _battleCharacterList[i].GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
            return;
        }


        var enemies = GetSelectableEnemiesFromPlayer(_battleCharacterList[_currentCharacterIndex]);

        for (int i = 4; i < 8; i++)
        {
            _battleCharacterList[i].GetComponent<SpriteRenderer>().color = Color.white;
        }


        if (Input.GetButtonDown("A"))
        {
            SetTargettedCharacter(_tempSelectedEnemy);
        }

        //move up
        if (Input.GetAxis("Vertical") < 0.000f)
        {
            if (_axisInUse == false)
            {
                _tempSelectedEnemy = Mathf.Clamp(_tempSelectedEnemy + 1, 4, 7);
                _axisInUse = true;
            }
        }
        else if (Input.GetAxis("Vertical") == 0.0f)
        {
            _axisInUse = false;
        }


        if (Input.GetAxis("Vertical") > 0.0f)
        {
            if (_axisInUse == false)
            {
                _tempSelectedEnemy = Mathf.Clamp(_tempSelectedEnemy - 1, 4, 7);
                _axisInUse = true;
            }
        }
        else if(Input.GetAxis("Vertical") == 0.0f)
        {
            _axisInUse = false;
        }
        FindObjectOfType<EventSystem>().enabled = true;

        _battleCharacterList[_tempSelectedEnemy].GetComponent<SpriteRenderer>().color = Color.red;
    }

    IEnumerator skipFrame()
    {
        yield return new WaitForSeconds(1);
    }
}


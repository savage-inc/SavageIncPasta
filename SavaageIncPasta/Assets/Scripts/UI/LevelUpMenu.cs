using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelUpMenu : MonoBehaviour
{
    public GUIStyle FontStyle;
    private bool _levelledUp = false;
    [SerializeField]
    private GameObject _choiceMenu;
    [SerializeField]
    private Button[] _buttons = new Button[2];
    public Text AbilityDescriptionA, AbilityDescriptionB;
    private BasicLevelling _levellingManager;
    private Character _levelledUpCharacter;

    private void Awake()
    {
        _levellingManager = FindObjectOfType<BasicLevelling>();
    }

    // Use this for initialization
    void Start()
    {
        _choiceMenu.SetActive(false);

        //Set the functions that will be called when the buttons are pressed
        _buttons[0].onClick.AddListener(ChoiceAPressed);
        _buttons[0].onClick.AddListener(Resume);

        _buttons[1].onClick.AddListener(ChoiceBPressed);
        _buttons[1].onClick.AddListener(Resume);
    }

    // Update is called once per frame
    void Update()
    {
        //Check if player has pressed back button when a character has levelled up
        if (_levellingManager.GetNumOfCharactersLevelledUp() > 0 && Input.GetKeyDown(KeyCode.Joystick1Button6))
        {
            _levelledUp = true;
        }

        if (_levelledUp)
            Pause();
        else
            Resume();
    }

    //Function to resume game play when player made their choice after levelling up
    private void Resume()
    {
        if (_choiceMenu.activeInHierarchy)
        {
            _choiceMenu.SetActive(false);
            Time.timeScale = 1.0f;
            _levelledUp = false;
            _levellingManager.RemoveFirstLevelledUpCharacterFromList();
            FindObjectOfType<EventSystem>().SetSelectedGameObject(GameObject.Find("Option A"));
        }
    }

    //Function to stop the game play so player can choose an ability when they level up
    private void Pause()
    {
        if (!_choiceMenu.activeInHierarchy)
        {
            _buttons[0].GetComponentInChildren<Text>().text = DisplayOptionAText();
            _buttons[1].GetComponentInChildren<Text>().text = DisplayOptionBText();
            _choiceMenu.SetActive(true);
            Time.timeScale = 0.0f;
            FindObjectOfType<EventSystem>().SetSelectedGameObject(GameObject.Find("Option A"));
        }
    }

    //Add the correct ability to the character depending on which option they chose
    private void PlayersChoiceAfterLevellingUp(char choice)
    {
        _levelledUpCharacter = _levellingManager.GetLevelledUpCharacter();

        switch (_levelledUpCharacter.Level + 1)
        {
            case 2:
                if (choice == 'a')
                {
                    _levelledUpCharacter.Abilities.Add(2);
                }
                else if (choice == 'b')
                {
                    _levelledUpCharacter.Abilities.Add(3);
                }
                break;
            case 3:
                switch (_levelledUpCharacter.Class)
                {
                    case ClassType.eWARRIOR:
                        _levelledUpCharacter.Strength++;
                        _levelledUpCharacter.Constitution++;
                        break;
                    case ClassType.eRANGER:
                        _levelledUpCharacter.Dexterity++;
                        break;
                    case ClassType.eWIZARD:
                        _levelledUpCharacter.Intelligence++;
                        break;
                    case ClassType.eSHAMAN:
                        _levelledUpCharacter.Intelligence++;
                        break;
                }
                break;
            case 4:
                if (choice == 'a')
                {
                    if (_levelledUpCharacter.Class == ClassType.eWIZARD)
                    {
                        _levelledUpCharacter.Abilities.Add(6);
                    }
                    else
                    {
                        _levelledUpCharacter.Abilities.Add(4);
                    }
                }
                if (choice == 'b')
                {
                    _levelledUpCharacter.Abilities.Add(5);
                }

                if (_levelledUpCharacter.Class == ClassType.eWIZARD)
                {
                    _levelledUpCharacter.Abilities.Add(4);
                }
                break;
            case 5:
                switch (_levelledUpCharacter.Class)
                {
                    case ClassType.eWARRIOR:
                        _levelledUpCharacter.Abilities.Add(6);
                        _levelledUpCharacter.Strength++;
                        break;
                    case ClassType.eRANGER:
                        _levelledUpCharacter.Abilities.Add(6);
                        _levelledUpCharacter.Dexterity++;
                        break;
                    case ClassType.eWIZARD:
                        _levelledUpCharacter.Abilities.Add(7);
                        _levelledUpCharacter.Intelligence++;
                        break;
                    case ClassType.eSHAMAN:
                        _levelledUpCharacter.Abilities.Add(6);
                        break;
                }
                break;
            default:
                break;
        }

        _levelledUpCharacter.Level++;
        PersistantData.SavePartyData(FindObjectOfType<PartyInventory>(), FindObjectOfType<PlayerManager>(), FindObjectOfType<ClanManager>());
    }

    private void ChoiceAPressed()
    {
        PlayersChoiceAfterLevellingUp('a');
    }

    private void ChoiceBPressed()
    {
        PlayersChoiceAfterLevellingUp('b');
    }

    //Output the name of the first ability that the player will choose
    private string DisplayOptionAText()
    {
        _levelledUpCharacter = _levellingManager.GetLevelledUpCharacter();

        switch (_levelledUpCharacter.Level + 1)
        {
            case 2:
                AbilityDescriptionA.text = AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 2).AbilityDescription;
                return AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 2).AbilityName;
            case 3:
                switch (_levelledUpCharacter.Class)
                {
                    case ClassType.eWARRIOR:
                        AbilityDescriptionA.text = "Strength +1 and Constitution +1";
                        return "Strength +1 and Constitution +1";
                    case ClassType.eRANGER:
                        AbilityDescriptionA.text = "Dexterity +1";
                        return "Dexterity +1";
                    case ClassType.eWIZARD:
                        AbilityDescriptionA.text = "Intelligence +1";
                        return "Intelligence +1";
                    case ClassType.eSHAMAN:
                        AbilityDescriptionA.text = "Intelligence +1";
                        return "Intelligence +1";
                }
                break;
            case 4:
                if (_levelledUpCharacter.Class == ClassType.eWIZARD)
                {
                    AbilityDescriptionA.text = AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 6).AbilityDescription;
                    return AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 6).AbilityName;
                }
                else
                {
                    AbilityDescriptionA.text = AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 4).AbilityDescription;
                    return AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 4).AbilityName;
                }
            case 5:
                switch (_levelledUpCharacter.Class)
                {
                    case ClassType.eWARRIOR:
                        AbilityDescriptionA.text = AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 6).AbilityDescription;
                        return "Strength +1 and Penne Storm";
                    case ClassType.eRANGER:
                        AbilityDescriptionA.text = AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 6).AbilityDescription;
                        return "Dexterity +1 and Split Shot";
                    case ClassType.eWIZARD:
                        AbilityDescriptionA.text = AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 7).AbilityDescription;
                        return "Intelligence +1 and Spaghetti Whip";
                    case ClassType.eSHAMAN:
                        AbilityDescriptionA.text = AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 6).AbilityDescription;
                        return "Shroud of Regeneration";
                }
                break;
            default:
                return null;
        }

        return null;
    }

    //Output the name of the second ability that the player will choose
    //Outputs the first ability if there's only 1 option
    private string DisplayOptionBText()
    {
        _levelledUpCharacter = _levellingManager.GetLevelledUpCharacter();

        switch (_levelledUpCharacter.Level + 1)
        {
            case 2:
                AbilityDescriptionB.text = AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 3).AbilityDescription;
                return AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 3).AbilityName;
            case 3:
                switch (_levelledUpCharacter.Class)
                {
                    case ClassType.eWARRIOR:
                        AbilityDescriptionB.text = "Strength +1 and Constitution +1";
                        return "Strength +1 and Constitution +1";
                    case ClassType.eRANGER:
                        AbilityDescriptionB.text = "Dexterity +1";
                        return "Dexterity +1";
                    case ClassType.eWIZARD:
                        AbilityDescriptionB.text = "Intelligence +1";
                        return "Intelligence +1";
                    case ClassType.eSHAMAN:
                        AbilityDescriptionB.text = "Intelligence +1";
                        return "Intelligence +1";
                }
                break;
            case 4:
                AbilityDescriptionB.text = AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 5).AbilityDescription;
                return AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 5).AbilityName;
            case 5:
                switch (_levelledUpCharacter.Class)
                {
                    case ClassType.eWARRIOR:
                        AbilityDescriptionB.text = AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 6).AbilityDescription;
                        return "Strength +1 and Penne Storm";
                    case ClassType.eRANGER:
                        AbilityDescriptionB.text = AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 6).AbilityDescription;
                        return "Dexterity +1 and Split Shot";
                    case ClassType.eWIZARD:
                        AbilityDescriptionB.text = AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 7).AbilityDescription;
                        return "Intelligence +1 and Spaghetti Whip";
                    case ClassType.eSHAMAN:
                        AbilityDescriptionB.text = AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 6).AbilityDescription;
                        return "Shroud of Regeneration";
                }
                break;
            default:
                return null;
        }

        return null;
    }

    private void OnGUI()
    {
        if (_levellingManager.GetNumOfCharactersLevelledUp() > 0)
        {
            GUI.Box(new Rect(Screen.width - 350, 50, 350, 50), "A character has levelled up!", FontStyle);

            if (_choiceMenu.activeInHierarchy)
            {
                GUI.Box(new Rect(Screen.width / 2 - 100, 100, 250, 50), _levellingManager.GetLevelledUpCharacter().Name, FontStyle);
                GUI.Box(new Rect(Screen.width / 2 - 100, 170, 250, 50), "Welcome to level " + (_levellingManager.GetLevelledUpCharacter().Level + 1), FontStyle);
            }
        }
    }
}

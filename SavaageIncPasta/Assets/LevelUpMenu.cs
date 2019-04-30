using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelUpMenu : MonoBehaviour
{
    private bool _levelledUp = false;
    private GameObject _choiceMenu;
    private Button[] _buttons = new Button[2];
    private BasicLevelling _levellingManager;
    private Character _levelledUpCharacter;
    private int _numOptionsForLevel = 2;

    public void LevelledUp()
    {
        _levelledUp = true;
    }

    private void Awake()
    {
        _choiceMenu = GameObject.Find("Choice Menu");
        _levellingManager = FindObjectOfType<BasicLevelling>();

        _buttons[0] = GameObject.Find("Option A").GetComponent<Button>();
        _buttons[1] = GameObject.Find("Option B").GetComponent<Button>();
    }

    // Use this for initialization
    void Start()
    {
        _choiceMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_choiceMenu.activeInHierarchy)
        {
            _buttons[0].onClick.AddListener(Resume);
            _buttons[1].onClick.AddListener(Resume);
        }

        if (_levelledUp)
            Pause();
        else
            Resume();
    }

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

    private string DisplayOptionAText()
    {
        _levelledUpCharacter = gameObject.GetComponent<BasicLevelling>().GetLevelledUpCharacter();

        switch (_levelledUpCharacter.Level)
        {
            case 2:
                return AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 2 * _levelledUpCharacter.Level - 2).AbilityName;
            case 3:
                switch (_levelledUpCharacter.Class)
                {
                    case ClassType.eWARRIOR:
                        return "Strength +1 and Constitution +1";
                    case ClassType.eRANGER:
                        return "Agility +1";
                    case ClassType.eWIZARD:
                        return "Intelligence +1";
                    case ClassType.eSHAMAN:
                        return "Intelligence +1";
                }
                break;
            case 4:
                return AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 2 * _levelledUpCharacter.Level - 2).AbilityName;
            case 5:
                switch (_levelledUpCharacter.Class)
                {
                    case ClassType.eWARRIOR:
                        return "Penne Storm and Strength +1";
                    case ClassType.eRANGER:
                        return "Agility +1 and Split Shot";
                    case ClassType.eWIZARD:
                        return "Intelligence +1 and Spaghetti Whip";
                    case ClassType.eSHAMAN:
                        return "Shroud of Regeneration";
                }
                break;
            default:
                return null;
        }

        return null;
    }

    private string DisplayOptionBText()
    {
        _levelledUpCharacter = gameObject.GetComponent<BasicLevelling>().GetLevelledUpCharacter();

        if (_numOptionsForLevel == 2)
        {
            _levelledUpCharacter = gameObject.GetComponent<BasicLevelling>().GetLevelledUpCharacter();

            switch (_levelledUpCharacter.Level)
            {
                case 2:
                    return AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 2 * _levelledUpCharacter.Level - 1).AbilityName;
                case 3:
                    switch (_levelledUpCharacter.Class)
                    {
                        case ClassType.eWARRIOR:
                            return "Strength +1 and Constitution +1";
                        case ClassType.eRANGER:
                            return "Agility +1";
                        case ClassType.eWIZARD:
                            return "Intelligence +1";
                        case ClassType.eSHAMAN:
                            return "Intelligence +1";
                    }
                    break;
                case 4:
                    return AbilityManager.Instance.GetAbility(_levelledUpCharacter.Class, 2 * _levelledUpCharacter.Level - 1).AbilityName;
                case 5:
                    switch (_levelledUpCharacter.Class)
                    {
                        case ClassType.eWARRIOR:
                            return "Penne Storm and Strength +1";
                        case ClassType.eRANGER:
                            return "Agility +1 and Split Shot";
                        case ClassType.eWIZARD:
                            return "Intelligence +1 and Spaghetti Whip";
                        case ClassType.eSHAMAN:
                            return "Shroud of Regeneration";
                    }
                    break;
                default:
                    return null;
            }
        }

        return null;
    }
}

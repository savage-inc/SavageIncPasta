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
            _choiceMenu.SetActive(true);
            Time.timeScale = 0.0f;
            FindObjectOfType<EventSystem>().SetSelectedGameObject(GameObject.Find("Option A"));
        }
    }
}

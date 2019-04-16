using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelUpMenu : MonoBehaviour
{
    private bool _levelledUp = false;
    private GameObject _choiceMenu;
    public GameObject BattleEventSystem;
    private Image[] _buttons = new Image[2];
    private int _currentButton = 0;

    private void Awake()
    {
        _choiceMenu = GameObject.Find("Choice Menu");

        _buttons[0] = GameObject.Find("Option A").GetComponent<Image>();
        _buttons[1] = GameObject.Find("Option B").GetComponent<Image>();
    }

    // Use this for initialization
    void Start()
    {
        _choiceMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            if (_levelledUp)
                Resume();
            else
                Pause();
        }
    }

    void Resume()
    {
        _choiceMenu.SetActive(false);
        Time.timeScale = 1.0f;
        _levelledUp = false;
        FindObjectOfType<EventSystem>().SetSelectedGameObject(GameObject.Find("AbilitiesButton"));
    }

    void Pause()
    {
        _choiceMenu.SetActive(true);
        Time.timeScale = 0.0f;
        _levelledUp = true;
        FindObjectOfType<EventSystem>().SetSelectedGameObject(GameObject.Find("Option A"));
    }
}

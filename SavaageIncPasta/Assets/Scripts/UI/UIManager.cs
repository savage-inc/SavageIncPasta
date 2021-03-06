﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject Inventory;
    public GameObject ShopUI;
    public GameObject pauseMenuUI;
    public GameObject ClanUI;
    public GameObject BarracksUI;
    public GameObject FirstObject;
	public GameObject DialogueBox;
    public GameObject ChoiceMenu;


    public static bool GameIsPaused = false;

    private EventSystem _eventSystem;

    void Awake()
    {
        _eventSystem = FindObjectOfType<EventSystem>();

        if (FirstObject != null)
        {
            _eventSystem.SetSelectedGameObject(FirstObject);
        }
    }

    public void Close()
    {
        if (Inventory != null)
        {
            Inventory.SetActive(false);
        }

        if (pauseMenuUI)
        {
            pauseMenuUI.SetActive(false);
        }

        if (ClanUI != null)
        {
            ClanUI.SetActive(false);
        }

        if (BarracksUI != null)
        {
            BarracksUI.SetActive(false);
        }

		if (DialogueBox != null)
		{
			DialogueBox.SetActive(false);
		}

		if (ShopUI != null)

        {
            ShopUI.SetActive(false);
        }

        if (ChoiceMenu != null)
        {
            ChoiceMenu.SetActive(false);
        }
        Time.timeScale = 1f;
    }

    public void OpenInventory()
    {
        if (Inventory != null && !ChoiceMenu.activeInHierarchy)
        {
            Inventory.SetActive(true);
            //set first selected to first item
            if (FindObjectOfType<PartyInventory>().Inventory.GetItems().Count > 0)
            {
                var firstItem = Inventory.transform.GetChild(1).GetComponent<PartyInventoryUI>().InventoryContent.transform.GetChild(0).gameObject;
                _eventSystem.SetSelectedGameObject(firstItem);
            }
            else
            {
                var firstItem = Inventory.transform.GetChild(2).GetComponent<CharacterInventoryUI>().HeadButton.gameObject;
                _eventSystem.SetSelectedGameObject(firstItem);
            }

            if (pauseMenuUI != null)
            {
                pauseMenuUI.SetActive(false);
            }
            if (ClanUI != null)
            {
                ClanUI.SetActive(false);
            }
            if (BarracksUI != null)
            {
                BarracksUI.SetActive(false);
            }
            if (ShopUI != null)
            {
                ShopUI.SetActive(false);
            }
			if (DialogueBox != null)
			{
				DialogueBox.SetActive(false);
			}
			Time.timeScale = 0f;
        }
    }

    public void OpenClanUI()
    {
        Close();
        if (ClanUI != null)
        {
            ClanUI.SetActive(true);

            if (pauseMenuUI != null)
            {
                pauseMenuUI.SetActive(false);
            }
            if (Inventory != null)
            {
                Inventory.SetActive(false);
            }

            if (BarracksUI != null)
            {
                BarracksUI.SetActive(false);
            }
            Time.timeScale = 0f;
        }
    }

    public void OpenBarracksUI()
    {
        if (BarracksUI != null)
        {
            BarracksUI.SetActive(true);

            if (pauseMenuUI != null)
            {
                pauseMenuUI.SetActive(false);
            }
            if (Inventory != null)
            {
                Inventory.SetActive(false);
            }
            if (ClanUI != null)
            {
                ClanUI.SetActive(false);
            }
            if (ShopUI != null)
            {
                ShopUI.SetActive(false);
            }
			if (DialogueBox != null)
			{
				DialogueBox.SetActive(false);
			}
			Time.timeScale = 0f;
        }
    }
	public void OpenDialogueBox()
	{
		if (DialogueBox != null)
		{
			DialogueBox.SetActive(true);

			if (pauseMenuUI != null)
			{
				pauseMenuUI.SetActive(false);
			}
			if (Inventory != null)
			{
				Inventory.SetActive(false);
			}
			if (ShopUI != null)
			{
				ShopUI.SetActive(false);
			}
			if (ClanUI != null)
			{
				ClanUI.SetActive(false);
			}
			Time.timeScale = 0f;
		}
	}

	void Update()
    {
        if (Input.GetButtonDown("Y") || Input.GetKeyDown(KeyCode.I)) // X button
        {
            if (Inventory.activeInHierarchy)
            {
                Close();
            }
            else
            {
                OpenInventory();
            }
        }

        if (Input.GetButtonDown("B") && !ChoiceMenu.activeInHierarchy) // X button
        {
            Close();
        }

        if (Input.GetButtonDown("Start")) // start button
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void ShowLevel()
    {
        if (ChoiceMenu != null)
        {
            ChoiceMenu.SetActive(true);

            if (pauseMenuUI != null)
            {
                pauseMenuUI.SetActive(false);
            }
            if (Inventory != null)
            {
                Inventory.SetActive(false);
            }
            if (ShopUI != null)
            {
                ShopUI.SetActive(false);
            }
            if (ClanUI != null)
            {
                ClanUI.SetActive(false);
            }
            if (DialogueBox != null)
            {
                DialogueBox.SetActive(false);
            }
            Time.timeScale = 0f;
        }
    }

    	
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    void Pause()
    {
        _eventSystem.SetSelectedGameObject(pauseMenuUI.transform.GetChild(1).gameObject);

        pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        // link Menu here
        Debug.Log("Loading menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}

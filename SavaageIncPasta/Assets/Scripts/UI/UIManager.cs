using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject Inventory;
    public GameObject pauseMenuUI;
    public GameObject FirstObject;

    public static bool GameIsPaused = false;

    void Awake()
    {
        if (FirstObject != null)
        {
            EventSystem eventSystem = FindObjectOfType<EventSystem>();
            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(FirstObject);
            Button button = FirstObject.GetComponent<Button>();
            if (button != null)
            {
                button.OnSelect(null);
            }
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
    }
    public void OpenInventory()
    {
        if (Inventory != null)
        {
            Inventory.SetActive(true);
            if (pauseMenuUI != null)
            {
                pauseMenuUI.SetActive(true);
            }
        }
    }

    public void OpenPause()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
            if (Inventory != null)
            {
                Inventory.SetActive(false);
            }
        }
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Fire3")) // X button
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

        if (Input.GetButtonDown("Pause")) // start button
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

    	
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        // link Menu here
        Debug.Log("Loading menu...");
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}

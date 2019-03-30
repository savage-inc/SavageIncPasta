using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UIManager : MonoBehaviour
{
    public GameObject OffCanvas;
    public GameObject OnCanvas;
    public GameObject FirstObject;

    public static bool GameIsPaused = false;

    public void Switch()
    {
        OffCanvas.SetActive(true);
        OnCanvas.SetActive(false);
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(FirstObject, null);
    }

    public GameObject Inventory;
    public GameObject Menu;
    public GameObject pauseMenuUI;

    public void Close()
    {
        Inventory.SetActive(false);
        Menu.SetActive(false);
        pauseMenuUI.SetActive(false);
    }
    public void OpenInventory()
    {
        if (Inventory != null)
            Inventory.SetActive(true);
        if (Menu != null)
            Menu.SetActive(false);
    }
    public void OpenMenu()
    {
        Inventory.SetActive(false);
        Menu.SetActive(true);
    }
    public void OpenPause()
    {
        pauseMenuUI.SetActive(true);
        Inventory.SetActive(false);
        Menu.SetActive(false);
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
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}

//    public GameObject InventoryGameObject;
//    // Use this for initialization
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.I))
//        {
//            if (InventoryGameObject.activeInHierarchy)
//            {
//                InventoryGameObject.SetActive(false);
//            }
//            else
//            {
//                InventoryGameObject.SetActive(true);
//            }
//        }
//    }
//}

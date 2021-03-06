﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string MainSceneName;

    public void PlayGame()
    {
        SceneManager.LoadScene(MainSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void ClearSaves()
    {
        PersistantData.ClearSaves();
    }

}


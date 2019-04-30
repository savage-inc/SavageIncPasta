using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClanNPC : MonoBehaviour {
    private UIManager _uiManager;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

    /*void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetButtonDown("X") || Input.GetKeyDown(KeyCode.E))
        {
            if (!_uiManager.ClanUI.gameObject.activeInHierarchy)
            {
                ShowClan();
            }
            else
            {
                CloseClan();
            }
        }
    }*/

    void OnTriggerExit2D(Collider2D other)
    {
        CloseClan();
    }

    public void ShowClan()
    {
        _uiManager.OpenClanUI();
    }

    public void CloseClan()
    {
        _uiManager.Close();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : MonoBehaviour
{
    private UIManager _uiManager;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetButtonDown("A") || Input.GetKeyDown(KeyCode.E))
        {
            if (!_uiManager.BarracksUI.gameObject.activeInHierarchy)
            {
                StartCoroutine(showbarracks());
            }
            else
            {
                CloseBarracks();
            }
        }
    }

    IEnumerator showbarracks()
    {
        yield return new WaitForEndOfFrame();
        ShowBarracks();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        CloseBarracks();
    }

    public void ShowBarracks()
    {
        _uiManager.OpenBarracksUI();
    }

    public void CloseBarracks()
    {
        _uiManager.Close();
    }
}
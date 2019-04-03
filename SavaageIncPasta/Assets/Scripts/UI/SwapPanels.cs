using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwapPanels : MonoBehaviour
{ 
    public GameObject PanelA;
    public GameObject FirstSelectedA;

    public GameObject PanelB;
    public GameObject FirstSelectedB;

    private EventSystem _eventSystem;

    void Awake()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
    }

    public void Toggle()
    {
        if (PanelA.activeInHierarchy)
        {
            PanelA.SetActive(false);
            PanelB.SetActive(true);
            _eventSystem.SetSelectedGameObject(FirstSelectedB);
        }
        else
        {
            PanelB.SetActive(false);
            PanelA.SetActive(true);
            _eventSystem.SetSelectedGameObject(FirstSelectedA);
        }
    }
}

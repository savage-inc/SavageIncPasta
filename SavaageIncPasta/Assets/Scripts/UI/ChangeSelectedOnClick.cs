using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeSelectedOnClick : MonoBehaviour
{
    public GameObject ChangeTo;
    private EventSystem _eventSystem;

    void Awake()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
    }

    public void ChangeSelected()
    {
        if (ChangeTo != null)
        {
            _eventSystem.SetSelectedGameObject(ChangeTo);
        }
    }
}

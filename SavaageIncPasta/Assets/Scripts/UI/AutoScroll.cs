using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ScrollRect))]
public class AutoScroll : MonoBehaviour {

    public float LerpTime = 1.0f;
    private ScrollRect _scrollRect;
    private EventSystem _eventSystem;
    private Button[] _buttons;
    private int _index;
    private float _verticalPosition;
    private bool _up;
    private bool _down;

    // Use this for initialization
    void Start ()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _eventSystem = FindObjectOfType<EventSystem>();
        _buttons = transform.GetChild(0).GetChild(0).GetComponentsInChildren<Button>();
        _verticalPosition = 1f - ((float)_index / (_buttons.Length - 1));
    }
	
	// Update is called once per frame
	void Update ()
    {
        try
        {
            //get the index of the button
            if (_eventSystem.currentSelectedGameObject.transform.parent.parent.parent.GetComponent<AutoScroll>() != null)
            {
                _index = _eventSystem.currentSelectedGameObject.transform.GetSiblingIndex();
                _verticalPosition = 1f - ((float)_index / (_buttons.Length - 1));
            }
            _scrollRect.verticalNormalizedPosition = _verticalPosition;
        }
        catch (System.Exception e)
        {
            return;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject InventoryGameObject;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (InventoryGameObject.activeInHierarchy)
            {
                InventoryGameObject.SetActive(false);
            }
            else
            {
                InventoryGameObject.SetActive(true);
            }
        }
	}
}

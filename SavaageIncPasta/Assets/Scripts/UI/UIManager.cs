using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UIManager : MonoBehaviour
{
    public GameObject OffCanvas;
    public GameObject OnCanvas;
    public GameObject FirstObject;
    public void Switch()
    {
        OffCanvas.SetActive(true);
        OnCanvas.SetActive(false);
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(FirstObject, null);
    }

    public GameObject Inventory;
    public GameObject Menu;
    public GameObject Pause;

    public void Close()
    {
        Inventory.SetActive(false);
        Menu.SetActive(false);
        Pause.SetActive(false);
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
        Pause.SetActive(true);
        Inventory.SetActive(false);
        Menu.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetButtonDown("Y"))
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

        if (Input.GetButtonDown("Start"))
        {
            if(Pause.activeInHierarchy)
            {
                Close();
            }
            else
            {
                OpenPause();
            }
        }
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

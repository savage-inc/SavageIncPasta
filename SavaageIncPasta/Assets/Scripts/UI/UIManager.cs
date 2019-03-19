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
    public void Close()
    {
        Inventory.SetActive(false);
        Menu.SetActive(false);
    }
    public void OpenInventory()
    {
        Inventory.SetActive(true);
        Menu.SetActive(false);
    }
    public void OpenMenu()
    {
        Inventory.SetActive(false);
        Menu.SetActive(true);
    }


 //   public GameObject InventoryGameObject;
	//// Use this for initialization
	//void Start () {
		
	//}
	
	//// Update is called once per frame
	//void Update ()
 //   {
 //       if (Input.GetKeyDown(KeyCode.I))
 //       {
 //           if (InventoryGameObject.activeInHierarchy)
 //           {
 //               InventoryGameObject.SetActive(false);
 //           }
 //           else
 //           {
 //               InventoryGameObject.SetActive(true);
 //           }
 //       }
	//}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[System.Serializable]
public struct BarrackCharacter
{
    public BaseItemData itemData;
    public int numofChar;   // Number of characters available
}


[RequireComponent(typeof(GameObjectGUID))]
public class Barracks : MonoBehaviour
{
    public BarracksUI barracksUI;
    public List<BarrackCharacter> BarrackStartItems;
    public float PriceModifier = 1.0f;
    //How long should the barrack restock soldiers since last visit (In minutes)
    public float RestockTime = 1.0f;

    public Inventory Inventory { get; private set; }
    private PartyInventory _partyInventory;
    private float _lastVisit;

    void Awake()
    {
        StockBarracks();

        _partyInventory = FindObjectOfType<PartyInventory>();

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetButtonDown("A"))
        {
            if (!barracksUI.gameObject.activeInHierarchy)
            {
                ShowBarracks();
            }
            else
            {
                CloseBarracks();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        CloseBarracks();
    }

    public void ShowBarracks()
    {
        //check to restock
        if (_lastVisit + (RestockTime * 60.0f) <= Time.realtimeSinceStartup)
        {
            //restock
            Debug.Log("Restocking shop");
            ShowBarracks();
            _lastVisit = 0.0f;
        }

        barracksUI.barracks = this;
        barracksUI.gameObject.SetActive(true);
    }

    public void CloseBarracks()
    {
        barracksUI.gameObject.SetActive(false);
        barracksUI.barracks = null;
        if (_lastVisit == 0.0f)
        {
            _lastVisit = Time.realtimeSinceStartup;
        }
    }

    private void StockBarracks()
    {
        Inventory = new Inventory(BarrackStartItems.Count, true);
        foreach (var Soldier in BarrackStartItems)
        {
            for (int i = 0; i < Soldier.numofChar; i++)
            {
                Inventory.AddItem(Soldier.itemData);
            }
        }
    }
}

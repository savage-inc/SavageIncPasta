using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class BarracksUI : MonoBehaviour
{
    public Barracks barracks;
    public GameObject ItemPanelObject;
    public GameObject PartyInventoryObject;
    public RectTransform PartyInventoryContent;
    public Text PartyGoldText;
    public GameObject firstItem;

    private PartyInventory _partyInventory;


    void Start()
    {
        FindObjectOfType<EventSystem>().SetSelectedGameObject(firstItem);
    }

    void Awake()
    {
        _partyInventory = FindObjectOfType<PartyInventory>();
    }

    void Update()
    {
        if (Input.GetButton("B"))
        {
            gameObject.SetActive(false);
        }
        else if (Input.GetButton("LB"))
        {
            ShowBarracks();
        }
    }


    void SyncShop()
    {
        //Get shop Inventory
        foreach (var item in Shop.Inventory.GetItems())
        {
            AddShopUIItem(item);
        }

        PartyGoldText.text = "Party Gold " + _partyInventory.Gold;
    }

}


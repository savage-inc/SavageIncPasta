using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyInventoryUI : MonoBehaviour
{
    public GameObject ItemPanelObject;
    public RectTransform Content;

    private Inventory _inventory;

	// Use this for initialization
	void Awake ()
    {
        _inventory = FindObjectOfType<PartyInventory>().Inventory;
        _inventory.OnItemAdd += AddUIItem;
        _inventory.OnItemRemove += RemoveItemUI;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void AddUIItem(BaseItemData item)
    {
        var itemPanel = Instantiate(ItemPanelObject);
        itemPanel.transform.SetParent(Content.transform);

        itemPanel.transform.GetChild(0).GetComponent<Image>().sprite = item.PreviewSprite;
        itemPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "x" + 1; //TODO get inventory amount
        itemPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = item.Name;
    }

    void RemoveItemUI(BaseItemData item, int amount)
    {

    }
}

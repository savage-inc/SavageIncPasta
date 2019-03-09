using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShopItem
{
    public BaseItemData Item;
    public int Stock;
}

[RequireComponent(typeof(GameObjectGUID))]
public class Shop : MonoBehaviour
{
    public List<ShopItem> ShopStartItems;
    public float PriceModfier = 1.0f;
    public Inventory Inventory { get; private set; }
    private PartyInventory _partyInventory;
    public ShopInventoryUI ShopUI;


    // Use this for initialization
    void Awake ()
    {
        Inventory = new Inventory(ShopStartItems.Count, true);
        foreach (var shopItem in ShopStartItems)
        {
            for (int i = 0; i < shopItem.Stock; i++)
            {
                Inventory.AddItem(shopItem.Item);
            }
        }

        _partyInventory = FindObjectOfType<PartyInventory>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetButtonDown("A") || Input.GetKeyDown(KeyCode.E))
        {
            if (!ShopUI.gameObject.activeInHierarchy)
            {
                ShowShop();
            }
            else
            {
                CloseShop();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        CloseShop();
    }

    public void ShowShop()
    {
        ShopUI.Shop = this;
        ShopUI.gameObject.SetActive(true);
    }

    public void CloseShop()
    {
        ShopUI.gameObject.SetActive(false);
        ShopUI.Shop = null;
    }

    //Sell an item to the player
    public void SellItem(BaseItemData item)
    {
        int actualPrice = (int)(item.BaseMoneyValue * PriceModfier);

        //check if the party has enough Gold
        if (_partyInventory.Gold - actualPrice >= 0)
        {
            _partyInventory.Gold -= actualPrice;
            _partyInventory.Inventory.AddItem(item);

            Inventory.RemoveItem(item.Name);
        }
    }

    //Buy an item from a player
    public void BuyItem(BaseItemData item)
    {
        //sell at half price
        _partyInventory.Gold += (int)(item.BaseMoneyValue / 2.0f);
        _partyInventory.Inventory.RemoveItem(item.Name);

        //add it to the shop
        Inventory.AddItem(item);
    }
}

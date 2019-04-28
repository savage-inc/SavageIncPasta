using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public struct ShopItem
{
    public BaseItemData Item;
    public int Stock;
}

[RequireComponent(typeof(GameObjectGUID))]
public class Shop : MonoBehaviour
{
    public ShopInventoryUI ShopUI;
    public List<ShopItem> ShopStartItems;
    public float PriceModfier = 1.0f;
    //How long should the shop restock since last visit (In minutes)
    public float RestockTime = 1.0f;

    public bool RandomWeapons = false;
    public bool RandomArmour = false;
    public int RandomItemCount = 0;

    public Inventory Inventory { get; private set; }
    private PartyInventory _partyInventory;
    private float _lastVisit;

    // Use this for initialization
    void Awake ()
    {
        _partyInventory = FindObjectOfType<PartyInventory>();

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetButtonDown("A"))
        {
            if (!ShopUI.gameObject.activeInHierarchy)
            {
                StartCoroutine(showshop());
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

    IEnumerator showshop()
    {
        yield return new WaitForEndOfFrame();
        ShowShop();
    }
    public void ShowShop()
    {
        if(Inventory == null || Inventory.GetItems().Count == 0)
        {
            StockShop();
        }
        //check to restock
        if (_lastVisit + (RestockTime * 60.0f )<= Time.realtimeSinceStartup)
        {
            //restock
            Debug.Log("Restocking shop");
            StockShop();
            _lastVisit = 0.0f;
        }

        ShopUI.Shop = this;
        ShopUI.gameObject.SetActive(true);
    }

    public void CloseShop()
    {
        ShopUI.gameObject.SetActive(false);
        ShopUI.Shop = null;
        if (_lastVisit == 0.0f)
        {
            _lastVisit = Time.realtimeSinceStartup;
        }
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

    private void StockShop()
    {
        Inventory = new Inventory(ShopStartItems.Count, true);
        foreach (var shopItem in ShopStartItems)
        {
            for (int i = 0; i < shopItem.Stock; i++)
            {
                Inventory.AddItem(shopItem.Item);
            }
        }

        //add random items
        if (RandomArmour)
        {
            for (int i = 0; i < RandomItemCount; i++)
            {
                ShopItem shopItem;
                shopItem.Item = ItemDatabase.Instance.Armour[Random.Range(0, ItemDatabase.Instance.Armour.Count)];
                shopItem.Stock = 1;
                Inventory.AddItem(shopItem.Item);
            }
        }

        if (RandomWeapons)
        {
            for (int i = 0; i < RandomItemCount; i++)
            {
                ShopItem shopItem;
                shopItem.Item = ItemDatabase.Instance.Weapons[Random.Range(0, ItemDatabase.Instance.Weapons.Count)];
                shopItem.Stock = 1;
                Inventory.AddItem(shopItem.Item);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public InventoryItem Item;
    public Shop Shop;


    public GameObject ItemToolTipPrefab;
    private GameObject _itemToolTipInstance;

    private void Awake()
    {
        Button button = GetComponent<Button>();

        EventTrigger.Entry enterEvent = new EventTrigger.Entry();
        enterEvent.eventID = EventTriggerType.PointerEnter;
        enterEvent.callback.AddListener((eventData) => { OnHoverEnter(); });

        EventTrigger.Entry exitEvent = new EventTrigger.Entry();
        exitEvent.eventID = EventTriggerType.PointerExit;
        exitEvent.callback.AddListener((eventData) => { OnHoverExit(); });

        button.gameObject.AddComponent<EventTrigger>();
        button.gameObject.GetComponent<EventTrigger>().triggers.Add(enterEvent);
        button.gameObject.GetComponent<EventTrigger>().triggers.Add(exitEvent);
    }

    //Sell the item to a shop
    public void SellItem()
    {
        if (Shop == null || Shop == null || Item == null || Item.Item == null)
            return;

        //Shop buys the item
        Shop.BuyItem(Item.Item);

        Destroy(_itemToolTipInstance);
    }

    //Buy the item from a shop
    public void BuyItem()
    {
        if (Shop == null || Shop == null || Item == null || Item.Item == null)
            return;

        //shop sells the item to the party
        Shop.SellItem(Item.Item);

        Destroy(_itemToolTipInstance);
    }

    public void OnHoverEnter()
    {
        if (Item == null || Item.Item == null)
            return;

        //instantiate tool tip
        _itemToolTipInstance = Instantiate(ItemToolTipPrefab);
        _itemToolTipInstance.gameObject.SetActive(true);

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        float width = rectTransform.rect.width / 2;
        _itemToolTipInstance.transform.SetParent(FindObjectOfType<Canvas>().transform);
        _itemToolTipInstance.transform.position = new Vector3(gameObject.transform.position.x - width, gameObject.transform.position.y, 0);

        _itemToolTipInstance.GetComponent<ItemTooltip>().Item = Item.Item;
    }

    public void OnHoverExit()
    {
        if (Item == null || Item.Item == null)
            return;

        Destroy(_itemToolTipInstance);

    }



    void OnDisable()
    {
        Destroy(_itemToolTipInstance);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (Item == null || Item.Item == null)
            return;

        //instantiate tool tip
        _itemToolTipInstance = Instantiate(ItemToolTipPrefab);
        _itemToolTipInstance.gameObject.SetActive(true);

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        float width = rectTransform.rect.width / 2;
        _itemToolTipInstance.transform.SetParent(FindObjectOfType<Canvas>().transform);
        _itemToolTipInstance.transform.position = new Vector3(gameObject.transform.position.x - width, gameObject.transform.position.y, 0);

        _itemToolTipInstance.GetComponent<ItemTooltip>().Item = Item.Item;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (Item == null || Item.Item == null)
            return;

        Destroy(_itemToolTipInstance);
    }
}

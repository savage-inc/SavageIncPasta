using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public InventoryItem Item;
    public Inventory Inventory;
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

    public void TransferItem(Inventory to)
    {
        if(Inventory == null || to == null || Item == null || Item.Item == null)
            return;

        to.AddItem(Item.Item);
        Inventory.RemoveItem(Item.Item.Name);

        Destroy(_itemToolTipInstance);
    }

    public void EquipItem(CharacterEquipment to)
    {
        if (Inventory == null || to == null || Item == null || Item.Item == null)
            return;

        switch (Item.Item.ItemType)
        {
            case ItemType.eCONSUMABLE:
                break;
            case ItemType.eARMOUR:
                to.EquipArmour((ArmourItemData)Item.Item, Inventory);
                break;
            case ItemType.eWEAPON:
                to.EquipWeapon((WeaponItemData)Item.Item, Inventory);
                break;
            case ItemType.eMAGICWEAPON:
                to.EquipWeapon((WeaponItemData)Item.Item, Inventory);
                break;
        }

        Destroy(_itemToolTipInstance);
    }

    public void ConsumeItem()
    {
        if (Inventory == null || Item == null || Item.Item == null)
            return;

        Inventory.RemoveItem(Item);

        Destroy(_itemToolTipInstance);
    }

    public void RemoveItem()
    {
        if (Inventory == null || Item == null || Item.Item == null)
            return;

        Inventory.RemoveItem(Item);

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
        float width = rectTransform.rect.width/2;
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
}

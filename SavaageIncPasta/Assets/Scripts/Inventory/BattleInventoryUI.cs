using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleInventoryUI : MonoBehaviour
{
    public Sprite BaseSlotSprite;
    public GameObject Player1;
    private List<InventoryItemButton> _slots;
    private PartyInventory _partyInventory;
    public ConsumableItemData SelectedConsumable { get; private set; }
    private EventSystem _eventSystem;

    void Awake()
    {
        _slots = new List<InventoryItemButton>();
        _partyInventory = FindObjectOfType<PartyInventory>();
        _eventSystem = FindObjectOfType<EventSystem>();
        foreach (Transform child in transform)
        {
            var button = child.gameObject.GetComponent<InventoryItemButton>();
            button.GetComponent<Button>().onClick.AddListener(() => useItem(button));
            _slots.Add(button);

        }
    }

	// Use this for initialization
	void Start ()
    {
        //load party inventory from file
        PersistantData.LoadPartyData(_partyInventory, null);
        AddItems();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void AddItems()
    {
        //loop through party inventory
        foreach (var inventoryItem in _partyInventory.Inventory.GetItems())
        {
            //only add consumables
            if (inventoryItem.Item.ItemType == ItemType.eCONSUMABLE)
            {
                var slot = getEmptySlot();
                slot.Item = inventoryItem;
                slot.Inventory = _partyInventory.Inventory;
                slot.GetComponent<Image>().sprite = slot.Item.Item.PreviewSprite;
            }
        }
    }

    InventoryItemButton getEmptySlot()
    {
        foreach (var slot in _slots)
        {
            if (slot.Item == null)
                return slot;
        }
        return _slots[0];
    }

    void sortSlots()
    {
        bool sortNext = false;
        foreach (var slot in _slots)
        {
            if (slot.Item == null)
            {
                sortNext = true;
                continue;
            }

            if (sortNext == true && slot.Item != null)
            {
                var newSlot = getEmptySlot();
                newSlot.Item = slot.Item;
                newSlot.Inventory = slot.Inventory;

                slot.Item = null;
                slot.Inventory = null;
            }
        }
    }

    void useItem(InventoryItemButton slot)
    {
        if(slot.Item.Item == null)
            return;
        if (slot.Item.Amount == 1)
        {
            removeSlot(slot);
            sortSlots();
            return;
        }

        Debug.Log("Using item " + slot.Item.Item.Name);
        SelectedConsumable = (ConsumableItemData)slot.Item.Item;
        _partyInventory.Inventory.RemoveItem(slot.Item);

        //make the player select a character
        _eventSystem.SetSelectedGameObject(Player1);
    }

    void removeSlot(InventoryItemButton slot)
    {
        slot.Item = null;
        slot.Inventory = null;
        slot.GetComponent<Image>().sprite = BaseSlotSprite;

    }

    public void Save()
    {
        PersistantData.SavePartyData(_partyInventory,FindObjectOfType<PlayerManager>());
    }

    public void UseConsumableOnCharacter(Character character)
    {
        if (SelectedConsumable != null)
        {
            switch (SelectedConsumable.ConsumableType)
            {
                case ConsumableItemData.Type.eHEALTH:
                    character.ChangeHealth((int)SelectedConsumable.EffectAmount);
                    SelectedConsumable = null;
                    break;
                case ConsumableItemData.Type.eMANA:
                    character.Mana += (int)SelectedConsumable.EffectAmount;
                    SelectedConsumable = null;
                    break;
                case ConsumableItemData.Type.eSTRENGTH:
                    SelectedConsumable = null;
                    break;
                case ConsumableItemData.Type.eCONSITUTION:
                    SelectedConsumable = null;
                    break;
                case ConsumableItemData.Type.eDEXTERITY:
                    SelectedConsumable = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

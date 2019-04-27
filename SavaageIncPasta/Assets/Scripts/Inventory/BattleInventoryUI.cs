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
    public GameObject FirstSelected;
    private List<InventoryItemButton> _slots;
    private PartyInventory _partyInventory;

    public ConsumableItemData SelectedConsumable { get; private set; }
    private EventSystem _eventSystem;
    private Battle _battle;
    private List<BattleCharacter> _battleCharacters;

    private bool _selectingPlayer = false;
    private bool _skipFrame = false;
    private int _selectedPlayer = 0;
    private int _selectedItemSlot = 0;
    private bool _axisInUse = false;

    void Awake()
    {
        _slots = new List<InventoryItemButton>();
        _partyInventory = FindObjectOfType<PartyInventory>();
        _eventSystem = FindObjectOfType<EventSystem>();
        _battle = FindObjectOfType<Battle>();
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
        _battleCharacters = _battle.GetBattleCharacters();
    }
	
	// Update is called once per frame
	void Update () {
        if (_skipFrame)
        {
            _skipFrame = false;
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            _battleCharacters[i].GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (!_selectingPlayer)
        {
            return;
        }


        if (Input.GetButtonDown("A"))
        {
            UseConsumableOnCharacter(_battleCharacters[_selectedPlayer].Character, _selectedItemSlot);
        }

        //move up
        if (Input.GetAxis("Vertical") < 0.000f)
        {
            if (_axisInUse == false)
            {
                _selectedPlayer = Mathf.Clamp(_selectedPlayer + 1, 0, 3);
                _axisInUse = true;
            }
        }
        else if (Input.GetAxis("Vertical") == 0.0f)
        {
            _axisInUse = false;
        }


        if (Input.GetAxis("Vertical") > 0.0f)
        {
            if (_axisInUse == false)
            {
                _selectedPlayer = Mathf.Clamp(_selectedPlayer - 1, 0, 3);
                _axisInUse = true;
            }
        }
        else if (Input.GetAxis("Vertical") == 0.0f)
        {
            _axisInUse = false;
        }
        FindObjectOfType<EventSystem>().enabled = true;

        _battleCharacters[_selectedPlayer].GetComponent<SpriteRenderer>().color = Color.red;
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

        SelectedConsumable = (ConsumableItemData)slot.Item.Item;

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

    public void UseConsumableOnCharacter(Character character, int slotIndex)
    {
        if (SelectedConsumable != null)
        {
            switch (SelectedConsumable.ConsumableType)
            {
                case ConsumableItemData.Type.eHEALTH:
                    character.ChangeHealth((int)SelectedConsumable.EffectAmount);            
                    break;
                case ConsumableItemData.Type.eMANA:
                    character.Mana += (int)SelectedConsumable.EffectAmount;
                    break;
                case ConsumableItemData.Type.eSTRENGTH:
                    break;
                case ConsumableItemData.Type.eCONSITUTION:
                    break;
                case ConsumableItemData.Type.eDEXTERITY:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //remove object 
        Debug.Log("Using item " + SelectedConsumable.Name);

        _partyInventory.Inventory.RemoveItem(_slots[slotIndex].Item);
        if (_slots[slotIndex].Item.Amount == 0)
        {
            removeSlot(_slots[slotIndex]);
            sortSlots();
        }
        SelectedConsumable = null;

        _selectingPlayer = false;
        _eventSystem.SetSelectedGameObject(null,null);
        _eventSystem.SetSelectedGameObject(FirstSelected);
    }

    public void UseItem(int itemSlot)
    {
        if (_slots[itemSlot].Item.Item == null)
        {
            return;
        }
        _selectingPlayer = true;
        _skipFrame = true;
        _selectedItemSlot = itemSlot;
        useItem(_slots[itemSlot]);
    }

    public void DisplayItem(int slotIndex)
    {
        FindObjectOfType<BattleInfoUI>().Item = _slots[slotIndex].Item.Item;
    }

    public void RemoveItemDisplay()
    {
        FindObjectOfType<BattleInfoUI>().Item = null;
    }
}

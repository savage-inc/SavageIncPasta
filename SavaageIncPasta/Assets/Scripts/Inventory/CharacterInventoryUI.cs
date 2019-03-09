using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInventoryUI : MonoBehaviour
{
    public Text CharacterName;
    public Image CharacterPreview;
    public Button HeadButton;
    public Button BodyButton;
    public Button LegsButton;
    public Button MainHandButton;
    public Button OffHandButton;

    public Sprite DefaultPreviewSprite;

    private CharacterEquipment _currentCharacterEquipment;
    private Inventory _partyInventory;

    // Use this for initialization
    void Start ()
    {
        //TODO get a specfi 
        _currentCharacterEquipment = FindObjectOfType<CharacterInventory>().CurrentCharacterEquipment;
        

        //get party inventory
        _partyInventory = FindObjectOfType<PartyInventory>().Inventory;

        //On button Click
        HeadButton.onClick.AddListener(() => HeadButton.GetComponent<InventoryItemButton>().TransferItem(_partyInventory));
        BodyButton.onClick.AddListener(() => BodyButton.GetComponent<InventoryItemButton>().TransferItem(_partyInventory));
        LegsButton.onClick.AddListener(() => LegsButton.GetComponent<InventoryItemButton>().TransferItem(_partyInventory));
        MainHandButton.onClick.AddListener(() => MainHandButton.GetComponent<InventoryItemButton>().TransferItem(_partyInventory));
        OffHandButton.onClick.AddListener(() => OffHandButton.GetComponent<InventoryItemButton>().TransferItem(_partyInventory));
    }

    void OnDisable()
    {
        if (_currentCharacterEquipment != null)
        {
            _currentCharacterEquipment.OnItemAdd -= AddUIItem;
            _currentCharacterEquipment.OnItemRemove -= RemoveItemUI;
        }

        HeadButton.image.sprite = DefaultPreviewSprite;
        HeadButton.GetComponent<InventoryItemButton>().Item = null;
        BodyButton.image.sprite = DefaultPreviewSprite;
        BodyButton.GetComponent<InventoryItemButton>().Item = null;
        LegsButton.image.sprite = DefaultPreviewSprite;
        LegsButton.GetComponent<InventoryItemButton>().Item = null;
        MainHandButton.image.sprite = DefaultPreviewSprite;
        MainHandButton.GetComponent<InventoryItemButton>().Item = null;
        OffHandButton.image.sprite = DefaultPreviewSprite;
        OffHandButton.GetComponent<InventoryItemButton>().Item = null;
    }

    void OnEnable()
    {
        if (_currentCharacterEquipment == null)
        {
            return;
        }

        _currentCharacterEquipment.OnItemAdd += AddUIItem;
        _currentCharacterEquipment.OnItemRemove += RemoveItemUI;
        SyncEquipment();
    }

    void SyncEquipment()
    {        
        foreach (var item in _currentCharacterEquipment.GetItems())
        {
            AddUIItem(item);
        }
    }

    void AddUIItem(InventoryItem item)
    {
        if (item.Item.ItemType == ItemType.eARMOUR)
        {
            ArmourItemData armourItem = (ArmourItemData) item.Item;
            switch (armourItem.ArmourSlotType)
            {
                case ArmourItemData.SlotType.eHEAD:
                    HeadButton.image.sprite = armourItem.PreviewSprite;
                    HeadButton.GetComponent<InventoryItemButton>().Item = item;
                    HeadButton.GetComponent<InventoryItemButton>().Inventory = _currentCharacterEquipment;
                    break;
                case ArmourItemData.SlotType.eCHEST:
                    BodyButton.image.sprite = armourItem.PreviewSprite;
                    BodyButton.GetComponent<InventoryItemButton>().Item = item;
                    BodyButton.GetComponent<InventoryItemButton>().Inventory = _currentCharacterEquipment;
                    break;
                case ArmourItemData.SlotType.eLEGS:
                    LegsButton.image.sprite = armourItem.PreviewSprite;
                    LegsButton.GetComponent<InventoryItemButton>().Item = item;
                    LegsButton.GetComponent<InventoryItemButton>().Inventory = _currentCharacterEquipment;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        else if (item.Item.ItemType == ItemType.eWEAPON)
        {
            WeaponItemData weaponItem = (WeaponItemData) item.Item;
            if (weaponItem.IsMainHand)
            {
                MainHandButton.image.sprite = weaponItem.PreviewSprite;
                MainHandButton.GetComponent<InventoryItemButton>().Item = item;
                MainHandButton.GetComponent<InventoryItemButton>().Inventory = _currentCharacterEquipment;
            }
            else
            {
                OffHandButton.image.sprite = weaponItem.PreviewSprite;
                OffHandButton.GetComponent<InventoryItemButton>().Item = item;
                OffHandButton.GetComponent<InventoryItemButton>().Inventory = _currentCharacterEquipment;
            }
        }
    }

    void RemoveItemUI(InventoryItem item)
    {
        if (item == null)
            return;

        if (item.Item.ItemType == ItemType.eARMOUR)
        {
            ArmourItemData armourItem = (ArmourItemData)item.Item;
            switch (armourItem.ArmourSlotType)
            {
                case ArmourItemData.SlotType.eHEAD:
                    HeadButton.image.sprite = DefaultPreviewSprite;
                    HeadButton.GetComponent<InventoryItemButton>().Item = null;
                    break;
                case ArmourItemData.SlotType.eCHEST:
                    BodyButton.image.sprite = DefaultPreviewSprite;
                    BodyButton.GetComponent<InventoryItemButton>().Item = null;
                    break;
                case ArmourItemData.SlotType.eLEGS:
                    LegsButton.image.sprite = DefaultPreviewSprite;
                    LegsButton.GetComponent<InventoryItemButton>().Item = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        else if (item.Item.ItemType == ItemType.eWEAPON)
        {
            WeaponItemData weaponItem = (WeaponItemData)item.Item;
            if (weaponItem.IsMainHand)
            {
                MainHandButton.image.sprite = DefaultPreviewSprite;
                MainHandButton.GetComponent<InventoryItemButton>().Item = null;
            }
            else
            {
                OffHandButton.image.sprite = DefaultPreviewSprite;
                OffHandButton.GetComponent<InventoryItemButton>().Item = null;
            }
        }
    }
}

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
        _currentCharacterEquipment = FindObjectOfType<CharacterInventory>().CurrentCharacterEquipment;
        _currentCharacterEquipment.OnItemAdd += AddUIItem;
        _currentCharacterEquipment.OnItemRemove += RemoveItemUI;

        //get party inventory
        _partyInventory = FindObjectOfType<PartyInventory>().Inventory;

        //On button Click
        HeadButton.onClick.AddListener(() => HeadButton.GetComponent<ItemButton>().TransferItem(_partyInventory));
        BodyButton.onClick.AddListener(() => BodyButton.GetComponent<ItemButton>().TransferItem(_partyInventory));
        LegsButton.onClick.AddListener(() => LegsButton.GetComponent<ItemButton>().TransferItem(_partyInventory));
        MainHandButton.onClick.AddListener(() => MainHandButton.GetComponent<ItemButton>().TransferItem(_partyInventory));
        OffHandButton.onClick.AddListener(() => OffHandButton.GetComponent<ItemButton>().TransferItem(_partyInventory));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void AddUIItem(InventoryItem item)
    {
        if (item.Item.ItemType == ItemType.eARMOUR)
        {
            ArmourItemData armourItem = (ArmourItemData) item.Item;
            switch (armourItem.ArmourType)
            {
                case ArmourItemData.Type.eHEAD:
                    HeadButton.image.sprite = armourItem.PreviewSprite;
                    HeadButton.GetComponent<ItemButton>().Item = item;
                    HeadButton.GetComponent<ItemButton>().Inventory = _currentCharacterEquipment;
                    break;
                case ArmourItemData.Type.eCHEST:
                    BodyButton.image.sprite = armourItem.PreviewSprite;
                    BodyButton.GetComponent<ItemButton>().Item = item;
                    BodyButton.GetComponent<ItemButton>().Inventory = _currentCharacterEquipment;
                    break;
                case ArmourItemData.Type.eLEGS:
                    LegsButton.image.sprite = armourItem.PreviewSprite;
                    LegsButton.GetComponent<ItemButton>().Item = item;
                    LegsButton.GetComponent<ItemButton>().Inventory = _currentCharacterEquipment;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        else if (item.Item.ItemType == ItemType.eWEAPON || item.Item.ItemType == ItemType.eMAGICWEAPON)
        {
            WeaponItemData weaponItem = (WeaponItemData) item.Item;
            if (weaponItem.IsMainHand)
            {
                MainHandButton.image.sprite = weaponItem.PreviewSprite;
                MainHandButton.GetComponent<ItemButton>().Item = item;
                MainHandButton.GetComponent<ItemButton>().Inventory = _currentCharacterEquipment;
            }
            else
            {
                OffHandButton.image.sprite = weaponItem.PreviewSprite;
                OffHandButton.GetComponent<ItemButton>().Item = item;
                OffHandButton.GetComponent<ItemButton>().Inventory = _currentCharacterEquipment;
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
            switch (armourItem.ArmourType)
            {
                case ArmourItemData.Type.eHEAD:
                    HeadButton.image.sprite = DefaultPreviewSprite;
                    HeadButton.GetComponent<ItemButton>().Item = null;
                    break;
                case ArmourItemData.Type.eCHEST:
                    BodyButton.image.sprite = DefaultPreviewSprite;
                    BodyButton.GetComponent<ItemButton>().Item = null;
                    break;
                case ArmourItemData.Type.eLEGS:
                    LegsButton.image.sprite = DefaultPreviewSprite;
                    LegsButton.GetComponent<ItemButton>().Item = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        else if (item.Item.ItemType == ItemType.eWEAPON || item.Item.ItemType == ItemType.eMAGICWEAPON)
        {
            WeaponItemData weaponItem = (WeaponItemData)item.Item;
            if (weaponItem.IsMainHand)
            {
                MainHandButton.image.sprite = DefaultPreviewSprite;
                MainHandButton.GetComponent<ItemButton>().Item = null;
            }
            else
            {
                OffHandButton.image.sprite = DefaultPreviewSprite;
                OffHandButton.GetComponent<ItemButton>().Item = null;
            }
        }
    }
}

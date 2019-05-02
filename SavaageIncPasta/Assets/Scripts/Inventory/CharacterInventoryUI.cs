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
    public HealthBar HealthBar;
    public ManaBar ManaBar;
    public ComfortBar ComfortBar;

    public Sprite DefaultPreviewSprite;

    private PlayerManager _playerManager;
    private Inventory _partyInventory;
    private int _currentCharacterIndex = 0;

    private void Awake()
    {
        _playerManager = FindObjectOfType<PlayerManager>();

    }
    // Use this for initialization
    void Start ()
    {
        if(_playerManager == null)
        {
            Debug.LogWarning("Character Inventory couldn't find Player manager");
            return;
        }
        else if(_playerManager.Characters.Count == 0)
        {
            return;
        }

        //get party inventory
        _partyInventory = FindObjectOfType<PartyInventory>().Inventory;

        //On button Click
        HeadButton.onClick.AddListener(() => HeadButton.GetComponent<InventoryItemButton>().TransferItem(_partyInventory));
        BodyButton.onClick.AddListener(() => BodyButton.GetComponent<InventoryItemButton>().TransferItem(_partyInventory));
        LegsButton.onClick.AddListener(() => LegsButton.GetComponent<InventoryItemButton>().TransferItem(_partyInventory));
        MainHandButton.onClick.AddListener(() => MainHandButton.GetComponent<InventoryItemButton>().TransferItem(_partyInventory));
        OffHandButton.onClick.AddListener(() => OffHandButton.GetComponent<InventoryItemButton>().TransferItem(_partyInventory));

        _playerManager.Characters[_currentCharacterIndex].Equipment.OnItemAdd += AddUIItem;
        _playerManager.Characters[_currentCharacterIndex].Equipment.OnItemRemove += RemoveItemUI;

        HealthBar.Character = _playerManager.Characters[_currentCharacterIndex];
        ManaBar.Character = _playerManager.Characters[_currentCharacterIndex];
        ComfortBar.Character = _playerManager.Characters[_currentCharacterIndex];

        SyncEquipment();
    }

    void OnDisable()
    {
        if (_playerManager.Characters.Count == 0)
        {
            return;
        }

        if (_playerManager.Characters[_currentCharacterIndex].Equipment != null)
        {
            _playerManager.Characters[_currentCharacterIndex].Equipment.OnItemAdd -= AddUIItem;
            _playerManager.Characters[_currentCharacterIndex].Equipment.OnItemRemove -= RemoveItemUI;
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
        if (_playerManager == null || _playerManager.Characters == null || _playerManager.Characters.Count == 0 || _playerManager.Characters[_currentCharacterIndex].Equipment == null)
        {
            Debug.LogWarning("No chracters in party");
            return;
        }

        CharacterName.text = _playerManager.Characters[_currentCharacterIndex].Name;
        CharacterPreview.sprite = FindObjectOfType<SpriteManager>().GetSprite(_playerManager.Characters[_currentCharacterIndex].SpritePreviewName);

        _playerManager.Characters[_currentCharacterIndex].Equipment.OnItemAdd += AddUIItem;
        _playerManager.Characters[_currentCharacterIndex].Equipment.OnItemRemove += RemoveItemUI;
        SyncEquipment();
    }

    private void Update()
    {
        if (Input.GetButtonDown("LB") && _playerManager.Characters.Count > 0)
        {
            _currentCharacterIndex = Mathf.Clamp(_currentCharacterIndex-1, 0, _playerManager.Characters.Count-1);
            changeCharacter(_currentCharacterIndex);
        }
        else if (Input.GetButtonDown("RB") && _playerManager.Characters.Count > 0)
        {
            _currentCharacterIndex = Mathf.Clamp(_currentCharacterIndex+1, 0, _playerManager.Characters.Count - 1);
            changeCharacter(_currentCharacterIndex);
        }
    }

    public CharacterEquipment GetCharacterEquipment()
    {
        return _playerManager.Characters[_currentCharacterIndex].Equipment;
    }

    void SyncEquipment()
    {
        HeadButton.image.sprite = DefaultPreviewSprite;
        HeadButton.GetComponent<InventoryItemButton>().Item = null;
        LegsButton.image.sprite = DefaultPreviewSprite;
        LegsButton.GetComponent<InventoryItemButton>().Item = null;
        BodyButton.image.sprite = DefaultPreviewSprite;
        BodyButton.GetComponent<InventoryItemButton>().Item = null;
        MainHandButton.image.sprite = DefaultPreviewSprite;
        MainHandButton.GetComponent<InventoryItemButton>().Item = null;
        OffHandButton.image.sprite = DefaultPreviewSprite;
        OffHandButton.GetComponent<InventoryItemButton>().Item = null;
        foreach (var item in _playerManager.Characters[_currentCharacterIndex].Equipment.GetItems())
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
                    HeadButton.GetComponent<InventoryItemButton>().Inventory = _playerManager.Characters[_currentCharacterIndex].Equipment;
                    break;
                case ArmourItemData.SlotType.eCHEST:
                    BodyButton.image.sprite = armourItem.PreviewSprite;
                    BodyButton.GetComponent<InventoryItemButton>().Item = item;
                    BodyButton.GetComponent<InventoryItemButton>().Inventory = _playerManager.Characters[_currentCharacterIndex].Equipment;
                    break;
                case ArmourItemData.SlotType.eLEGS:
                    LegsButton.image.sprite = armourItem.PreviewSprite;
                    LegsButton.GetComponent<InventoryItemButton>().Item = item;
                    LegsButton.GetComponent<InventoryItemButton>().Inventory = _playerManager.Characters[_currentCharacterIndex].Equipment;
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
                MainHandButton.GetComponent<InventoryItemButton>().Inventory = _playerManager.Characters[_currentCharacterIndex].Equipment;
            }
            else
            {
                OffHandButton.image.sprite = weaponItem.PreviewSprite;
                OffHandButton.GetComponent<InventoryItemButton>().Item = item;
                OffHandButton.GetComponent<InventoryItemButton>().Inventory = _playerManager.Characters[_currentCharacterIndex].Equipment;
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

    void changeCharacter(int index)
    {
        _playerManager.Characters[_currentCharacterIndex].Equipment.OnItemAdd -= AddUIItem;
        _playerManager.Characters[_currentCharacterIndex].Equipment.OnItemRemove -= RemoveItemUI;

        //change player
        _currentCharacterIndex = index;
        var currentCharacter = _playerManager.Characters[_currentCharacterIndex];
        _playerManager.Characters[_currentCharacterIndex].Equipment = currentCharacter.Equipment;
        currentCharacter.Equipment.OnItemAdd += AddUIItem;
        currentCharacter.Equipment.OnItemRemove += RemoveItemUI;
        CharacterName.text = currentCharacter.Name;
        CharacterPreview.sprite = FindObjectOfType<SpriteManager>().GetSprite(currentCharacter.SpritePreviewName);

        HealthBar.Character = currentCharacter;
        ManaBar.Character = currentCharacter;
        ComfortBar.Character = currentCharacter;


        SyncEquipment();
    }
}

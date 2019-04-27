/* Attach to ClanMenu */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClanControl : MonoBehaviour {

    public CharacterComparison CharacterCompare;
    public GameObject FirstSelected;
    public Character SelectedCharacter;
    public int SelectedColumn = 1;
    public Text ColumnText;


    private List<SelectCharacter> clanMember;
    private ClanManager _clanManager;

    [SerializeField]
    private GameObject buttonTemplate;
    [SerializeField]
    private GridLayoutGroup gridGroup;

    [SerializeField]
    private Sprite[] characterSprite; // Character Sprite array


    private void Start()
    {
        _clanManager = FindObjectOfType<ClanManager>();
        GenClan();
    }

    private void OnEnable()
    {
        if(FirstSelected != null)
        {
            var eventSystem = FindObjectOfType<EventSystem>();
            eventSystem.SetSelectedGameObject(FirstSelected);
        }
    }

    void GenClan()
    {
        // If number of clan members less than 5, then constraint count of clanMember
        if (_clanManager.SpareCharacterPool.Count < 5)
        {
            gridGroup.constraintCount = _clanManager.SpareCharacterPool.Count;
        }
        else
        {
            // Set column to 4
            gridGroup.constraintCount = 4;
        }

        foreach (Character newCharacter in _clanManager.SpareCharacterPool)
        {
            GameObject newButton = Instantiate(buttonTemplate) as GameObject;
            newButton.SetActive(true);

            ClanButton characterButton = newButton.AddComponent<ClanButton>();
            characterButton.Character = newCharacter; // Add new character
            characterButton.CharacterCompare = CharacterCompare; // Add character compare
            newButton.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<SpriteManager>().GetSprite(newCharacter.SpritePreviewName);

            // Add set to compare mode to newbutton when on click
            newButton.GetComponent<Button>().onClick.AddListener(characterButton.SetToCompareMode);

            newButton.transform.SetParent(gridGroup.transform, false);
        }

    }

    public struct SelectCharacter
    {
        public Sprite characterSprite;
    }

    private void OnRectTransformDimensionsChange()
    {
        //rsize button to fit
        float width = RectTransformUtility.PixelAdjustRect(GetComponent<RectTransform>(), FindObjectOfType<Canvas>()).width - 32;
        Vector2 newSize = new Vector2(width / 4, width / 4);
        gridGroup.cellSize = newSize;
    }

    public void ChangeColumnOfSelectedCharacter()
    {
        SelectedCharacter.CurrCol = SelectedColumn;
        SelectedCharacter = null;

    }

    public void IncreaseColumn()
    {
        SelectedColumn = Mathf.Clamp(SelectedColumn + 1, 1, 3);
        ColumnText.text = SelectedColumn.ToString();
    }


    public void DecreaseColumn()
    {
        SelectedColumn = Mathf.Clamp(SelectedColumn - 1, 1, 3);
        ColumnText.text = SelectedColumn.ToString();

    }
}

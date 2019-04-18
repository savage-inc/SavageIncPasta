using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class BarracksControl : MonoBehaviour
{
    public CharacterComparison CharacterCompare;
    public GameObject FirstSelected;

    private List<SelectCharacter> Characters;
    private BarracksManager _barracksManager;

    [SerializeField]
    private GameObject buttonTemplate;
    [SerializeField]
    private GridLayoutGroup gridGroup;

    // Use this for initialization
 //   void Start ()
 //   {
 //       _randomCharacter = GenerateRandomCharacter.GenerateCharacter();
 //       GenBarracks();
	//}

    private void OnEnable()
    {
        if (FirstSelected != null)
        {
            var eventSystem = FindObjectOfType<EventSystem>();
            eventSystem.SetSelectedGameObject(FirstSelected);
        }
    }

    void GenBarracks()
    {
        // We need to randomise characters to be able to be bought
        // Need them to be compared to current party
        // Need an if statement that if party is full go to clan
        // We need to be able to display the following in the UI the character, the price, 
        // the party gold 



        //If number of clan members less than 5, then constraint count of clanMember
        if (_barracksManager.RandomCharacterPool.Count < 5)
        {
            gridGroup.constraintCount = _barracksManager.RandomCharacterPool.Count;
        }
        else
        {
            // Set column to 4
            gridGroup.constraintCount = 4;
        }

        foreach (Character newCharacter in _barracksManager.RandomCharacterPool)
        {
            GameObject newButton = Instantiate(buttonTemplate) as GameObject;
            newButton.SetActive(true);

            BuyButton buyButton = newButton.AddComponent<BuyButton>();
            buyButton.Character = newCharacter; // Add new character
            buyButton.CharacterCompare = CharacterCompare; // Add character compare
            newButton.transform.GetChild(0).GetComponent<Image>().sprite = FindObjectOfType<SpriteManager>().GetSprite(newCharacter.SpritePreviewName);

            // Add set to compare mode to newbutton when on click
            newButton.GetComponent<Button>().onClick.AddListener(buyButton.Buy);

            newButton.transform.SetParent(gridGroup.transform, false);
        }
    }

    public struct SelectCharacter
    {
        public Sprite characterSprite;
    }

}

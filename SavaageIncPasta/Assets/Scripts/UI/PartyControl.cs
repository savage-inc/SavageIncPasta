/* Attach to ClanMenu */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyControl : MonoBehaviour {

    private List<SelectCharacter> clanMember;

    [SerializeField]
    private GameObject buttonTemplate;
    [SerializeField]
    private GridLayoutGroup gridGroup;

    [SerializeField]
    private Sprite[] characterSprite; // Character Sprite array

    private void Start()
    {
        // clan member array to select character
        clanMember = new List<SelectCharacter>();

        // Loop through 20 times
        for (int i = 0; i < 20; i++)
        {
            SelectCharacter newCharacter;
            newCharacter.characterSprite = characterSprite[Random.Range(0, characterSprite.Length)];

            clanMember.Add(newCharacter);
        }

        GenParty();
    }

    void GenParty()
    {
        // If number of clan members less than 6, then constraint count of clanMember
        if (clanMember.Count < 6)
        {
            gridGroup.constraintCount = clanMember.Count;
        }
        else
        {
            // Set column to 5
            gridGroup.constraintCount = 5;
        }

        foreach (SelectCharacter newCharacter in clanMember)
        {
            GameObject newButton = Instantiate(buttonTemplate) as GameObject;
            newButton.SetActive(true);

            newButton.GetComponent<PartyButton>().SetIcon(newCharacter.characterSprite);
            newButton.transform.SetParent(transform.GetChild(0).transform.GetChild(0), false);
        }
    }

    public struct SelectCharacter
    {
        public Sprite characterSprite;
    }
}

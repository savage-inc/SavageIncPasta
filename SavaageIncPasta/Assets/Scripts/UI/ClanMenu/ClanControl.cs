/* Attach to ClanMenu */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClanControl : MonoBehaviour {

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

    void GenClan()
    {
        // If number of clan members less than 6, then constraint count of clanMember
        if (_clanManager.SpareCharacterPool.Count < 6)
        {
            gridGroup.constraintCount = _clanManager.SpareCharacterPool.Count;
        }
        else
        {
            // Set column to 5
            gridGroup.constraintCount = 5;
        }

        foreach (Character newCharacter in _clanManager.SpareCharacterPool)
        {
            GameObject newButton = Instantiate(buttonTemplate) as GameObject;
            newButton.SetActive(true);

            ClanButton characterButton = newButton.AddComponent<ClanButton>();
            characterButton.Character = newCharacter;

            newButton.transform.SetParent(transform.GetChild(0).transform.GetChild(0), false);
        }
    }

    public struct SelectCharacter
    {
        public Sprite characterSprite;
    }
}

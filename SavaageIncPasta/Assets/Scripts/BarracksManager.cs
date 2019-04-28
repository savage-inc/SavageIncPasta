using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracksManager : MonoBehaviour
{

    public List<Character> RandomCharacterPool;
    public List<int> Prices;
    public bool RandomCharacter;
    private PartyInventory _partyInventory;

    //Gerenate characters to be bought
    private void Awake()
    {
        _partyInventory = FindObjectOfType<PartyInventory>();
        RandomCharacterPool = new List<Character>();
        if (RandomCharacter)
        {
            for (int i = 0; i < 4; i++)
            {
                var character = GenerateRandomCharacter.GenerateCharacter();
                RandomCharacterPool.Add(character);
                Prices.Add(Random.Range(15,21));

            }
        }
    }

    public void AddToParty(Character PartyMember)
    {
        FindObjectOfType<PlayerManager>().AddCharacter(PartyMember);
    }

    public void AddToClan(Character ClanMember)
    {
        FindObjectOfType<ClanManager>().AddCharacter(ClanMember);
    }

    // Buy Character, remove character from barracks add character to party if party is full 
    // add character to clan
    public void BuyCharacter(int index)
    {
        if (_partyInventory.Gold - Prices[index] >= 0)
        {
           
            if (FindObjectOfType<PlayerManager>().Characters.Count < 4)
            {
                AddToParty(RandomCharacterPool[index]);
            }
            else
            {
                AddToClan(RandomCharacterPool[index]);
            }

            _partyInventory.Gold -= Prices[index];

            //generate new character to replace the old one
            RandomCharacterPool[index] = GenerateRandomCharacter.GenerateCharacter();
            Prices[index] = Random.Range(15, 21);
        }
    }
}
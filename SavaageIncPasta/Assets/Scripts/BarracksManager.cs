using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracksManager : MonoBehaviour
{

    public List<Character> RandomCharacterPool;
    public bool RandomCharacter;

    //Gerenate characters to be bought
    private void Awake()
    {
        RandomCharacterPool = new List<Character>();
        if (RandomCharacter)
        {
            for (int i = 0; i < 4; i++)
            {
                var character = GenerateRandomCharacter.GenerateCharacter();
                RandomCharacterPool.Add(character);
            }
        }
    }

    public void AddCharacter(Character c)
    {
        RandomCharacterPool.Add(c);
    }
    
    public void RemoveCharacter(Character c)
    {
        RandomCharacterPool.Remove(c);
    }

    public void AddToBarracks(Character BarracksMember)
    {
        FindObjectOfType<PlayerManager>().AddCharacter(BarracksMember);
        RemoveCharacter(BarracksMember);
    }

    public void RemoveFromBarracks(Character BarracksMember)
    {
        AddCharacter(BarracksMember);
        FindObjectOfType<PlayerManager>().RemoveCharacter(BarracksMember);
    }

    public void AddToParty(Character PartyMember)
    {
        FindObjectOfType<PlayerManager>().AddCharacter(PartyMember);
        RemoveCharacter(PartyMember);
    }

    public void AddToClan(Character ClanMember)
    {
        FindObjectOfType<PlayerManager>().AddCharacter(ClanMember);
        RemoveCharacter(ClanMember);
    }

    // Buy Character, remove character from barracks add character to party if party is full 
    //add character to clan
    public void BuyCharacter(Character BarracksMember)
    {
        RemoveFromBarracks(BarracksMember);

        if (FindObjectOfType<PlayerManager>().Characters.Count < 4)
        {
            AddToParty(BarracksMember);
        }
        else
        {
            AddToClan(BarracksMember);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClanManager : MonoBehaviour
{
    public List<Character> SpareCharacterPool;
    List<Character> _party;


    private void Awake()
    {
        _party = FindObjectOfType<PlayerManager>().Characters;
    }
    public void AddCharacter(Character c)
    {
        SpareCharacterPool.Add(c);
    }

    public void RemoveCharacter(Character c)
    {
        SpareCharacterPool.Remove(c);
    }

    public void SwapCharacters(Character PartyMember, Character ClanMember)
    {
        RemoveFromParty(PartyMember);
        AddToParty(ClanMember);
    }

    public void RemoveFromParty(Character PartyMember)
    {
        AddCharacter(PartyMember);
        FindObjectOfType<PlayerManager>().RemoveCharacter(PartyMember);
    }

    public void AddToParty(Character ClanMember)
    {
        FindObjectOfType<PlayerManager>().AddCharacter(ClanMember);
        RemoveCharacter(ClanMember);
    }


}

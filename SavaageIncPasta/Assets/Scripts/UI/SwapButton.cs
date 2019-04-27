using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapButton : MonoBehaviour
{
    public CharacterComparison PartyMember;
    public CharacterComparison ClanMember;
    public ClanManager clan;

    public void SwapCharacters()
    {
        if (PartyMember.character == null && ClanMember.character != null && FindObjectOfType<PlayerManager>().Characters.Count < 4)
        {
            clan.AddToParty(ClanMember.character);
        }
        else if (ClanMember.character == null && PartyMember.character != null)
        {
            clan.RemoveFromParty(PartyMember.character);
        }
        else if (ClanMember.character != null && PartyMember.character != null)
        {
            clan.SwapCharacters(PartyMember.character, ClanMember.character);
        }
    }

}

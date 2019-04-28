using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilltyButton : MonoBehaviour {

    public int AbilityID = 1;
    private Battle _battle;
    private BattleInfoUI _info;

    private void Awake()
    {
        _battle = FindObjectOfType<Battle>();
        _info = FindObjectOfType<BattleInfoUI>();
    }
    
    public void DisplayAbility()
    {
        //get current selectedcharacter
        var player = _battle.GetCurrentPlayer();
        //get ability 
        var ability = AbilityManager.Instance.GetAbility(player.Character.Class, AbilityID);

        _info.Ability = ability;
    }

    public void CloseAbility()
    {
        _info.Ability = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilltyButton : MonoBehaviour {

    public AbilityData Ability;
    private Battle _battle;
    private BattleInfoUI _info;

    private void Awake()
    {
        _battle = FindObjectOfType<Battle>();
        _info = FindObjectOfType<BattleInfoUI>();
    }
    
    public void DisplayAbility()
    {
        _info.Ability = Ability;
    }

    public void CloseAbility()
    {
        _info.Ability = null;
        _info.Clear();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeSelectedToAliveEnemy : MonoBehaviour {
    private Battle _battle;
    private EventSystem _eventSystem;

    void Awake()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
        _battle = FindObjectOfType<Battle>();
    }

    public void ChangeSelected(int abilityID)
    {
        if (_battle != null)
        {
            if (abilityID == 0)
            {
                _eventSystem.SetSelectedGameObject(_battle.GetFirstAliveEnemy().gameObject);
            }
            else
            {
                //get current selectedcharacter
                var player = _battle.GetCurrentPlayer();
                var ability = AbilityManager.Instance.GetAbility(player.Character.Class, abilityID);

                if (ability.RequiresTarget)
                {
                    _eventSystem.SetSelectedGameObject(_battle.GetFirstAliveEnemy().gameObject);
                }
            }
        }
    }
}

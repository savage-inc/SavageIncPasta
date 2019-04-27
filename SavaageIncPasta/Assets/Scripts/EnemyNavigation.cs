using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyNavigation : MonoBehaviour
{
    public BattleCharacter AttackingPlayer;
    private Battle _battle;
    private List<BattleCharacter> _selectableEnemies;
    private List<Button> _buttons;

    private void Awake()
    {
        _battle = FindObjectOfType<Battle>();
        _selectableEnemies = new List<BattleCharacter>();
        _buttons = new List<Button>();
        foreach (Transform child in transform)
        {
            _buttons.Add(child.GetComponent<Button>());
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void GetEnemies()
    {
        _selectableEnemies = _battle.GetSelectableEnemiesFromPlayer(AttackingPlayer);
    }

    void clearNavigations()
    {
        foreach (var button in _buttons)
        {
        }
    }

    void setupNaviagation()
    {

    }
}

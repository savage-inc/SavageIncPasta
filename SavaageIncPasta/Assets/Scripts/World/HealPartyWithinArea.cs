using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class HealPartyWithinArea : MonoBehaviour {

    private PlayerManager _playerManager;
	// Use this for initialization
	void Awake () {
        GetComponent<BoxCollider2D>().isTrigger = true;
        _playerManager = FindObjectOfType<PlayerManager>();

    }

    // Update is called once per frame
    void Update ()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var character in _playerManager.Characters)
        {
            if (character != null)
            {
                character.ChangeHealth(character.MaxHealth);
                character.ChangeMana(character.MaxMana);
            }
        }
    }
}

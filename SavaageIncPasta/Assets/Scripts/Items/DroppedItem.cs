using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DroppedItem : MonoBehaviour {

    public BaseItem Item;

    private SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public string Name;
	// Use this for initialization
	void Start ()
    {
        Name = Item.ItemData.Name;
        _spriteRenderer.sprite = Item.ItemData.PreviewSprite;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

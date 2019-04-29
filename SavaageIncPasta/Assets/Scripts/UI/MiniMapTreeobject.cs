using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapTreeobject : MonoBehaviour {
    private static SpriteRenderer spriteRender;
    static int VertexCount = Mathf.Clamp(3, 3, 10);
    public Vector2[] vertices = new Vector2[VertexCount];
    private void Awake()
    {
        spriteRender = gameObject.GetComponent<SpriteRenderer>();
        Sprite sprite = new Sprite();
        Texture2D texture = new Texture2D(100, 100);
        sprite = Sprite.Create(texture, new Rect(0, 0, 100, 100),new Vector2(50,50));
        spriteRender.sprite = sprite;
        for (int i = 0; i < VertexCount; i++)
        {
            sprite.vertices[i] = vertices[i];
            print(sprite.vertices[i]);

        }
    }
    // Use this for initialization
    void Start () {


    }
    
    

    
	
	// Update is called once per frame
	void Update () {
		
	}
}

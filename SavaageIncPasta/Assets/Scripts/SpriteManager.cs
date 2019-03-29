using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpriteManager : MonoBehaviour {

    [System.Serializable]
    private struct SpriteContainer
    {
        public string Name;
        public Sprite Sprite;
    }

    private static SpriteManager _instance;
    [SerializeField]
    private List<SpriteContainer> _sprites;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);

    }

    public Sprite GetSprite(string spriteName)
    {
        foreach(var sprite in _sprites)
        {
            if(sprite.Name == spriteName)
            {
                return sprite.Sprite;
            }
        }
        return null;
    }
}

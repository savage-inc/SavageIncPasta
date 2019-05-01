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
    [SerializeField]
    private List<SpriteContainer> _swords;
    [SerializeField]
    private List<SpriteContainer> _bows;
    [SerializeField]
    private List<SpriteContainer> _daggers;
    [SerializeField]
    private List<SpriteContainer> _spears;
    [SerializeField]
    private List<SpriteContainer> _stars;
    [SerializeField]
    private List<SpriteContainer> _chest;
    [SerializeField]
    private List<SpriteContainer> _legs;
    [SerializeField]
    private List<SpriteContainer> _helmet;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            LoadItemIcons();
        }
        else
        {
            Destroy(gameObject);
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

    public void LoadItemIcons()
    {
        //load swords
        Sprite[] swordSprites = Resources.LoadAll<Sprite>("Sprites/Items/weapons/swords");
        foreach (var sprite in swordSprites)
        {
            SpriteContainer spriteContainer = new SpriteContainer();
            spriteContainer.Name = sprite.name;
            spriteContainer.Sprite = sprite;
            _sprites.Add(spriteContainer);
            _swords.Add(spriteContainer);
        }

        Sprite[] bowSprites = Resources.LoadAll<Sprite>("Sprites/Items/weapons/bow");
        foreach (var sprite in bowSprites)
        {
            SpriteContainer spriteContainer = new SpriteContainer();
            spriteContainer.Name = sprite.name;
            spriteContainer.Sprite = sprite;
            _sprites.Add(spriteContainer);
            _bows.Add(spriteContainer);
        }

        Sprite[] daggerSprites = Resources.LoadAll<Sprite>("Sprites/Items/weapons/daggers");
        foreach (var sprite in daggerSprites)
        {
            SpriteContainer spriteContainer = new SpriteContainer();
            spriteContainer.Name = sprite.name;
            spriteContainer.Sprite = sprite;
            _sprites.Add(spriteContainer);
            _daggers.Add(spriteContainer);
        }

        Sprite[] starSprites = Resources.LoadAll<Sprite>("Sprites/Items/weapons/stars");
        foreach (var sprite in starSprites)
        {
            SpriteContainer spriteContainer = new SpriteContainer();
            spriteContainer.Name = sprite.name;
            spriteContainer.Sprite = sprite;
            _sprites.Add(spriteContainer);
            _stars.Add(spriteContainer);
        }

        Sprite[] spearSprites = Resources.LoadAll<Sprite>("Sprites/Items/weapons/spears");
        foreach (var sprite in spearSprites)
        {
            SpriteContainer spriteContainer = new SpriteContainer();
            spriteContainer.Name = sprite.name;
            spriteContainer.Sprite = sprite;
            _sprites.Add(spriteContainer);
            _spears.Add(spriteContainer);
        }

        Sprite[] hemletSprites = Resources.LoadAll<Sprite>("Sprites/Items/armour/helmet");
        foreach (var sprite in hemletSprites)
        {
            SpriteContainer spriteContainer = new SpriteContainer();
            spriteContainer.Name = sprite.name;
            spriteContainer.Sprite = sprite;
            _sprites.Add(spriteContainer);
            _helmet.Add(spriteContainer);
        }

        Sprite[] bootSprites = Resources.LoadAll<Sprite>("Sprites/Items/armour/boots");
        foreach (var sprite in bootSprites)
        {
            SpriteContainer spriteContainer = new SpriteContainer();
            spriteContainer.Name = sprite.name;
            spriteContainer.Sprite = sprite;
            _sprites.Add(spriteContainer);
            _legs.Add(spriteContainer);
        }

        Sprite[] chestSprites = Resources.LoadAll<Sprite>("Sprites/Items/armour/chest");
        foreach (var sprite in chestSprites)
        {
            SpriteContainer spriteContainer = new SpriteContainer();
            spriteContainer.Name = sprite.name;
            spriteContainer.Sprite = sprite;
            _sprites.Add(spriteContainer);
            _chest.Add(spriteContainer);
        }
    }

    public Sprite GetRandomWeaponIcon(WeaponItemData.SubType weapoonType)
    {
        switch (weapoonType)
        {
            case WeaponItemData.SubType.eSHORTSWORD:
                return _swords[Random.Range(0, _swords.Count)].Sprite;
            case WeaponItemData.SubType.eLONGSWORD:
                return _swords[Random.Range(0, _swords.Count)].Sprite;
            case WeaponItemData.SubType.eWHIP:
                return _spears[Random.Range(0, _spears.Count)].Sprite;
            case WeaponItemData.SubType.eRAPIER:
                return _swords[Random.Range(0, _swords.Count)].Sprite;
            case WeaponItemData.SubType.eTHROWINGSTAR:
                return _stars[Random.Range(0, _stars.Count)].Sprite;
            case WeaponItemData.SubType.eBOW:
                return _bows[Random.Range(0, _bows.Count)].Sprite;
            case WeaponItemData.SubType.ePISTOL:
                return _bows[Random.Range(0, _bows.Count)].Sprite; ;
            case WeaponItemData.SubType.eRIFLE:
                return _bows[Random.Range(0, _bows.Count)].Sprite; ;
            default:
                return _swords[0].Sprite;
        }

    }

    public Sprite GetRandomArmourIcon(ArmourItemData.SlotType slotType)
    {
        switch (slotType)
        {
            case ArmourItemData.SlotType.eHEAD:
                return _helmet[Random.Range(0, _helmet.Count)].Sprite;
            case ArmourItemData.SlotType.eCHEST:
                return _chest[Random.Range(0, _chest.Count)].Sprite;
            case ArmourItemData.SlotType.eLEGS:
                return _legs[Random.Range(0, _legs.Count)].Sprite;
            default:
                return _helmet[0].Sprite;
        }

    }

}

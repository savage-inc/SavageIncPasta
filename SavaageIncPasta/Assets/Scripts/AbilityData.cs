using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Ability", order = 1)]
public class AbilityData : ScriptableObject
{
    [SerializeField]
    private Sprite _sprite;
    [SerializeField]
    private string _abilityName;
    [SerializeField]
    [TextArea(5, 10)]
    private string _abilityDescription;
    [SerializeField]
    private int _manaCost;
    [SerializeField]
    private int _level = 1;
    [SerializeField]
    private ClassType _class;
    [SerializeField]
    private bool _target = true;
    [SerializeField]
    private int _tree = 1;
    [SerializeField]
    private int _id = 1;
    [SerializeField]
    private GameObject _projectile;

    public Sprite Sprite
    {
        get
        {
            return _sprite;
        }
    }

    public string AbilityName
    {
        get
        {
            return _abilityName;
        }
    }

    public string AbilityDescription
    {
        get
        {
            return _abilityDescription;
        }
    }

    public int ManaCost
    {
        get
        {
            return _manaCost;
        }
    }

    public int Level
    {
        get
        {
            return _level;
        }
    }

    public ClassType Class
    {
        get
        {
            return _class;
        }
    }

    public bool RequiresTarget
    {
        get
        {
            return _target;
        }
    }

    public int Tree
    {
        get
        {
            return _tree;
        }
    }

    public int ID
    {
        get
        {
            return _id;
        }
    }

    public GameObject Projectile
    {
        get
        {
            return _projectile;
        }
    }
}

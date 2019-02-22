using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Items/ArmourItem", order = 1)]
[System.Serializable]
public class ArmourItemData : BaseItemData
{
    public enum Type
    {
        eHEAD,
        eCHEST,
        eLEGS,
    }

    public ArmourItemData()
    {
        base._itemType = ItemType.eARMOUR;
    }

    [SerializeField]
    private Type _armourType;

    [SerializeField]
    private float _modifier;

    public Type ArmourType
    {
        get { return _armourType; }
    }

    public float Modifier
    {
        get { return _modifier; }
    }
}
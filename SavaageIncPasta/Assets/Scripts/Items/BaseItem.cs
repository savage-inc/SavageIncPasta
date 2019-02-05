using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    eCONSUMABLE,
    eARMOUR,
    eWEAPON
}

public abstract class BaseItem : ScriptableObject {
    public string Item;
    public string Description;

    protected ItemType ItemType; //Type of base item
}

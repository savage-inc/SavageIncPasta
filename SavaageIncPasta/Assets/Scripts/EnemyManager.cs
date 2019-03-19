using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyList
{
    public List<Character> Enemies;
}

public class EnemyManager : MonoBehaviour
{
    public List<EnemyList> EnemyGroups;
}

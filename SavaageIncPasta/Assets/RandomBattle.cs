using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Enemies
{
    public string name;
    public int weight;
}

public class RandomBattle : MonoBehaviour
{
    public int MinStepsBeforeBattle;
    public int MaxStepsBeforeBattle;
    private readonly System.Random _randChance = new System.Random();
    private int _battleTriggerCounter;
    private int _stepCounter = 0;
    private bool _isColliding = false;
    private Rigidbody2D _rb;
    private Vector2 _oldPos;
    private readonly float _range = 2.5f;
    private int _totalWeights = 0;

    //public List<string> Enemies = new List<string>();
    public List<Enemies> Enemies = new List<Enemies>();

    // Use this for initialization
    void Start()
    {
        _rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        _battleTriggerCounter = _randChance.Next(MinStepsBeforeBattle, MaxStepsBeforeBattle + 1);
        _oldPos = _rb.position;

        foreach (Enemies enemies in Enemies)
        {
            _totalWeights += enemies.weight;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CapValuesForMinAndMaxSteps();

        if ((_oldPos.sqrMagnitude <= _rb.position.sqrMagnitude - _range || _oldPos.sqrMagnitude >= _rb.position.sqrMagnitude + _range) && _isColliding)
        {
            _oldPos = _rb.position;
            _stepCounter++;
        }
        
        if (_stepCounter >= _battleTriggerCounter)
        {
            _battleTriggerCounter = _randChance.Next(MinStepsBeforeBattle, MaxStepsBeforeBattle + 1);
            _stepCounter = 0;

            int randomEnemy = _randChance.Next(_totalWeights+1);
            int weight = 0;
            foreach (Enemies enemy in Enemies)
            {
                weight += enemy.weight;
                if (weight >= randomEnemy)
                {
                    Debug.Log(enemy.name + " probability: " + (float)enemy.weight / (float)_totalWeights+1);
                    break;
                }
            }
        }
    }

    private void CapValuesForMinAndMaxSteps()
    {
        if (MinStepsBeforeBattle < 1)
            MinStepsBeforeBattle = 1;

        if (MaxStepsBeforeBattle < 1)
            MaxStepsBeforeBattle = 10;

        if (MinStepsBeforeBattle > MaxStepsBeforeBattle)
            MaxStepsBeforeBattle = MinStepsBeforeBattle + 10;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _isColliding = true;       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isColliding = false;
    }
}

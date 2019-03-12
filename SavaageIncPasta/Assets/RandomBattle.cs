using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public List<string> Enemies = new List<string>();

    // Use this for initialization
    void Start()
    {
        _rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        _battleTriggerCounter = _randChance.Next(MinStepsBeforeBattle, MaxStepsBeforeBattle + 1);
        _oldPos = _rb.position;
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

            int randomEnemy = _randChance.Next(Enemies.Count);
            Debug.Log(Enemies[randomEnemy]);
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

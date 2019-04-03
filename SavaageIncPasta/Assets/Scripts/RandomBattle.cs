using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ListOfEnemies
{
    public string NameOfTeam;
    public int Weight;

    public void CapWeight()
    {
        if (this.Weight <= 0)
            Weight = 1;
    }
}

public class RandomBattle : MonoBehaviour
{
    public int MinStepsBeforeBattle;
    public int MaxStepsBeforeBattle;
    private readonly System.Random _randNumGenerator = new System.Random();
    private int _battleTriggerCounter;
    private int _stepCounter = 0;
    private bool _isColliding = false;
    private Rigidbody2D _rb;
    private Vector2 _oldPos;
    private readonly float _range = 2.5f;
    private int _totalWeights = 0;

    [SerializeField] public List<ListOfEnemies> ListOfEnemyTeams = new List<ListOfEnemies>();

    // Use this for initialization
    void Start()
    {
        _rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        _battleTriggerCounter = _randNumGenerator.Next(MinStepsBeforeBattle, MaxStepsBeforeBattle + 1);
        _oldPos = _rb.position;

        foreach (ListOfEnemies enemy in ListOfEnemyTeams)
        {
            enemy.CapWeight();
            _totalWeights += enemy.Weight;
        }

        CapValuesForMinAndMaxSteps();
    }

    // Update is called once per frame
    void Update()
    {
        //If player takes a step in the battle area then increase step counter
        if ((_oldPos.sqrMagnitude <= _rb.position.sqrMagnitude - _range || _oldPos.sqrMagnitude >= _rb.position.sqrMagnitude + _range) && _isColliding)
        {
            _oldPos = _rb.position;
            _stepCounter++;
        }
        
        if (_stepCounter >= _battleTriggerCounter)
        {
            _battleTriggerCounter = _randNumGenerator.Next(MinStepsBeforeBattle, MaxStepsBeforeBattle + 1);
            _stepCounter = 0;

            //Randomise number between 0 and total weights
            //See which enemy the randomised number points to by adding their weights
            int randomEnemy = _randNumGenerator.Next(_totalWeights+1);
            int weight = 0;
            foreach (ListOfEnemies enemy in ListOfEnemyTeams)
            {
                weight += enemy.Weight;
                if (weight >= randomEnemy)
                {
                    Debug.Log(enemy.NameOfTeam + " probability: " + (float)enemy.Weight / (float)_totalWeights + 1);
                    //Add enemies to player prefs to be loaded in during the battle scene
                    PlayerPrefs.SetString("EnemyTeam", enemy.NameOfTeam);
                    PlayerPrefs.SetInt("SceneOrigin", SceneManager.GetActiveScene().buildIndex);
                    PlayerPrefs.SetFloat("SceneOriginX", _rb.position.x);
                    PlayerPrefs.SetFloat("SceneOriginY", _rb.position.y);
                    PlayerPrefs.Save();
                    break;
                }
            }
            SceneManager.LoadScene("Battle", LoadSceneMode.Single);
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

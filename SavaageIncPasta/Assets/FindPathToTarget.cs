using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPathToTarget : MonoBehaviour
{
    public GameObject Target;
    private GameObject _AStarManager;
    private AStarPathfinding _pathfinding;
    private List<Node> _path;

    public List<Node> Path
    {
        get
        {
            return _path;
        }

        set
        {
            _path = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        _AStarManager = GameObject.Find("A* Manager");
        _pathfinding = _AStarManager.GetComponent<AStarPathfinding>();
    }

    // Update is called once per frame
    void Update()
    {
        _path = _pathfinding.FindPath(gameObject.transform.position, Target.transform.position);

        if (_path != null && !GetComponent<AIController>().enabled)
        {
            GetComponent<AIController>().enabled = true;
        }
    }
}

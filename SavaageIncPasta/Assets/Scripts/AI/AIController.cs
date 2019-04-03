using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private List<Node> _targets = new List<Node>();
    private int _destPoint = 0;
    public float MoveSpeed;
    private Vector3 _destination;

    public Vector3 Destination
    {
        get
        {
            return _destination;
        }
    }

    void Awake()
    {
        this.enabled = false;
    }

    // Use this for initialization
    void Start()
    {
        _targets = GetComponent<FindPathToTarget>().Path;
        _destination = _targets[_destPoint].Position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(_targets[_destPoint].Position, transform.position) < 0.05f)
        {
            GoToNextPoint();
        }

        transform.position += (_destination - transform.position).normalized * MoveSpeed * Time.deltaTime;
    }

    private void GoToNextPoint()
    {
        _destPoint = (_destPoint + 1) % _targets.Count;

        if (_destPoint == 0)
        {
            _targets.Reverse();
        }

        _destination = _targets[_destPoint].Position;
    }
}

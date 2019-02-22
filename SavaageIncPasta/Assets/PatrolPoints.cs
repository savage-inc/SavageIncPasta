using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoints : MonoBehaviour {
    private GameObject[] _targets = new GameObject[3];
    private int _destPoint = 0;
    private float _moveSpeed = 2.5f;
    private Vector3 _destination;

    public Vector3 Destination
    {
        get
        {
            return _destination;
        }
    }

    // Use this for initialization
    void Start () {
        _targets[0] = GameObject.Find("Checkpoint");
        _targets[1] = GameObject.Find("Checkpoint (1)");
        _targets[2] = GameObject.Find("Checkpoint (2)");

        _destination = _targets[_destPoint].transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        if (Vector3.Distance(_targets[_destPoint].transform.position, transform.position) < 0.05f)
        {
            GoToNextPoint();
        }

        transform.position += (_destination - transform.position).normalized * _moveSpeed * Time.deltaTime;
    }

    private void GoToNextPoint()
    {
        _destPoint = (_destPoint + 1) % _targets.Length;
        _destination = _targets[_destPoint].transform.position;
    }
}

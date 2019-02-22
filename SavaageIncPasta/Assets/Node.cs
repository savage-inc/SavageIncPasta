using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private bool _walkable;
    private Vector3 _mapPosition;
    private Node _parent;
    private int _stepsFromCurrentPos, _stepsFromDestination, _gridPosX, _gridPosY;

    public Node Parent
    {
        get
        {
            return _parent;
        }

        set
        {
            _parent = value;
        }
    }

    public int StepsFromCurrentPos
    {
        get
        {
            return _stepsFromCurrentPos;
        }

        set
        {
            _stepsFromCurrentPos = value;
        }
    }
    public int StepsFromDestination
    {
        get
        {
            return _stepsFromDestination;
        }

        set
        {
            _stepsFromDestination = value;
        }
    }

    public int PosX
    {
        get
        {
            return _gridPosX;
        }
    }

    public int PosY
    {
        get
        {
            return _gridPosY;
        }
    }
    public int TotalCost
    {
        get
        {
            return _stepsFromCurrentPos + _stepsFromDestination;
        }
    }

    public bool Walkable
    {
        get
        {
            return _walkable;
        }

        set
        {
            _walkable = value;
        }
    }

    public Vector3 Position
    {
        get
        {
            return _mapPosition;
        }

        set
        {
            _mapPosition = value;
        }
    }

    public Node(bool walkable, Vector3 position, int x, int y)
    {
        _walkable = walkable;
        _mapPosition = position;
        _gridPosX = x;
        _gridPosY = y;
    }
}

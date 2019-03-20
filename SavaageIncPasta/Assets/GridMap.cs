using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    public bool DebugView = false;

    public LayerMask Obstacle;
    [SerializeField]private Vector2 _worldSize = new Vector2();
    private readonly float _nodeSize = 1.0f;
    private Node[,] _grid;

    public Node[,] Grid
    {
        get
        {
            return _grid;
        }
    }

    private int _gridSizeX, _gridSizeY;

    void Start()
    {
        _gridSizeX = Mathf.RoundToInt(_worldSize.x / _nodeSize);
        _gridSizeY = Mathf.RoundToInt(_worldSize.y / _nodeSize);

        CreateGrid();
    }

    private void CreateGrid()
    {
        _grid = new Node[_gridSizeX, _gridSizeY];
        Vector3 bottomLeft = transform.position - (Vector3.right * _worldSize.x / 2) - (Vector3.up * _worldSize.y / 2);

        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * _nodeSize + (_nodeSize / 2)) + Vector3.up * (y * _nodeSize + (_nodeSize / 2));
                bool walkable = !(Physics2D.CircleCast(worldPoint, (_nodeSize / 2), Vector3.up, (_nodeSize / 2), Obstacle));
                _grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbourNodes(Node node)
    {
        List<Node> neighbourNodes = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int neighboursX = node.PosX + x;
                int neighboursY = node.PosY + y;

                if (neighboursX >= 0 && neighboursX < _gridSizeX && neighboursY >= 0 && neighboursY < _gridSizeY)
                {
                    neighbourNodes.Add(_grid[neighboursX, neighboursY]);
                }
            }
        }

        return neighbourNodes;
    }

    public Node FindNodeInGrid(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x / _worldSize.x) + 0.5f;
        float percentY = (worldPosition.y / _worldSize.y) + 0.5f;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);

        return _grid[x, y];
    }

    //public Vector3 FindNodeInWorld(Node node)
    //{
    //    float percentX = node.PosX / (_gridSizeX - 1.0f);
    //    float percentY = node.PosY / (_gridSizeY - 1.0f);

    //    percentX = Mathf.Clamp01(percentX);
    //    percentY = Mathf.Clamp01(percentY);

    //    float x = _worldSize.x * (percentX - 0.5f);
    //    float y = _worldSize.y * (percentY - 0.5f);

    //    return new Vector3(x, y, 0);
    //}

    private void OnDrawGizmos()
    {
        if (!DebugView)
        {
            return;
        }

        Gizmos.DrawWireCube(transform.position, new Vector3(_worldSize.x, _worldSize.y, 1));

        if (_grid != null)
        {
            foreach (Node node in _grid)
            {
                Gizmos.color = (node.Walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(node.Position, Vector3.one * (_nodeSize - 0.1f));
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    public LayerMask Obstacle;
    private readonly Vector2 _worldSize = new Vector2(17, 10);
    private readonly float _nodeSize = 1.0f;
    Node[,] grid;
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

    private int _gridSizeX, _gridSizeY;

    void Start()
    {
        _gridSizeX = Mathf.RoundToInt(_worldSize.x / _nodeSize);
        _gridSizeY = Mathf.RoundToInt(_worldSize.y / _nodeSize);

        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Node[_gridSizeX, _gridSizeY];
        Vector3 bottomLeft = transform.position - (Vector3.right * _worldSize.x / 2) - (Vector3.up * _worldSize.y / 2);

        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * _nodeSize + (_nodeSize / 2)) + Vector3.up * (y * _nodeSize + (_nodeSize / 2));
                bool walkable = !(Physics2D.CircleCast(worldPoint, (_nodeSize / 2), Vector3.up, (_nodeSize / 2), Obstacle));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
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
                    neighbourNodes.Add(grid[neighboursX, neighboursY]);
                }
            }
        }

        return neighbourNodes;
    }

    public Node FindNodeInGrid(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + _worldSize.x / 2) / _worldSize.x;
        float percentY = (worldPosition.y + _worldSize.y / 2) / _worldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    public Vector3 FindNodeInWorld(Node node)
    {
        float percentX = (node.PosX / _gridSizeX) + 1/2;
        float percentY = (node.PosY / _gridSizeY) + 1/2;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt(percentX * _worldSize.x);
        int y = Mathf.RoundToInt(percentY * _worldSize.y);

        return new Vector3(x, y, 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_worldSize.x, _worldSize.y, 1));

        if (grid != null)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = (node.Walkable) ? Color.white : Color.red;

                if (_path != null)
                {
                    if (_path.Contains(node))
                    {
                        Gizmos.color = Color.black;
                    }
                }

                Gizmos.DrawCube(node.Position, Vector3.one * (_nodeSize - 0.1f));
            }
        }
    }
}

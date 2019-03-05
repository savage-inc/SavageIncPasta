using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    private GridMap _grid;

    private void Awake()
    {
        _grid = GetComponent<GridMap>();
    }

    public List<Node> FindPath(Vector3 startPos, Vector3 destination)
    {
        Node startNode = _grid.FindNodeInGrid(startPos);
        Node destinationNode = _grid.FindNodeInGrid(destination);

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].TotalCost < currentNode.TotalCost ||
                    (openList[i].TotalCost == currentNode.TotalCost && openList[i].StepsFromDestination < currentNode.StepsFromDestination))
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == destinationNode)
            {
                return RetracePath(startNode, destinationNode);
            }

            foreach (Node neighbour in _grid.GetNeighbourNodes(currentNode))
            {
                if (closedList.Contains(neighbour) || !neighbour.Walkable)
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.StepsFromCurrentPos + GetDistance(currentNode, neighbour);
                if (!openList.Contains(neighbour) || newMovementCostToNeighbour < neighbour.StepsFromCurrentPos)
                {
                    neighbour.StepsFromCurrentPos = newMovementCostToNeighbour;
                    neighbour.StepsFromDestination = GetDistance(neighbour, destinationNode);
                    neighbour.Parent = currentNode;

                    if (!openList.Contains(neighbour) && !closedList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }

        return null;
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }

    private int GetDistance(Node firstNode, Node secondNode)
    {
        int distanceX = Mathf.Abs(firstNode.PosX - secondNode.PosX);
        int distanceY = Mathf.Abs(firstNode.PosY - secondNode.PosY);

        if (distanceX > distanceY)
        {
            return (4 * distanceY) + (10 * distanceX);
        }

        return (4 * distanceX) + (10 * distanceY);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    private GridMap grid;
    public Transform ai, target;

    private void Awake()
    {
        grid = GetComponent<GridMap>();
    }

    private void Update()
    {
        FindPath(ai.position, target.position);
    }

    public void FindPath(Vector3 startPos, Vector3 destination)
    {
        Node startNode = grid.FindNodeInGrid(startPos);
        Node destinationNode = grid.FindNodeInGrid(destination);

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
                RetracePath(startNode, destinationNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbourNodes(currentNode))
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

        return;
    }

    private void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        grid.Path = path;
    }

    private int GetDistance(Node firstNode, Node secondNode)
    {
        int distanceX = Mathf.Abs(firstNode.PosX - secondNode.PosX);
        int distanceY = Mathf.Abs(firstNode.PosY - secondNode.PosY);

        if (distanceX > distanceY)
        {
            return (14 * distanceY) + (10 * (distanceX - distanceY));
            //4y + 10x
        }

        return (14 * distanceX) + (10 * (distanceY - distanceX));
    }
}

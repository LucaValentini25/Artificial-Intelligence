using System.Collections.Generic;
using UnityEngine;

public static class Pathfinding 
{

    // public static Pathfinding Instance { get; private set; }
    //
    // private void Awake()
    // {
    //     Instance = this;
    // }
    public static List<Node> CalculateAStar(Node startingNode, Node goalNode)
    {
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            if (current == goalNode)
            {
                List<Node> path = new List<Node>();

                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }

                path.Reverse();
                return path;
            }

            foreach (var item in current.GetNeighbours())
            {
                int newCost = costSoFar[current] + 1;
                float priority = newCost + Vector3.Distance(item.transform.position, goalNode.transform.position);

                if (!costSoFar.ContainsKey(item))
                {
                    if (!frontier.ContainsKey(item))
                        frontier.Enqueue(item, priority);
                    cameFrom.Add(item, current);
                    costSoFar.Add(item, newCost);
                }
                else if (costSoFar[item] > newCost)
                {
                    if (!frontier.ContainsKey(item))
                        frontier.Enqueue(item, priority);
                    cameFrom[item] = current;
                    costSoFar[item] = newCost;
                }
            }
        }
        return new List<Node>();
    }
    public static List<Node> CalculateThetaStar(Node startingNode, Node goalNode)
    {
        var listNode = CalculateAStar(startingNode, goalNode);

        int current = 0;

        while (current + 2 < listNode.Count)
        {
            if (GameManager.Instance.LosToWall(listNode[current].transform.position, listNode[current + 2].transform.position))
            {
                listNode.RemoveAt(current + 1);
            }
            else
                current++;
        }

        return listNode;
    }

    public static List<Node> CalculateThetaStar(Vector3 startingPos, Vector3 goalPos)
    {
        Node startingNode = GetNearNode(startingPos);
        Node goalNode = GetNearNode(goalPos);
        if (startingPos == null || goalNode == null) return null;
        return CalculateThetaStar(startingNode, goalNode);
    }

    public static Node GetNearNode(Vector3 pos)
    {
        return NodeManager.Instance.GetNearestNode(pos);
    }
}

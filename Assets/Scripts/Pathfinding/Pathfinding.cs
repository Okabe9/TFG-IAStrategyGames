using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class Pathfinding
{

    public static float Heuristic(Vector2 start, Vector2 end)
    {
        return Vector2.Distance(start, end);
    }

    public static List<Vector2> AStar(Vector2 start, Vector2 end)
    {
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        Node startNode = new Node(start, 0, Heuristic(start, end), null);
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // Sort openList by the sum of the cost and heuristic values
            openList.Sort((x, y) => (x.cost + x.heuristic).CompareTo(y.cost + y.heuristic));
            Node currentNode = openList[0];
            openList.RemoveAt(0);

            if (currentNode.position == end)
            {
                // Found the end node, backtrack through the parent nodes to get the path
                List<Vector2> path = new List<Vector2>();
                Node node = currentNode;
                while (node != null)
                {
                    path.Insert(0, node.position);
                    node = node.parent;
                }
                return path;
            }

            closedList.Add(currentNode);

            // Get the neighboring nodes
            List<Vector2> neighbors = new List<Vector2>();
            Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
            foreach (Vector2 direction in directions)
            {
                Vector2 neighbor = currentNode.position + direction;
                if (neighbor.x >= 0 && neighbor.x < BoardController.Instance.size && neighbor.y >= 0 && neighbor.y < BoardController.Instance.size)
                {
                    // TODO: Verificacion si es un tile enemigo
                    neighbors.Add(neighbor);

                }
            }

            foreach (Vector2 neighbor in neighbors)
            {
                float newCost = currentNode.cost + Vector2.Distance(currentNode.position, neighbor);
                float newHeuristic = Heuristic(neighbor, end);
                Node neighborNode = new Node(neighbor, newCost, newHeuristic, currentNode);

                // Check if the neighbor is already in the closed list
                bool isInClosedList = false;
                foreach (Node node in closedList)
                {
                    if (node.position == neighbor)
                    {
                        isInClosedList = true;
                        break;
                    }
                }
                if (isInClosedList)
                {
                    continue;
                }

                // Check if the neighbor is already in the open list
                bool isInOpenList = false;
                foreach (Node node in openList)
                {
                    if (node.position == neighbor)
                    {
                        isInOpenList = true;
                        if (newCost < node.cost)
                        {
                            node.cost = newCost;
                            node.heuristic = newHeuristic;
                            node.parent = currentNode;
                        }
                        break;
                    }
                }
                if (!isInOpenList)
                {
                    openList.Add(neighborNode);
                }
            }
        }

        // If we've reached here, it means there's no path to the end node
        return null;
    }

}


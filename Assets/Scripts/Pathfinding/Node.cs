using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Node
{
    public Vector2 position;
    public float cost;
    public float heuristic;
    public Node parent;

    public Node(Vector2 pos, float c, float h, Node p)
    {
        position = pos;
        cost = c;
        heuristic = h;
        parent = p;
    }
}
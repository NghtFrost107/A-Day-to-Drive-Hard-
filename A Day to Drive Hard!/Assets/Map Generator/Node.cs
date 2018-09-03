using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public Vector3 position;
    public int verticeIndex = -1;

    public Node(Vector3 pos)
    {
        position = pos;
    }
}

public class MainNode : Node
{
    public bool active;
    public Node above, right;

    public MainNode(Vector3 _pos, bool _active) : base(_pos)
    {
        active = _active;
        above = new Node(position + Vector3.forward);
        right = new Node(position + Vector3.right);
    }
}



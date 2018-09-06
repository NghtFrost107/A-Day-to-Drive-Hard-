using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public Vector3 position;
    public int verticeIndex = -1;
    public bool active;

    public Node(Vector3 pos, bool _active)
    {
        active = _active;
        position = pos;
    }
}




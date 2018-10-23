using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Square {

    public Node topLeft, topRight, bottomRight, bottomLeft;

    public int x; //Coordinates from start of section generated (Bottom Left = 0,0)
    public int y;

    public GameObject objectOnSquare;

    public Square (int cordX, int cordY)
    {
        x = cordX;
        y = cordY;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Square {

    public MainNode topLeft, topRight, bottomRight, bottomLeft;
    public int configuration;
    public int state;

    public int x; //Coordinates from start of section generated (Bottom Left = 0,0)
    public int y;

    public Square(int state)
    {

        this.state = state;
    }

    public void AssignNodes(MainNode topLeft, MainNode topRight, MainNode bottomRight, MainNode bottomLeft)
    {
        this.topLeft = topLeft;
        this.topRight = topRight;
        this.bottomLeft = bottomLeft;
        this.bottomRight = bottomRight;
    }
}

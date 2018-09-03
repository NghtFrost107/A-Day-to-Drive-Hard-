using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StateTypes
{
    AIR, TERRAIN, BOUNDARY
}

public class Square {

    public MainNode topLeft, topRight, bottomRight, bottomLeft;
    public int configuration;
    /*
     * Here holds the state for the current square.
     * 
     * State Types:
     * 0 = Nothing (Is air)
     * 1 = Terrain
     * 2 = Boundary Terrain (Between normal Terrain and Air)
     */
    public int state;

    public int x; //Coordinates from start of section generated (Bottom Left = 0,0)
    public int y;

    public Square (int cordX, int cordY)
    {
        x = cordX;
        y = cordY;
    }

    public Square(int cordX, int cordY, int state) : this(cordX, cordY)
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

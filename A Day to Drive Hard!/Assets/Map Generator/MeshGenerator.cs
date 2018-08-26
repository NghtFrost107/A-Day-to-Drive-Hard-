using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

    ActiveSquares squareGrid;

    List<Vector3> vertices;
    List<int> triangles;

    public void GenerateMesh(Square[,] section)
    {
        squareGrid = new ActiveSquares(section);

        vertices = new List<Vector3>();
        triangles = new List<int>();
    
        for (int x = 0; x < section.GetLength(0); x++)
        {
            for (int y = 0; y < section.GetLength(1); y++)
            {
               // if (section[x, y].bottomLeft.position.y > squareGrid.lowestPoint.bottomLeft.position.y)
                {
                    if (section[x, y].state != 0)
                    {
                        MeshFromPoints(new MainNode[] { section[x, y].bottomLeft, section[x, y].topLeft, section[x, y].bottomRight });
                        MeshFromPoints(new MainNode[] { section[x, y].bottomRight, section[x, y].topLeft, section[x, y].topRight });
                    }
                    if (section[x, y].state == 2 && x != section.GetLength(0) - 1)
                    {
                        Square connectingSquare = this.FindBoundarySquare(section, x + 1);
                        if (connectingSquare != null)
                        {
                            if (connectingSquare.y > section[x, y].y)
                            {
                                MeshFromPoints(new MainNode[] { section[x, y].topLeft, connectingSquare.topLeft, section[x, y].topRight });
                            }
                            else if (connectingSquare.y < section[x, y].y)
                            {
                                MeshFromPoints(new MainNode[] { section[x, y].topRight, connectingSquare.topRight, connectingSquare.topLeft });
                            }
                        }
                    }
                }
            }

        }

        //MeshFromPoints(new MainNode[] { section[0,(int)squareGrid.lowestPoint.bottomLeft.position.y].bottomLeft, squareGrid.lowestPoint.bottomRight, section[0,0].bottomLeft });

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }
    void MeshFromPoints(MainNode[] nodes)
    {
        AssignVertices(nodes);
        CreateTriangle(nodes);
    }

    void AssignVertices(MainNode[] nodes)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i].verticeIndex == -1)
            {
                nodes[i].verticeIndex = vertices.Count;
                vertices.Add(nodes[i].position);
            }
        }
    }

    void CreateTriangle(MainNode[] nodes)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            triangles.Add(nodes[i].verticeIndex);
        }
    }

    Square FindBoundarySquare(Square[,] section, int xColumn)
    {
        for(int y = 0; y < section.GetLength(1); y++)
        {
            if (section[xColumn,y].state == 2)
            {
                return section[xColumn, y];
            }
        }

        return null;
    }

    public class ActiveSquares
    {
        public Square lowestPoint;

        public ActiveSquares(Square[,] section)
        {
            /*
            int nodesX = section.GetLength(0);
            int nodesY = section.GetLength(1);

            lowestPoint = section[0, section.GetLength(1) - 1];
            int mapWidth = nodesX;
            int mapHeight = nodesY;
            
            MainNode[,] nodes = new MainNode[nodesX + 1, nodesY + 1];
            */

            for(int x = 0; x < section.GetLength(0); x++)
            {
                for (int y = 0; y < section.GetLength(1); y++)
                {
                    //Create the bottom left and top left nodes for each square
                    if (y == 0) //If it is creating the nodes in the first column
                    {
                        section[x, y].bottomLeft = new MainNode(new Vector3(section[x, y].x, section[x, y].y, 0), section[x, y].state != 0);
                    }else
                    {
                        section[x, y].bottomLeft = section[x, y - 1].topLeft;
                    }
                    section[x, y].topLeft = new MainNode(new Vector3(section[x, y].x, section[x, y].y + 1, 0), section[x, y].state != 0);

                    //Set the bottom-right and top-right nodes for the squares in the previous column at the same y value
                    if (x != 0)
                    {
                        section[x - 1, y].bottomRight = section[x, y].bottomLeft;
                        section[x - 1, y].topRight = section[x, y].topLeft;
                    }
                    //Make sure the squares at the furtherest x-column have a bottom-Right & top-Right node
                    if (x == section.GetLength(0) - 1)
                    {
                        section[x, y].bottomRight = new MainNode(new Vector3(section[x, y].x, section[x, y].y + 1, 0), section[x, y].state != 0);
                        section[x, y].topRight = new MainNode(new Vector3(section[x, y].x + 1, section[x, y].y + 1, 0), section[x, y].state != 0);
                    }





                    /*
                    Vector3 pos = new Vector3(-mapWidth / 2 + x, -mapHeight / 2 + y, 0);
                    if (x != nodesX && y != nodesY)
                    {
                        nodes[x, y] = new MainNode(pos, section[x, y].state == 1 || section[x, y].state == 2);
                        if (section[x, y].state == 2 && lowestPoint.bottomLeft.position.y > pos.y)
                        {
                            lowestPoint = section[x,y];
                        }
                    }
                    else
                        nodes[x, y] = new MainNode(pos, false);

                    */
                }
            }
            /*
            for (int x = 0; x < nodesX; x++)
            {
                for (int y = 0; y < nodesY; y++)
                {
                    section[x, y].AssignNodes(nodes[x, y + 1], nodes[x + 1, y + 1], nodes[x + 1, y], nodes[x, y]);
                }
            }*/
        }
    }

}

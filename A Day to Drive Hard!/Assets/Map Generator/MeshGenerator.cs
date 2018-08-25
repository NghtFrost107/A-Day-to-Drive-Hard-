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
                if (section[x, y].bottomLeft.position.y > squareGrid.lowestPoint)
                {
                    if (section[x, y].state == 1 || section[x, y].state == 2)
                    {
                        MeshFromPoints(new MainNode[] { section[x, y].bottomLeft, section[x, y].topLeft, section[x, y].bottomRight });
                        MeshFromPoints(new MainNode[] { section[x, y].bottomRight, section[x, y].topLeft, section[x, y].topRight });
                    }
                }
            }

        }

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


    public class ActiveSquares
    {
        public float lowestPoint = Screen.height;

        public ActiveSquares(Square[,] section)
        {
            int nodesX = section.GetLength(0);
            int nodesY = section.GetLength(1);

            int mapWidth = nodesX;
            int mapHeight = nodesY;

            MainNode[,] nodes = new MainNode[nodesX + 1, nodesY + 1];

            for(int x = 0; x <= nodesX; x++)
            {
                for (int y = 0; y <= nodesY; y++)
                {
                    Vector3 pos = new Vector3(-mapWidth / 2 + x, -mapHeight / 2 + y, 0);
                    if (x != nodesX && y != nodesY)
                    {
                        nodes[x, y] = new MainNode(pos, section[x, y].state == 1 || section[x, y].state == 2);
                        if (section[x, y].state == 2 && lowestPoint > pos.y)
                        {
                            lowestPoint = pos.y;
                        }
                    }
                    else
                        nodes[x, y] = new MainNode(pos, false);

                    
                }
            }

            for (int x = 0; x < nodesX; x++)
            {
                for (int y = 0; y < nodesY; y++)
                {
                    section[x, y].AssignNodes(nodes[x, y + 1], nodes[x + 1, y + 1], nodes[x + 1, y], nodes[x, y]);
                }
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

    List<Vector3> vertices;
    List<int> triangles;

    public void GenerateMesh(Square[,] section, int lowestPoint)
    {
        AssignNodes(section);

        vertices = new List<Vector3>();
        triangles = new List<int>();
    
        for (int x = 0; x < section.GetLength(0); x++)
        {
            for (int y = 0; y < section.GetLength(1); y++)
            {
                if (section[x, y].y > lowestPoint)
                {
                    if (section[x, y].state != 0)
                    {
                        MeshFromPoints(new MainNode[] { section[x, y].bottomLeft, section[x, y].topLeft, section[x, y].bottomRight });
                        MeshFromPoints(new MainNode[] { section[x, y].bottomRight, section[x, y].topLeft, section[x, y].topRight });
                    }
                    if (section[x, y].state == 2 && x != section.GetLength(0) - 1)
                    {
                        Square connectingSquare = MapGenerator.FindBoundarySquare(section, x + 1);
                        if (connectingSquare != null)
                        {
                            SectionCollider sectionCollider = GetComponent<SectionCollider>();
                            if (connectingSquare.y > section[x, y].y)
                            {
                                MeshFromPoints(new MainNode[] { section[x, y].topLeft, connectingSquare.topLeft, section[x, y].topRight });
                                sectionCollider.addPoint(section[x, y].topLeft);
                                sectionCollider.addPoint(connectingSquare.topLeft);
                            }
                            else if (connectingSquare.y < section[x, y].y)
                            {
                                MeshFromPoints(new MainNode[] { section[x, y].topRight, connectingSquare.topRight, connectingSquare.topLeft });
                                sectionCollider.addPoint(section[x, y].topRight);
                                sectionCollider.addPoint(connectingSquare.topRight);
                            } else if (connectingSquare.y == section[x,y].y)
                            {
                                sectionCollider.addPoint(section[x, y].topRight);
                            }
                        }
                    }
                }
            }

        }

        MeshFromPoints(new MainNode[] { section[0, 0].bottomLeft, section[0,lowestPoint].topLeft, section[section.GetLength(0) - 1,lowestPoint].topRight  });
        MeshFromPoints(new MainNode[] { section[0, 0].bottomLeft, section[section.GetLength(0) - 1, lowestPoint].topRight, section[section.GetLength(0) - 1, 0].bottomRight });

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

    

    private void AssignNodes(Square[,] section)
    {
        for (int x = 0; x < section.GetLength(0); x++)
        {
            for (int y = 0; y < section.GetLength(1); y++)
            {
                //Create the bottom left and top left nodes for each square
                if (y == 0) //If it is creating the nodes in the first column
                {
                    section[x, y].bottomLeft = new MainNode(new Vector3(section[x, y].x, section[x, y].y, 0), section[x, y].state != 0);
                }
                else
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
                    section[x, y].bottomRight = new MainNode(new Vector3(section[x, y].x + 1, section[x, y].y, 0), section[x, y].state != 0);
                    section[x, y].topRight = new MainNode(new Vector3(section[x, y].x + 1, section[x, y].y + 1, 0), section[x, y].state != 0);
                }
            }
        }
    }
}

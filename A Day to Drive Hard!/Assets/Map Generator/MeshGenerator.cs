using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

    List<Vector3> vertices;
    List<int> triangles;


    int vertIndex = 0;

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
                        //MeshFromPoints(new MainNode[] { section[x, y].bottomLeft, section[x, y].topLeft, section[x, y].bottomRight });
                        //MeshFromPoints(new MainNode[] { section[x, y].bottomRight, section[x, y].topLeft, section[x, y].topRight });
                    }
                    

                    if (section[x, y].state == 2 && x != section.GetLength(0) - 1)
                    {
                        
                        Square connectingSquare = MapGenerator.FindBoundarySquare(section, x + 1);
                        if (connectingSquare != null)
                        {
                            SectionCollider sectionCollider = GetComponent<SectionCollider>();
                            if (connectingSquare.y > section[x, y].y)
                            {
                                AssignSquareMesh(section[x, y]);
                                //MeshFromPoints(new MainNode[] { section[x, y].topLeft, connectingSquare.topLeft, section[x, y].topRight });
                                Vector2[] curvePoints = calculateCurve(section[x, y].topLeft, connectingSquare.topLeft);
                                AssignCurveVertices(curvePoints, connectingSquare.bottomLeft.position);

                                sectionCollider.addPoint(curvePoints);
                                /*sectionCollider.addPoint(section[x, y].topLeft);
                                sectionCollider.addPoint(connectingSquare.topLeft);*/
                            }
                            else if (connectingSquare.y < section[x, y].y)
                            {
                                AssignSquareMesh(section[x, y]);
                                Vector2[] curvePoints = calculateCurve(section[x, y].topRight, connectingSquare.topRight);
                                AssignCurveVertices(curvePoints, connectingSquare.topLeft.position);

                                sectionCollider.addPoint(curvePoints);
                                 //MeshFromPoints(new MainNode[] { section[x, y].topRight, connectingSquare.topRight, connectingSquare.topLeft });
                                 /*sectionCollider.addPoint(section[x, y].topRight);
                                 sectionCollider.addPoint(connectingSquare.topRight);*/
                            } else if (connectingSquare.y == section[x,y].y)
                            {
                                AssignSquareMesh(section[x, y]);
                                sectionCollider.addPoint(section[x, y].topRight.position);
                            }
                        }
                    }
                }
            }

        }

        //MeshFromPoints(new MainNode[] { section[0, 0].bottomLeft, section[0,lowestPoint].topLeft, section[section.GetLength(0) - 1,lowestPoint].topRight  });
       // MeshFromPoints(new MainNode[] { section[0, 0].bottomLeft, section[section.GetLength(0) - 1, lowestPoint].topRight, section[section.GetLength(0) - 1, 0].bottomRight });
        


        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    void AssignSquareMesh(Square square)
    {
        vertices.Add(square.bottomLeft.position);
        vertices.Add(square.topLeft.position);
        vertices.Add(square.topRight.position);
        vertices.Add(square.bottomRight.position);

        for(int i = 0; i < 2; i++)
        {
            triangles.Add(vertIndex);
            triangles.Add(vertIndex + 1 + i);
            triangles.Add(vertIndex + 2 + i);
        }

        vertIndex += 4;
        
    }

    void AssignCurveVertices(Vector2[] points, Vector2 anchor)
    {
        for(int i = 0; i < points.Length; i++)
        {
            vertices.Add(points[i]);
        }
        vertices.Add(anchor);

        for (int i = 0; i < points.Length - 1; i++)
        {
            triangles.Add(vertIndex + i);
            triangles.Add(vertIndex + i + 1);
            triangles.Add(vertIndex + points.Length);
        }
        vertIndex += points.Length + 1;
    }

    Vector2[] calculateCurve(MainNode start, MainNode end)
    {
        List<Vector2> points = new List<Vector2>();
        Vector2 curvePoint;
        if (start.position.y > end.position.y)
        {
            curvePoint = new Vector2(start.position.x, end.position.y);
        } else
        {
            curvePoint = new Vector2(end.position.x, start.position.y);
        }
        
        for (float i = 0; i <= 1.01f; i += 0.1f)
        {
            points.Add(QuadraticCurve(start.position,curvePoint, end.position,i));
        }
        return points.ToArray();
    }

    Vector2 QuadraticCurve(Vector2 start, Vector2 curvePoint, Vector2 end, float pointPosition)
    {
        Vector2 p0 = Lerp(start, curvePoint, pointPosition);
        Vector2 p1 = Lerp(curvePoint, end, pointPosition);
        return Lerp(p0, p1, pointPosition);
    }
    Vector2 Lerp(Vector2 start, Vector2 end, float pointPosition)
    {
        return start + (end - start) * pointPosition;
    }

    void CurvedMesh(Vector2[] points)
    {

    }

    /*
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
    */
    

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

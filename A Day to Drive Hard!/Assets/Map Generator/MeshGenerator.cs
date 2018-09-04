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
                        //AssignSquareMesh(section[x, y]);
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
                                Vector2[] curvePoints = calculateCurve(section[x, y].topLeft, connectingSquare.topLeft);
                                AssignCurveVertices(curvePoints, connectingSquare.bottomLeft.position);

                                sectionCollider.addPoint(curvePoints);
                            }
                            else if (connectingSquare.y < section[x, y].y)
                            {
                                AssignSquareMesh(section[x, y]);
                                Vector2[] curvePoints = calculateCurve(section[x, y].topRight, connectingSquare.topRight);
                                AssignCurveVertices(curvePoints, connectingSquare.topLeft.position);

                                sectionCollider.addPoint(curvePoints);
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

    Vector2[] calculateCurve(MainNode curveStart, MainNode curveEnd)
    {
        List<Vector2> points = new List<Vector2>();
        Vector2 curvePoint1;
        Vector2 curvePoint2;
        if (curveStart.position.y > curveEnd.position.y)
        {
            curvePoint1 = new Vector2(curveEnd.position.x, curveStart.position.y);
            curvePoint2 = new Vector2(curveStart.position.x, curveEnd.position.y);
        } else
        {
            curvePoint1 = new Vector2(curveEnd.position.x, curveStart.position.y);
            curvePoint2 = new Vector2(curveStart.position.x, curveEnd.position.y);
        }
        
        for (float i = 0; i <= 1.01f; i += 0.1f)
        {
            points.Add(CubicCurve(curveStart.position,curvePoint1, curvePoint2, curveEnd.position,i));
        }
        return points.ToArray();
    }

    Vector2 CubicCurve(Vector2 start, Vector2 curvePoint1, Vector2 curvePoint2, Vector2 end, float pointPosition)
    {
        Vector2 p0 = QuadraticCurve(start, curvePoint1, curvePoint2, pointPosition);
        Vector2 p1 = QuadraticCurve(curvePoint1, curvePoint2, end, pointPosition);
        return Lerp(p0, p1, pointPosition);
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

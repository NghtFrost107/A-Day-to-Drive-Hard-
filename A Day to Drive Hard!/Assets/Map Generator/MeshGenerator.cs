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

        Square[] boundarySquares = GetComponent<MapGenerator>().boundarySquares;

        //Runs through all boundary squares in the section of the map
        for (int x = 1; x < boundarySquares.Length - 2; x++)
        {
            Square previousSquare = GetComponent<MapGenerator>().FindBoundarySquare(x - 1); 
            Square nextSquare = GetComponent<MapGenerator>().FindBoundarySquare(x + 1);
            Square nextNextSquare = GetComponent<MapGenerator>().FindBoundarySquare(x + 2);
            if (nextSquare != null && nextNextSquare != null)
            {
                SectionCollider sectionCollider = GetComponent<SectionCollider>();
                Vector2[] collisionPoints;
                if (nextSquare.y > boundarySquares[x].y) //When the elevation is going up
                {
                    AssignSquareMesh(boundarySquares[x]);
                    
                    if (nextNextSquare.y > nextSquare.y) //When the elevation will still be going up upon next iteration
                    {
                        /**       _
                         *      _|N|
                         *    _|N|
                         *  _|x| -- You are here (x)
                         * |P|
                         * 
                         * Is currently going up a hill (Previous squares was lower and next squares are higher than the current square)
                         */
                        if (previousSquare.y < boundarySquares[x].y)
                        {
                            AssignTriangleMesh(boundarySquares[x].topLeft.position, nextSquare.topLeft.position, boundarySquares[x].topRight.position);
                            collisionPoints = new Vector2[2] { boundarySquares[x].topLeft.position, nextSquare.topLeft.position };
                        }
                        /**        _
                         *       _|N|
                         *  _  _|N|
                         * |P||x| -- You are here (x)
                         * 
                         * At the bottom of a hill starting to go up (First square to change elevation from flat terrain)
                         */
                        else 
                        {
                            collisionPoints = calculateQuadraticCurve(previousSquare.topLeft, new Vector2(previousSquare.topLeft.position.x + 1, nextSquare.y), nextSquare.topLeft);
                            AssignCurveVertices(collisionPoints, nextSquare.bottomLeft.position);
                        }
                    }
                    else
                    {
                        if (previousSquare.y < boundarySquares[x].y) //Is at the end of a hill (Previous square was lower but the next square goes back to flat terrain)
                        {
                            collisionPoints = calculateQuadraticCurve(boundarySquares[x].topLeft, new Vector2(nextSquare.topLeft.position.x - 0.25f, nextSquare.topLeft.position.y), nextSquare.topRight);
                            AssignCurveVertices(collisionPoints, nextSquare.bottomRight.position);
                            x++; //Skip over next boundary square as mesh has already been drawn for it
                        }
                        else { //When a small elevation change occurs that doesn't match the other criteria (Normally occurs if elevation changes by 1 square but rest is flat terrain)
                            collisionPoints = calculateCubicCurve(previousSquare.topLeft, new Vector2(previousSquare.topLeft.position.x + 1, previousSquare.topRight.position.y), new Vector2(nextSquare.topLeft.position.x - 1, nextSquare.topLeft.position.y), nextSquare.topLeft);
                            AssignCurveVertices(collisionPoints, nextSquare.bottomLeft.position);
                        }
                        
                    }

                    sectionCollider.addPoint(collisionPoints);
                }
                else if (nextSquare.y < boundarySquares[x].y)
                {                    
                    if (nextNextSquare.y < nextSquare.y)
                    {
                        
                        if (previousSquare.y > boundarySquares[x].y)
                        {
                            AssignSquareMesh(boundarySquares[x]);
                            AssignTriangleMesh(boundarySquares[x].topRight.position, nextSquare.topRight.position, boundarySquares[x].bottomRight.position);
                            collisionPoints = new Vector2[2] { boundarySquares[x].topRight.position, nextSquare.topRight.position };
                        }
                        else
                        {
                            collisionPoints = calculateQuadraticCurve(boundarySquares[x].topLeft, new Vector2(boundarySquares[x].topRight.position.x, boundarySquares[x].topLeft.position.y), nextSquare.topRight);
                            AssignCurveVertices(collisionPoints, boundarySquares[x].bottomLeft.position);
                        }
                    }
                    else
                    {
                        if (previousSquare.y > boundarySquares[x].y)
                        {
                            AssignSquareMesh(boundarySquares[x]);
                            collisionPoints = calculateQuadraticCurve(boundarySquares[x].topRight, new Vector2(nextSquare.topRight.position.x - 0.25f, nextSquare.topRight.position.y), nextNextSquare.topRight);
                            AssignCurveVertices(collisionPoints, boundarySquares[x].bottomRight.position);
                        }
                        else
                        {
                            AssignSquareMesh(boundarySquares[x + 1]);
                            collisionPoints = calculateCubicCurve(boundarySquares[x].topLeft, new Vector2(boundarySquares[x].topLeft.position.x + 1, boundarySquares[x].topLeft.position.y), new Vector2(nextSquare.topLeft.position.x, nextSquare.topLeft.position.y), nextSquare.topRight);
                            AssignCurveVertices(collisionPoints, boundarySquares[x].bottomLeft.position);
                            x++;
                        }
                    }

                    sectionCollider.addPoint(collisionPoints);
                }
                else if (nextSquare.y == boundarySquares[x].y)
                {
                    AssignSquareMesh(boundarySquares[x]);
                    sectionCollider.addPoint(boundarySquares[x].topRight.position);
                }
            }
        }/*
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
                    

                    if (section[x, y].state == 2 && x < section.GetLength(0) - 2) //If the square is a boundary square and is not within the last 2 squares at the end of the section
                    {

                       
                    }
                }
            }*/

              


        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    void AssignTriangleMesh(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        vertices.Add(pointA);
        vertices.Add(pointB);
        vertices.Add(pointC);

        for (int i = 0; i < 3; i++)
        {
            triangles.Add(vertIndex + i);
        }
        vertIndex += 3;
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

    Vector2[] calculateCubicCurve(Node curveStart, Vector2 curvePoint1, Vector2 curvePoint2, Node curveEnd)
    {
        List<Vector2> points = new List<Vector2>();
       /* Vector2 curvePoint1;
        Vector2 curvePoint2;
        if (curveStart.position.y > curveEnd.position.y)
        {
            curvePoint1 = new Vector2(curveEnd.position.x - 0.5f, curveStart.position.y);
            curvePoint2 = new Vector2(curveStart.position.x + 0.5f, curveEnd.position.y);
        } else
        {
            curvePoint1 = new Vector2(curveEnd.position.x - 0.5f, curveStart.position.y);
            curvePoint2 = new Vector2(curveStart.position.x + 0.5f, curveEnd.position.y);
        }
        */
        for (float i = 0; i <= 1.01f; i += 0.1f)
        {
            points.Add(CubicCurve(curveStart.position,curvePoint1, curvePoint2, curveEnd.position,i));
        }
        return points.ToArray();
    }

    Vector2[] calculateQuadraticCurve(Node start, Vector2 curvePoint, Node end)
    {
        List<Vector2> points = new List<Vector2>();

        for (float i = 0; i <= 1.01f; i += 0.1f)
        {
            points.Add(QuadraticCurve(start.position, curvePoint, end.position, i));
        }
        return points.ToArray();
    }

    /**
     * Allows for the creation of a cubic curve using 4 points of reference
     */
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
                    section[x, y].bottomLeft = new Node(new Vector3(section[x, y].x, section[x, y].y, 0), section[x, y].state != 0);
                }
                else
                {
                    section[x, y].bottomLeft = section[x, y - 1].topLeft;
                }
                section[x, y].topLeft = new Node(new Vector3(section[x, y].x, section[x, y].y + 1, 0), section[x, y].state != 0);

                //Set the bottom-right and top-right nodes for the squares in the previous column at the same y value
                if (x != 0)
                {
                    section[x - 1, y].bottomRight = section[x, y].bottomLeft;
                    section[x - 1, y].topRight = section[x, y].topLeft;
                }
                //Make sure the squares at the furtherest x-column have a bottom-Right & top-Right node
                if (x == section.GetLength(0) - 1)
                {
                    section[x, y].bottomRight = new Node(new Vector3(section[x, y].x + 1, section[x, y].y, 0), section[x, y].state != 0);
                    section[x, y].topRight = new Node(new Vector3(section[x, y].x + 1, section[x, y].y + 1, 0), section[x, y].state != 0);
                }
            }
        }
    }
}

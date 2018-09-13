using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

    List<Vector3> vertices;
    List<int> triangles;

    Mesh mesh;

    int vertIndex = 0;

    public void GenerateMesh(Square[] boundary, int lowestPoint, Square connectingSquare)
    {
        AssignNodes(boundary);

        vertices = new List<Vector3>();
        triangles = new List<int>();

        /* Runs through all boundary squares in the section of the map
         * 
         * It is reponsible for assigning the meshes to these squares and linking them together smoothly with curved meshes.
         * 
         * Comments are shown above each if statement explaining the current square it is looking at and how it uses the squares around it to determine how to link the squares smoothly.
         * 
         * P = Previous Square
         * x = Current Square
         * N = Next Square
         * S = Next Next Square
         */
        for (int x = 0; x < boundary.Length - 2; x++)
        {
            Square previousSquare;
            if (x == 0) {
                previousSquare = connectingSquare;
            } else {
                previousSquare = boundary[x - 1];
            }

            Square nextSquare = boundary[x + 1];
            Square nextNextSquare = boundary[x + 2];
            if (nextSquare != null && nextNextSquare != null)
            {
                SectionCollider sectionCollider = GetComponent<SectionCollider>();
                Vector2[] collisionPoints;

                /*    _
                 *  _|N|
                 * |x|^
                 * 
                 * When the elevation is going up
                 */
                if (nextSquare.y > boundary[x].y)
                {
                    AssignSquareMesh(boundary[x]); //Creates the mesh for the current square

                    /*       _
                     *     _|S|
                     *   _|N|^
                     *  |x|
                     * When the elevation will still be going up for the next 2 squares
                     */
                    if (nextNextSquare.y > nextSquare.y)
                    {
                        /*        _
                         *      _|S|
                         *    _|N|
                         *  _|x| -- You are here (x)
                         * |P|
                         * Is currently going up a hill (Previous squares was lower and next squares are higher than the current square)
                         * Will create a straight diagonal line from the current square (x) to the next square (N)
                         * 
                         */
                        if (previousSquare.y < boundary[x].y)
                        {
                            AssignTriangleMesh(boundary[x].topLeft.position, nextSquare.topLeft.position, boundary[x].topRight.position);
                            collisionPoints = new Vector2[2] { boundary[x].topLeft.position, nextSquare.topLeft.position };
                        }
                        /*This statement contains two different possible scenarios and creates the mesh accordingly. 
                         * 
                         * First Scenario uses the previous square from 2 squares back to determine what curve is needed
                         *           _
                         *         _|S|
                         *    _  _|N|
                         *  _|P||x|
                         * |2|
                         * 
                         * At a point in a hill that is slightly flat.
                         * Will create a steep curve to smooth the transition between the curve from the previous square (P) to the next square (N)
                         * 
                         * Second Scenario:
                         *         _
                         *       _|S|
                         *  _  _|N|
                         * |P||x| -- You are here (x)
                         * 
                         * At the bottom of a hill starting to go up (First square to change elevation from flat terrain)
                         * Will curve the mesh from the start of the previous square (P) to the end of the current square (x).
                         */
                        else
                        {
                            Square x2PreviousSquare;
                            if (x < 2)
                            {
                                x2PreviousSquare = previousSquare;
                            }
                            else
                            {
                                x2PreviousSquare = boundary[x - 2];
                            }

                            if (x2PreviousSquare.y < previousSquare.y)
                            {
                                collisionPoints = calculateQuadraticCurve(boundary[x].topLeft, new Vector2(boundary[x].topLeft.position.x + 0.15f, boundary[x].topLeft.position.y), nextSquare.topLeft);
                                AssignCurveVertices(collisionPoints, boundary[x].topRight.position);
                            }
                            else
                            {
                                collisionPoints = calculateQuadraticCurve(previousSquare.topLeft, new Vector2(previousSquare.topLeft.position.x + 1, nextSquare.y), nextSquare.topLeft);
                                AssignCurveVertices(collisionPoints, nextSquare.bottomLeft.position);
                            }
                        }
                    }
                    /*    _  _
                    *   _|N||S|
                    *  |x|^ 
                    * When the elevation will only be going up for the next square
                    */
                    else
                    {

                        /*       _  _
                         *     _|N||S|
                         *   _|x| -- You are here (x)
                         *  |P|
                         *  
                         *  Is at the end of a hill (Previous square was lower but the next square goes back to flat terrain)
                         *  Will curve the mesh from the current square (x) to the end of the next square (N)
                         */
                        if (previousSquare.y < boundary[x].y)
                        {
                            collisionPoints = calculateQuadraticCurve(boundary[x].topLeft, new Vector2(nextSquare.topLeft.position.x - 0.25f, nextSquare.topLeft.position.y), nextSquare.topRight);
                            AssignCurveVertices(collisionPoints, nextSquare.bottomRight.position);
                            AssignSquareMesh(boundary[x].bottomLeft.position, boundary[x].bottomRight.position, new Vector2(boundary[x].x, 0), new Vector2(boundary[x].bottomRight.position.x, 0));
                            x++; //Skip over next boundary square as mesh has already been drawn for it

                        }

                        /*
                         * When a small elevation change occurs that doesn't match the other criteria (Normally occurs if elevation changes by 1 square but rest is flat terrain)
                         * 
                         * Example:
                         *        _  _
                         *   _  _|N||S|
                         *  |P||x| -- You are here(x)
                         *  
                         *  Will curve the mesh from the start of the current square (x) to the end of the next square (N)
                         */
                        else
                        {
                            collisionPoints = calculateCubicCurve(boundary[x].topLeft, new Vector2(boundary[x].topLeft.position.x + 1, boundary[x].topRight.position.y), new Vector2(nextSquare.topRight.position.x - 1, nextSquare.topRight.position.y), nextSquare.topRight);
                            AssignCurveVertices(collisionPoints, nextSquare.bottomRight.position);
                            AssignSquareMesh(boundary[x].bottomLeft.position, boundary[x].bottomRight.position, new Vector2(boundary[x].x, 0), new Vector2(boundary[x].bottomRight.position.x, 0));
                            x++;
                        }

                    }

                    sectionCollider.addPoint(collisionPoints);
                }
                /*  _
                 * |x|_
                 *  >|N|
                 *   
                 * When the elevation is going down
                 */
                else if (nextSquare.y < boundary[x].y)
                {
                    /*  _
                     * |x|_
                     *   |N|_
                     *    >|S|
                     * When the elevation will still be going down for the next 2 squares
                     */
                    if (nextNextSquare.y < nextSquare.y)
                    {
                        /*  _
                         * |P|_
                         *   |x|_
                         *     |N|_
                         *       |S|
                         * Is currently going down a hill (Previous squares was higher and next squares are lower than the current square)
                         * Will create a straight diagonal line from the current square (x) to the next square (N)
                         * 
                         */
                        if (previousSquare.y > boundary[x].y)
                        {
                            AssignSquareMesh(boundary[x]);
                            AssignTriangleMesh(boundary[x].topRight.position, nextSquare.topRight.position, boundary[x].bottomRight.position);
                            collisionPoints = new Vector2[2] { boundary[x].topRight.position, nextSquare.topRight.position };
                        }
                        /*  _  _
                         * |P||x|_
                         *      |N|_
                         *        |S|
                         * 
                         * At the top of a hill starting to go down (First square to change elevation from flat terrain)
                         * Will curve the mesh from the start of the current square (x) to the end of the next square (N).
                         */
                        else
                        {
                            collisionPoints = calculateQuadraticCurve(boundary[x].topLeft, new Vector2(boundary[x].topRight.position.x, boundary[x].topLeft.position.y), nextSquare.topRight);
                            AssignCurveVertices(collisionPoints, boundary[x].bottomLeft.position);
                        }
                    }
                    /*  _
                     * |x|_  _
                     *  >|N||S|
                     *  
                     * When the elevation will only be going down for the next square
                     */
                    else
                    {
                        /*This statement contains two different possible scenarios and creates the mesh accordingly. 
                         * 
                         * First scenario uses a square 3 steps forward to determine the curve
                         *   _
                         *  |P|_
                         *    |x|_  _
                         *      |N||S|_
                         *           |3|
                         *    
                         * There is a flat point in the hill
                         * Will curve the mesh between the current square (x) and the end of the next square (S)
                         * 
                         * Second Scenario:
                         *   _        
                         *  |P|_
                         *    |x|_  _
                         *      |N||S|
                         *  
                         *  Is at the end of a hill (Previous square was higher but the next square goes back to flat terrain)
                         *  Will curve the mesh from the end of current square (x) to the end of the next next square (S)
                         */
                        if (previousSquare.y > boundary[x].y)
                        {
                            AssignSquareMesh(boundary[x]);
                            Square x3NextSquare;
                            if (x > boundary.Length - 3)
                            {
                                x3NextSquare = nextNextSquare;
                            }
                            else
                            {
                                x3NextSquare = boundary[x + 3];
                            }
                            if (x3NextSquare.y < nextNextSquare.y)
                            {
                                collisionPoints = calculateQuadraticCurve(boundary[x].topRight, new Vector2(nextSquare.topRight.position.x - 0.2f, nextSquare.topRight.position.y), nextSquare.topRight);
                                AssignCurveVertices(collisionPoints, nextSquare.topLeft.position);
                            }
                            else
                            {
                                
                                collisionPoints = calculateQuadraticCurve(boundary[x].topRight, new Vector2(nextSquare.topRight.position.x - 0.25f, nextSquare.topRight.position.y), nextNextSquare.topRight);
                                AssignCurveVertices(collisionPoints, boundary[x].bottomRight.position);
                            }
                        }
                        /*
                        * When a small elevation change occurs that doesn't match the other criteria (Normally occurs if elevation changes by 1 square but rest is flat terrain)
                        * 
                        * Example:
                        *   _  _
                        *  |P||x|_  _
                        *       |N||S|
                        *  Will curve the mesh from the start of the current square (x) to the end of the next square (N)
                        */
                        else
                        {
                            AssignSquareMesh(boundary[x + 1]);
                            collisionPoints = calculateCubicCurve(boundary[x].topLeft, new Vector2(boundary[x].topLeft.position.x + 1, boundary[x].topLeft.position.y), new Vector2(nextSquare.topLeft.position.x, nextSquare.topLeft.position.y), nextSquare.topRight);
                            AssignCurveVertices(collisionPoints, boundary[x].bottomLeft.position);
                            AssignSquareMesh(boundary[x].bottomLeft.position, boundary[x].bottomRight.position, new Vector2(boundary[x].x, 0), new Vector2(boundary[x].bottomRight.position.x, 0));
                            x++;
                        }
                    }

                    sectionCollider.addPoint(collisionPoints);
                }
                /*
                 * When the elevation remains the same for the next square
                 *     _  _
                 * ...|x||N|...
                 */
                else if (nextSquare.y == boundary[x].y)
                {
                    AssignSquareMesh(boundary[x]);
                    sectionCollider.addPoint(boundary[x].topRight.position);

                }
                AssignSquareMesh(boundary[x].bottomLeft.position, boundary[x].bottomRight.position, new Vector2(boundary[x].x, 0), new Vector2(boundary[x].bottomRight.position.x, 0));
            }
        }

        //Add the last two squares to the section as the nextSquare variables don't apply to these last squares
        for (int i = boundary.Length - 2; i < boundary.Length; i++)
        {
            AssignSquareMesh(boundary[i]);
            AssignSquareMesh(boundary[i].bottomLeft.position, boundary[i].bottomRight.position, new Vector2(boundary[i].x, 0), new Vector2(boundary[i].bottomRight.position.x, 0));
            GetComponent<SectionCollider>().addPoint(boundary[i].topRight.position);
        }
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = vertices.ToArray(); //Assigns the vertices and triangles to the mesh to be generated
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    /*
     * Clear all mesh vertices, triangles & collider points associated with this object
     */
    public void ClearMesh()
    {
        mesh.Clear();
        triangles.Clear();
        vertices.Clear();
        vertIndex = 0;
        GetComponent<SectionCollider>().removePoints();
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

    /*
     * Simplified method to create a square mesh for a specific square. Calls the other AssignSquareMesh method and passes in this squares node coordinates.
     */
    void AssignSquareMesh(Square square)
    {
        AssignSquareMesh(square.topLeft.position, square.topRight.position, square.bottomLeft.position, square.bottomRight.position);
    }

    /*
     * Create a square mesh. This method uses four coordinates to determine the square shape and add the vertices and triangles appropriately.
     * 
     * This method can also be used to make rectangles, is not specific to squares.
     */ 
    void AssignSquareMesh(Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight)
    {
        vertices.Add(bottomLeft);
        vertices.Add(topLeft);
        vertices.Add(topRight);
        vertices.Add(bottomRight);

        for (int i = 0; i < 2; i++)
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

    private void AssignNodes(Square[] boundary)
    {
        for (int x = 0; x < boundary.Length; x++)
        {
            boundary[x].bottomLeft = new Node(new Vector3(boundary[x].x, boundary[x].y, 0));
            boundary[x].topLeft = new Node(new Vector3(boundary[x].x, boundary[x].y + 1, 0));

            if (x != 0)
            {
                if (boundary[x].y == boundary[x - 1].y)
                {
                    boundary[x - 1].bottomRight = boundary[x].bottomLeft;
                    boundary[x - 1].topRight = boundary[x].topLeft;
                }
                else if (boundary[x].y < boundary[x - 1].y) 
                {
                    boundary[x - 1].topRight = new Node(new Vector3(boundary[x-1].x + 1, boundary[x-1].y + 1, 0));
                    boundary[x - 1].bottomRight = boundary[x].topLeft;
                } else if (boundary[x].y > boundary[x - 1].y) {
                    boundary[x - 1].topRight = boundary[x].bottomLeft;
                    boundary[x - 1].bottomRight = new Node(new Vector3(boundary[x-1].x + 1, boundary[x-1].y, 0));
                }
            }

            if (x == boundary.Length - 1)
            {
                boundary[x].topRight = new Node(new Vector3(boundary[x].x + 1, boundary[x].y + 1, 0));
                boundary[x].bottomRight = new Node(new Vector3(boundary[x].x + 1, boundary[x].y, 0));
            }
        }
    }
}

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
        for (int x = 1; x < boundarySquares.Length - 2; x++)
        {
            Square previousSquare = GetComponent<MapGenerator>().FindBoundarySquare(x - 1); 
            Square nextSquare = GetComponent<MapGenerator>().FindBoundarySquare(x + 1);
            Square nextNextSquare = GetComponent<MapGenerator>().FindBoundarySquare(x + 2);
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
                if (nextSquare.y > boundarySquares[x].y)
                {
                    AssignSquareMesh(boundarySquares[x]); //Creates the mesh for the current square

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
                        if (previousSquare.y < boundarySquares[x].y)
                        {
                            AssignTriangleMesh(boundarySquares[x].topLeft.position, nextSquare.topLeft.position, boundarySquares[x].topRight.position);
                            collisionPoints = new Vector2[2] { boundarySquares[x].topLeft.position, nextSquare.topLeft.position };
                        }
                        /*This statement contains two different possible scenarios and creates the mesh accordingly. 
                         * 
                         * First Scenario uses the previous square from 2 squares back to determine what curve is needed
                         *           _
                         *         _|S|
                         *    _  _|N|
                         *  _|P||x|
                         * |P|
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
                            Square x2PreviousSquare = GetComponent<MapGenerator>().FindBoundarySquare(x - 2);

                            if (x2PreviousSquare.y < previousSquare.y)
                            {
                                collisionPoints = calculateQuadraticCurve(boundarySquares[x].topLeft, new Vector2(boundarySquares[x].topLeft.position.x + 0.15f, boundarySquares[x].topLeft.position.y), nextSquare.topLeft);
                                AssignCurveVertices(collisionPoints, boundarySquares[x].topRight.position);
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
                        if (previousSquare.y < boundarySquares[x].y)
                        {
                            collisionPoints = calculateQuadraticCurve(boundarySquares[x].topLeft, new Vector2(nextSquare.topLeft.position.x - 0.25f, nextSquare.topLeft.position.y), nextSquare.topRight);
                            AssignCurveVertices(collisionPoints, nextSquare.bottomRight.position);
                            AssignSquareMesh(boundarySquares[x].bottomLeft.position, boundarySquares[x].bottomRight.position, new Vector2(boundarySquares[x].x, 0), new Vector2(boundarySquares[x].bottomRight.position.x, 0));
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
                            collisionPoints = calculateCubicCurve(boundarySquares[x].topLeft, new Vector2(boundarySquares[x].topLeft.position.x + 1, boundarySquares[x].topRight.position.y), new Vector2(nextSquare.topRight.position.x - 1, nextSquare.topRight.position.y), nextSquare.topRight);
                            AssignCurveVertices(collisionPoints, nextSquare.bottomRight.position);
                            AssignSquareMesh(boundarySquares[x].bottomLeft.position, boundarySquares[x].bottomRight.position, new Vector2(boundarySquares[x].x, 0), new Vector2(boundarySquares[x].bottomRight.position.x, 0));
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
                else if (nextSquare.y < boundarySquares[x].y)
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
                        if (previousSquare.y > boundarySquares[x].y)
                        {
                            AssignSquareMesh(boundarySquares[x]);
                            AssignTriangleMesh(boundarySquares[x].topRight.position, nextSquare.topRight.position, boundarySquares[x].bottomRight.position);
                            collisionPoints = new Vector2[2] { boundarySquares[x].topRight.position, nextSquare.topRight.position };
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
                            collisionPoints = calculateQuadraticCurve(boundarySquares[x].topLeft, new Vector2(boundarySquares[x].topRight.position.x, boundarySquares[x].topLeft.position.y), nextSquare.topRight);
                            AssignCurveVertices(collisionPoints, boundarySquares[x].bottomLeft.position);
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
                        /*   _
                         *  |P|_
                         *    |x|_  _
                         *      |N||S|
                         *  
                         *  Is at the end of a hill (Previous square was higher but the next square goes back to flat terrain)
                         *  Will curve the mesh from the end of current square (x) to the end of the next next square (S)
                         */
                        if (previousSquare.y > boundarySquares[x].y)
                        {
                            AssignSquareMesh(boundarySquares[x]);
                            collisionPoints = calculateQuadraticCurve(boundarySquares[x].topRight, new Vector2(nextSquare.topRight.position.x - 0.25f, nextSquare.topRight.position.y), nextNextSquare.topRight);
                            AssignCurveVertices(collisionPoints, boundarySquares[x].bottomRight.position);
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
                            AssignSquareMesh(boundarySquares[x + 1]);
                            collisionPoints = calculateCubicCurve(boundarySquares[x].topLeft, new Vector2(boundarySquares[x].topLeft.position.x + 1, boundarySquares[x].topLeft.position.y), new Vector2(nextSquare.topLeft.position.x, nextSquare.topLeft.position.y), nextSquare.topRight);
                            AssignCurveVertices(collisionPoints, boundarySquares[x].bottomLeft.position);
                            AssignSquareMesh(boundarySquares[x].bottomLeft.position, boundarySquares[x].bottomRight.position, new Vector2(boundarySquares[x].x, 0), new Vector2(boundarySquares[x].bottomRight.position.x, 0));
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
                else if (nextSquare.y == boundarySquares[x].y)
                {
                    AssignSquareMesh(boundarySquares[x]);
                    sectionCollider.addPoint(boundarySquares[x].topRight.position);

                }
                AssignSquareMesh(boundarySquares[x].bottomLeft.position, boundarySquares[x].bottomRight.position, new Vector2(boundarySquares[x].x, 0), new Vector2(boundarySquares[x].bottomRight.position.x, 0));
            }
        }

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = vertices.ToArray(); //Assigns the vertices and triangles to the mesh to be generated
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

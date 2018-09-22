using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class MeshGeneratorTest {
    
    MeshGenerator meshGeneratorTest = new MeshGenerator();

    [Test]
    public void MeshGenerator_Lerp_ValidValueTest() {

        Vector2 expected = new Vector2(0.5f, 0.5f);
        Vector2 actual = meshGeneratorTest.Lerp(new Vector2(0, 0), new Vector2(1, 1), 0.5f);

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void MeshGenerator_Lerp_NegativeValueTest()
    {
        Vector2 expected = new Vector2(-0.5f, -0.5f);
        Vector2 actual = meshGeneratorTest.Lerp(new Vector2(0, 0), new Vector2(-1, -1), 0.5f);

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void MeshGenerator_AssignNodes_SideBySideTest()
    {
        Square[] test = new Square[2] { new Square(0, 0), new Square(1, 0) };
        meshGeneratorTest.AssignNodes(test);

        Assert.AreSame(test[0].topRight, test[1].topLeft);
    }

    [Test]
    public void MeshGenerator_AssignNodes_DiagonalTest()
    {
        Square[] test = new Square[2] { new Square(0, 0), new Square(1, 1) };
        meshGeneratorTest.AssignNodes(test);

        Assert.AreSame(test[0].topRight, test[1].bottomLeft);
    }

    [Test]
    public void MeshGenerator_AssignNodes_NotSameTest()
    {
        Square[] test = new Square[3] { new Square(0, 0), new Square(1, 0), new Square(2,0) };
        meshGeneratorTest.AssignNodes(test);

        Assert.AreNotSame(test[0].topRight, test[2].topLeft);
    }

    [Test]
    public void MeshGenerator_AssignSquareMesh_VerticesTest()
    {
        Square square = new Square(0, 0);
        meshGeneratorTest.AssignNodes(new Square[] { square });
        meshGeneratorTest.AssignSquareMesh(square);

        //Must pass all asserts to pass test
        Assert.AreEqual(meshGeneratorTest.GetVertices[0], square.bottomLeft.position);
        Assert.AreEqual(meshGeneratorTest.GetVertices[1], square.topLeft.position);
        Assert.AreEqual(meshGeneratorTest.GetVertices[2], square.topRight.position);
        Assert.AreEqual(meshGeneratorTest.GetVertices[3], square.bottomRight.position);
    }

    [Test]
    public void MeshGenerator_AssignSquareMesh_TrianglesTest()
    {
        Square square = new Square(0, 0);
        meshGeneratorTest.AssignNodes(new Square[] { square });
        meshGeneratorTest.AssignSquareMesh(square);

        Assert.AreEqual(new List<int> { 0, 1, 2, 0, 2, 3 }, meshGeneratorTest.GetTriangles); //Triangle array should look like this
    }
}

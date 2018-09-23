using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class MeshGeneratorTest {
    
    MeshGenerator meshGeneratorTest = new MeshGenerator();

    [Test]
    public void MeshGenerator_Lerp_ValidValue() {

        Vector2 expected = new Vector2(0.5f, 0.5f);
        Vector2 actual = meshGeneratorTest.Lerp(new Vector2(0, 0), new Vector2(1, 1), 0.5f);

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void MeshGenerator_Lerp_NegativeValue()
    {
        Vector2 expected = new Vector2(-0.5f, -0.5f);
        Vector2 actual = meshGeneratorTest.Lerp(new Vector2(0, 0), new Vector2(-1, -1), 0.5f);

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void MeshGenerator_AssignNodes()
    {
        Square test = new Square(0, 0);
        meshGeneratorTest.AssignNodes(new Square[] { test });

        Assert.NotNull(test.topRight);
        Assert.NotNull(test.topLeft);
        Assert.NotNull(test.bottomLeft);
        Assert.NotNull(test.bottomRight);
    }
    
    [Test]
    public void MeshGenerator_AssignNodes_SideBySide()
    {
        Square[] test = new Square[2] { new Square(0, 0), new Square(1, 0) };
        meshGeneratorTest.AssignNodes(test);
        
        Assert.AreSame(test[0].topRight, test[1].topLeft);
    }
    
    [Test]
    public void MeshGenerator_AssignNodes_Diagonal()
    {
        Square[] test = new Square[2] { new Square(0, 0), new Square(1, 1) };
        meshGeneratorTest.AssignNodes(test);

        Assert.AreSame(test[0].topRight, test[1].bottomLeft);
    }

    [Test]
    public void MeshGenerator_AssignNodes_NotSame()
    {
        Square[] test = new Square[3] { new Square(0, 0), new Square(1, 0), new Square(2,0) };
        meshGeneratorTest.AssignNodes(test);

        Assert.AreNotSame(test[0].topRight, test[2].topLeft);
    }
    
    [Test]
    public void MeshGenerator_AssignSquareMesh_Vertices()
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
    public void MeshGenerator_AssignSquareMesh_Triangles()
    {
        Square square = new Square(0, 0);
        meshGeneratorTest.AssignNodes(new Square[] { square });
        meshGeneratorTest.AssignSquareMesh(square);

        Assert.AreEqual(new List<int> { 0, 1, 2, 0, 2, 3 }, meshGeneratorTest.GetTriangles); //Triangle array should look like this
    }
    
}

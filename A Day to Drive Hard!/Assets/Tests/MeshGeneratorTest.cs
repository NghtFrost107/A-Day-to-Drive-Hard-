using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MeshGeneratorTest {
    
    MeshGenerator meshGeneratorTest = new MeshGenerator();

    [Test]
    public void MeshGenerator_Lerp_ValidValue() {

        Vector2 expected = new Vector2(0.4f, 0.5f);
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


    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator MeshGeneratorTestWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }
}

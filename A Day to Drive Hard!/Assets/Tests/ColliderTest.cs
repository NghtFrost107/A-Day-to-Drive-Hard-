using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;


public class ColliderTest  {

    SectionCollider collider = new SectionCollider();

    [Test]
    public void SectionCollider_Add_Point_To_ColliderTest()
    {
        Vector2 point = new Vector2(10, 20);
        collider.addPoint(point);

        Assert.AreEqual(collider.points[0], point);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionCollider : MonoBehaviour {

    public PolygonCollider2D mainCollider;

    private List<Vector2> points;

    public SectionCollider()
    {
        points = new List<Vector2>();
    }

    public void addCollider()
    {
        points.Add(new Vector2(points[points.Count - 1].x, 0));
        points.Add(new Vector2(0, 0));
        mainCollider.SetPath(0,points.ToArray());
    }

    public void addPoint(MainNode point)
    {
        points.Add(Vector3To2(point.position));
    }

    public static Vector2 Vector3To2(Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

}

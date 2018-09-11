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

    public void addCollider(int xStart)
    {
        points.Add(new Vector2(points[points.Count - 1].x, 0));
        points.Add(new Vector2(xStart, 0));
        mainCollider.SetPath(0,points.ToArray());
    }

    public void addPoint(Vector2 point)
    {
        points.Add(point);
    }

    public void addPoint(Vector2[] pointArray)
    {
        foreach (Vector2 point in pointArray)
        {
            points.Add(point);
        }
    }
}

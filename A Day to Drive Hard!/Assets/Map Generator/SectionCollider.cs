using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionCollider : MonoBehaviour {

    public PolygonCollider2D mainCollider;

    List<Vector2> points;

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

    public void removePoints()
    {
        points.Clear();
    }
}

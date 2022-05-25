using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObject : MonoBehaviour
{
    public Point Raw;
    public Point Point;

    public Renderer Renderer;

    [ContextMenu("UpdatePosition")]
    public void UpdatePosition() {
        this.transform.localPosition = new Vector3(Point.X, Point.Y, 0);
    }

    public void UpdatePosition(Rect source, Rect target) {
        Debug.Log(Point);
        this.transform.localPosition = new Vector3((Point.X/source.width)*target.width,(Point.Y/source.height)/target.width);
    }
}

[System.Serializable]
public class Point {
    public float X;
    public float Y;

    public Point (Vector3 vector) {
        X = vector.x;
        Y = vector.y;
    }

    public Point (float x, float y) {
        X = x;
        Y = y;
    }

    public Point (Transform t) {
        X = t.position.x;
        Y = t.position.y;
    }

    public override string ToString()
    {
        return "("+X + ", " + Y+")";
    }
}

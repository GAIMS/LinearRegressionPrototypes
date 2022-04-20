using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObject : MonoBehaviour
{
    public Point Point;

    public Renderer Renderer;

    [ContextMenu("UpdatePosition")]
    public void UpdatePosition() {
        this.transform.localPosition = new Vector3(Point.X, Point.Y, 0);
    }
}

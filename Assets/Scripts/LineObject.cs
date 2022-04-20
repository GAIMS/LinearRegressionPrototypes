using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineObject : MonoBehaviour
{
    public Line Line;

    public LineRenderer LineRenderer;

    [ContextMenu("Update Position")]
    public void UpdatePosition() {
        Vector3[] positions =  new Vector3[2];
        positions[0] = new Vector3(Line.A.X, Line.A.Y, 0);
        positions[1] = new Vector3(Line.B.X, Line.B.Y, 0);
        LineRenderer.SetPositions(positions);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPlacer : MonoBehaviour
{
    public GraphObject Graph;
    public int maxPoints;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (Graph.Points.Count >= maxPoints)
            {
                Destroy(Graph.Points[0].gameObject);
                Graph.Points.RemoveAt(0);
                Graph.Points.TrimExcess();
            }
            Graph.AddPoint(Input.mousePosition, true);
            Graph.RedrawLine();
        }
    }
}

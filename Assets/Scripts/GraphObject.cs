using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphObject : MonoBehaviour
{
    [Header("General")]
    public Camera Camera;
    public Rect Rect;
    public LineObject Line;
    public List<PointObject> Points;
    public GameObject PointContainer;
    [Header("Graph Data")]
    public int PointCount;
    [Header("Rect Rendering")]
    public List<PointObject> Corners;
    public List<LineObject> Sides;
    public bool DrawCorners = true;
    public bool DrawSides = true;
    // graph stuff?
    [Header("Prefabs")]
    public LineObject LinePrefab;
    public PointObject PointPrefab;

    [ContextMenu("Redraw Graph")]
    public void RedrawGraph() {
        Rect.x = this.transform.position.x;
        Rect.y = this.transform.position.y;
        RedrawRect();
        RedrawGrid();
        RedrawPoints();
        RedrawLine();
    }

    [ContextMenu("Redraw Rect")]
    public void RedrawRect() {
        RedrawCorners();
        RedrawSides();
        // redraw grid lines?
    }

    [ContextMenu("Redraw Grid")]
    public void RedrawGrid() {

    }

    [ContextMenu("Redraw Points")]
    public void RedrawPoints() {
        ClearPoints();
        GameObject empty = new GameObject();
        empty.transform.SetParent(PointContainer.transform);
        empty.transform.localPosition = new Vector3(0,0,0);
        for (int i = 0; i < PointCount; i++) {
            PointObject p = Instantiate(PointPrefab, empty.transform);
            p.Point = new Point(Random.Range(Rect.x - (Rect.width/2), Rect.x + (Rect.width/2)), Random.Range(Rect.y - (Rect.height/2), Rect.y + (Rect.height/2)));
            p.UpdatePosition();
            Points.Add(p);
        }
        RedrawLine();
    }

    [ContextMenu("Clear Points")]
    public void ClearPoints() {
        if (PointContainer.transform.childCount > 0) {
            Object.DestroyImmediate(PointContainer.transform.GetChild(0).gameObject);
            Points.Clear();
        }
        Line.Line = new Line(new Point(0,0), new Point(0,0));
        Line.UpdatePosition();
    }

    [ContextMenu("Redraw Line")]
    public void RedrawLine() {
        RedrawLine(Points);
    }

    [ContextMenu("Redraw Corners")]
    public void RedrawCorners() {
        for (int i = 0; i < 4; i++) {
            if (Corners.Count <= i || Corners[i] == null) {
                Corners.Add(Instantiate(PointPrefab));
            }
            switch (i) {
                case 0:
                    Corners[i].Point = new Point(Rect.x - (Rect.width/2), Rect.y - (Rect.height/2));
                    break;
                case 1:
                    Corners[i].Point = new Point(Rect.x + (Rect.width/2), Rect.y - (Rect.height/2));
                    break;
                case 2:
                    Corners[i].Point = new Point(Rect.x - (Rect.width/2), Rect.y + (Rect.height/2));
                    break;
                case 3:
                    Corners[i].Point = new Point(Rect.x + (Rect.width/2), Rect.y + (Rect.height/2));
                    break;
                
            }
            Corners[i].UpdatePosition();
            if (DrawCorners == false) {
                Corners[i].gameObject.SetActive(false);
            } else {
                Corners[i].gameObject.SetActive(true);
            }
        }
    }

    [ContextMenu("Redraw Sides")]
    public void RedrawSides() {
        if (Corners.Count == 4) {
            for (int i = 0; i < 4; i++) {
                if (Sides.Count <= i || Sides[i] == null) {
                    Sides.Add(Instantiate(LinePrefab));
                }
                Vector3[] positions =  new Vector3[2];
                switch (i) {
                    case 0:
                        Sides[i].Line = new Line(new Point(Corners[0].Point.X, Corners[0].Point.Y), new Point(Corners[1].Point.X, Corners[1].Point.Y));
                        break;
                    case 1:
                        Sides[i].Line = new Line(new Point(Corners[2].Point.X, Corners[2].Point.Y), new Point(Corners[3].Point.X, Corners[3].Point.Y));
                        break;
                    case 2:
                        Sides[i].Line = new Line(new Point(Corners[0].Point.X, Corners[0].Point.Y), new Point(Corners[2].Point.X, Corners[2].Point.Y));
                        break;
                    case 3:
                        Sides[i].Line = new Line(new Point(Corners[1].Point.X, Corners[1].Point.Y), new Point(Corners[3].Point.X, Corners[3].Point.Y));
                        break;
                }
                Sides[i].UpdatePosition();
                if (DrawSides == false) {
                    Sides[i].gameObject.SetActive(false);
                } else {
                    Sides[i].gameObject.SetActive(true);
                }
            }
        } else {
            Debug.LogError("Incorrect number of corners");
        }
    }

    public void RedrawLine(List<PointObject> _points) {
        // get our line of best fit
        List<Point> points = new List<Point>();
        foreach (PointObject _p in _points) {
            points.Add(_p.Point);
        }
        Line.Line = LineOfBestFit(points);

        // find our points of intersection
        List<Point> intersects = new List<Point>();
        try {
            intersects.Add(LineIntersection.FindIntersection(Line.Line, Sides[0].Line));
        } catch (System.SystemException e) {
            Debug.LogError("bottom line error");
            Debug.LogError(e);
        }
        try {
            intersects.Add(LineIntersection.FindIntersection(Line.Line, Sides[1].Line));
        } catch (System.SystemException e) {
            Debug.LogError("top line error");
            Debug.LogError(e);
        }
        try {
            intersects.Add(LineIntersection.FindIntersection(Line.Line, Sides[2].Line));
        } catch (System.SystemException e) {
            Debug.LogError("left line error");
            Debug.LogError(e);
        }
        try {
            intersects.Add(LineIntersection.FindIntersection(Line.Line, Sides[3].Line));
        } catch (System.SystemException e) {
            Debug.LogError("right line error");
            Debug.LogError(e);
        }

        // find the points which are within the bounds of teh screen edge
        List<Point> rectIntersects = new List<Point>();
        for (int i = 0; i < intersects.Count; i++) {
            Debug.Log(intersects[i]);
            if (intersects[i].X >= Corners[0].Point.X && intersects[i].X <= Corners[1].Point.X) {
                if (intersects[i].Y >= Corners[0].Point.Y && intersects[i].Y <= Corners[2].Point.Y) {
                    rectIntersects.Add(intersects[i]);
                }
            }
        }

        if (rectIntersects.Count == 2) {
            // build line renderer position values
            Line.Line = new Line(new Point(rectIntersects[0].X, rectIntersects[0].Y), new Point(rectIntersects[1].X, rectIntersects[1].Y));
            Line.UpdatePosition();
        } else if (rectIntersects.Count == 1) {
            Debug.LogError("Only 1 screen intersection, this shouldn't be possible.");
        } else if (rectIntersects.Count > 2) {
            Debug.LogError("More than 2 screen intersections, this shouldn't be possible.");
        } else {
            Debug.LogWarning("Not enough intersects to draw line");
        }
    }

    public Line LineOfBestFit(List<Point> points) {
        //average our points
        float meanX = points.Average(point => point.X);
        float meanY = points.Average(point => point.Y);

        //cacluate our slope
        float sumXY = points.Sum(point => point.X * point.Y);
        float sumXX = points.Sum(point => point.X * point.X);
        float slope = sumXY/sumXX;
        float yIntercept = meanY - (slope * meanX);
        float xIntercept = (0 - yIntercept) / slope;

        return new Line(new Point(0,yIntercept), new Point(xIntercept, 0));
    }

    public void AddPoint(Vector3 position, bool screenSpace = false, bool redrawLine = true) {
        Vector3 _position = position;
        if (screenSpace) {
            _position = Camera.ScreenToWorldPoint(position);
        }
        Point p = new Point(_position);
        if (p.X >= (Rect.x - (Rect.width/2)) && p.X <= (Rect.x + (Rect.width/2)) && p.Y >= (Rect.y - (Rect.height/2)) && p.Y <= (Rect.y + (Rect.height/2))) {
            PointObject point = Instantiate(PointPrefab, PointContainer.transform);
            point.Point = p;
            point.UpdatePosition();
            Points.Add(point);
            if (redrawLine) {
                RedrawLine();
            }
        } else {
            Debug.LogError("New Point is out of bounds");
        }
    }
}

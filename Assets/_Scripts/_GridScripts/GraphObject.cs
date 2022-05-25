using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class GraphObject : MonoBehaviour
{
    [Header("General")]
    public bool _Debug = false;
    public GraphMode Mode;
    public Camera Camera;
    public Rect Rect;
    public Rect Space;
    public LineObject Line;
    public GameObject LineContainer;
    public List<PointObject> Points;
    public GameObject PointContainer;
    [Header("Graph Data")]
    public int PointCount;
    [Header("Rect Rendering")]
    public List<PointObject> Corners;
    public List<LineObject> Sides;
    public bool DrawCorners = true;
    public bool DrawSides = true;
    public bool DrawLossLines = true;
    // graph stuff?
    [Header("Prefabs")]
    public LineObject LinePrefab;
    public PointObject PointPrefab;

    [ContextMenu("Redraw Graph")]
    public void RedrawGraph(){
        RedrawGraph(true, true);
    }

    public void RedrawGraph(bool redrawPoints, bool redrawLine) {
        if (_Debug) Debug.Log("RedrawGraph");
        //Rect.x = this.transform.position.x;
        //Rect.y = this.transform.position.y;
        RedrawRect();
        RedrawGrid();
        if (redrawPoints) {
            RedrawPoints();
        }
        if (redrawLine) {
            RedrawLine();
        }
        if (DrawLossLines == true) {
            RedrawLossLines();
        } else {
            ClearLossLines();
        }
    }

    [ContextMenu("Redraw Rect")]
    public void RedrawRect() {
        if (_Debug) Debug.Log("RedrawRect");
        RedrawCorners();
        RedrawSides();
        // redraw grid lines?
    }

    [ContextMenu("Redraw Grid")]
    public void RedrawGrid() {
        if (_Debug) Debug.Log("RedrawGrid");
    }

    [ContextMenu("Redraw Points")]
    public void RedrawPoints() {
        if (_Debug) Debug.Log("Redraw Points");
        ClearPoints();
        GameObject empty = new GameObject();
        empty.transform.SetParent(PointContainer.transform);
        empty.transform.localPosition = new Vector3(0,0,0);
        for (int i = 0; i < PointCount; i++) {
            PointObject p = Instantiate(PointPrefab, empty.transform);
            // change this to be based off the Space variable
            p.Point = new Point(Random.Range(Rect.x - (Rect.width/2), Rect.x + (Rect.width/2)), Random.Range(Rect.y - (Rect.height/2), Rect.y + (Rect.height/2)));
            p.UpdatePosition();
            Points.Add(p);
        }
        RedrawLine();
    }

    [ContextMenu("Clear Points")]
    public void ClearPoints() {
        if (_Debug) Debug.Log("Clear Points");
        if (PointContainer.transform.childCount > 0) {
            for (int i = PointContainer.transform.childCount - 1; i >= 0; i--) {
                if (Application.isEditor)
                    GameObject.DestroyImmediate(PointContainer.transform.GetChild(i).gameObject);
                else
                    GameObject.Destroy(PointContainer.transform.GetChild(i).gameObject);
            }
            Points.Clear();
        }
        Line.Line = new Line(new Point(0,0), new Point(0,0));
        Line.UpdatePosition();
    }

    [ContextMenu("Redraw Corners")]
    public void RedrawCorners() {
        if (_Debug) Debug.Log("Redraw Corners");
        for (int i = 0; i < 4; i++) {
            if (Corners.Count <= i || Corners[i] == null) {
                Corners.Add(Instantiate(PointPrefab));
            }
            switch (i) {
                case 0:
                    if (Mode == GraphMode.PositiveY) {
                        Corners[i].Point = new Point(Rect.x - (Rect.width/2), Rect.y);
                    } else {
                        Corners[i].Point = new Point(Rect.x - (Rect.width/2), Rect.y - (Rect.height/2));
                    }
                    break;
                case 1:
                    if (Mode == GraphMode.PositiveY) {
                        Corners[i].Point = new Point(Rect.x + (Rect.width/2), Rect.y);
                    } else {
                        Corners[i].Point = new Point(Rect.x + (Rect.width/2), Rect.y - (Rect.height/2));
                    }
                    break;
                case 2:
                    if (Mode == GraphMode.PositiveY) {
                        Corners[i].Point = new Point(Rect.x - (Rect.width/2), Rect.y + Rect.height);
                    } else {
                        Corners[i].Point = new Point(Rect.x - (Rect.width/2), Rect.y + (Rect.height/2));
                    }
                    break;
                case 3:
                    if (Mode == GraphMode.PositiveY) {
                        Corners[i].Point = new Point(Rect.x + (Rect.width/2), Rect.y + Rect.height);
                    } else {
                        Corners[i].Point = new Point(Rect.x + (Rect.width/2), Rect.y + (Rect.height/2));
                    }
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
        if (_Debug) Debug.Log("RedrawSides");
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

    [ContextMenu("Redraw Line")]
    public void RedrawLine() {
        RedrawLine(Points);
    }

    public void RedrawLine(List<PointObject> _points) {
        // get our line of best fit
        if (_Debug) Debug.Log("Redraw Line from points");
        List<Point> points = new List<Point>();
        foreach (PointObject _p in _points) {
            points.Add(_p.Point);
        }
        Line line = LineOfBestFit(points);
        RedrawLine(line);
    }

    public void RedrawLine(Line line) {
        if (_Debug) Debug.Log("RedrawLineTrue");
        Line.Line = line;
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
            if (intersects[i] != null && intersects[i].X >= Corners[0].Point.X && intersects[i].X <= Corners[1].Point.X) {
                if (intersects[i].Y >= Corners[0].Point.Y && intersects[i].Y <= Corners[2].Point.Y) {
                    bool duplicate = false;
                    for (int j = 0; j < rectIntersects.Count; j++) {
                        if (rectIntersects[j].X == intersects[i].X && rectIntersects[j].Y == intersects[i].Y) {
                            duplicate = true;
                        } else if (Mathf.Abs(rectIntersects[j].X - intersects[i].X) < 0.05f && Mathf.Abs(rectIntersects[j].Y - intersects[i].Y) < 0.05f) {
                            duplicate = true;
                        }
                    }
                    if (duplicate == false) {
                        rectIntersects.Add(intersects[i]);
                    }
                }
            }
        }

        foreach (Point p in rectIntersects) if (_Debug) Debug.Log(p);

        if (rectIntersects.Count == 2) {
            // build line renderer position values
            Line.Line = new Line(new Point(rectIntersects[0].X, rectIntersects[0].Y), new Point(rectIntersects[1].X, rectIntersects[1].Y));
            Line.UpdatePosition();
        } else if (rectIntersects.Count == 1) {
            Debug.LogError("Only 1 screen intersection, this shouldn't be possible.");
            for (int i = 0; i < intersects.Count; i++) {
                if (_Debug) Debug.Log(i + ": " + intersects[i]);
            }
        } else if (rectIntersects.Count > 2) {
            Debug.LogError("More than 2 screen intersections, this shouldn't be possible.");
        } else {
            Debug.LogWarning("Not enough intersects to draw line");
        }
    }

    public Line LineOfBestFit(List<Point> points) {
        if (_Debug) Debug.Log("LineOfBestFit");
        //average our points
        if (points.Count <= 1) {
            Debug.LogError("Not enough points to draw line");
            return new Line(Corners[0].Point, Corners[3].Point);
        }
        float meanX = points.Average(point => point.X);
        if (_Debug) Debug.Log("meanX: " + meanX);
        float meanY = points.Average(point => point.Y);
        if (_Debug) Debug.Log("meanY: " + meanY);

        //cacluate our slope
        float sumXY = points.Sum(point => point.X * point.Y);
        if (_Debug) Debug.Log("sumXY: " + sumXY);
        float sumXX = points.Sum(point => point.X * point.X);
        if (_Debug) Debug.Log("sumXX: " + sumXX);
        float slope = sumXY/sumXX;
        if (_Debug) Debug.Log("slope: " + slope);
        float yIntercept = meanY - (slope * meanX);
        if (_Debug) Debug.Log("yIntercept: " + yIntercept);
        float xIntercept = (0 - yIntercept) / slope;
        if (_Debug) Debug.Log("xIntercept: " + xIntercept);
        Point a = new Point(xIntercept, 0);
        if (_Debug) Debug.Log(a);
        Point b = new Point(yIntercept, 0);
        if (_Debug) Debug.Log(b);
        if (a.X == b.X && a.Y == b.Y) {
            b = new Point(1, slope * 1 + yIntercept);
        }
        return new Line(a,b);
    }

    public void AddPoint(Vector3 position, Color pointColor, bool screenSpace = false, bool redrawLine = true) {
        if (_Debug) Debug.Log("AddPoint");
        if (_Debug) Debug.Log(position);
        Vector3 _position = new Vector3((position.x/Space.width)*Rect.width/2 + Rect.x, (position.y/Space.height)*Rect.height + Rect.y, position.z);
        if (screenSpace) {
            _position = Camera.ScreenToWorldPoint(position);
        }
        Point p = new Point(_position);
        if (_Debug) Debug.Log(p);
        //if (p.X >= (Rect.x - (Rect.width/2)) && p.X <= (Rect.x + (Rect.width/2)) && p.Y >= (Rect.y - (Rect.height/2)) && p.Y <= (Rect.y + (Rect.height/2))) {
            PointObject point = Instantiate(PointPrefab, PointContainer.transform);
            point.Point = p;
            point.GetComponent<SpriteRenderer>().color = pointColor;
            point.UpdatePosition();
            Points.Add(point);
            if (redrawLine) {
                RedrawLine();
            }
            if (DrawLossLines) {
                RedrawLossLines();
            }
        //} else {
            //Debug.LogError("New Point is out of bounds");
        //}
    }

    public void InBounds(Point point) {

    }

    [ContextMenu("Draw Loss Lines")]
    public void RedrawLossLines() {
        if (_Debug) Debug.Log("RedrawLossLines");
        ClearLossLines();
        for (int i = 0; i < Points.Count; i++) {
            LineObject line = Instantiate(LinePrefab, LineContainer.transform);
            line.Line = CalculateLossLine(Points[i].Point, Line.Line);
            Vector3[] positions = new Vector3[] {new Vector3(line.Line.A.X, line.Line.A.Y, 0), new Vector3(line.Line.B.X, line.Line.B.Y, 0)};
            line.LineRenderer.SetPositions(positions);
        }
    }

    [ContextMenu("Clear Loss Lines")]
    public void ClearLossLines() {
        if (_Debug) Debug.Log("ClearLossLines");
        for (int i = LineContainer.transform.childCount - 1; i >= 0; i--)
        {
            if (Application.isEditor)
                GameObject.DestroyImmediate(LineContainer.transform.GetChild(i).gameObject);
            else
                GameObject.Destroy(LineContainer.transform.GetChild(i).gameObject);
        }
    }

    public float CalculateLoss(Point point, Line line, LossMode mode = LossMode.Default) {
        if (_Debug) Debug.Log("CalculateLoss");
        if (_Debug) Debug.Log(point);
        Line lossLine = CalculateLossLine(point, line);
        float loss = lossLine.A.Y - lossLine.B.Y;
        loss *= loss;
        switch (mode) {
            case LossMode.Default:
                
                break;
            case LossMode.Zero:
                loss *= ((loss < 0) ? -1 : 1);
                break;
        }
        return loss;
    }

    [ContextMenu("Calculate Total Loss")]
    public float CalculateTotalLoss(LossMode mode = LossMode.Default) {
        float totalLoss = 0f;
        for (int i = 0; i < Points.Count; i++) {
            totalLoss += CalculateLoss(Points[i].Point, Line.Line, mode);
        }
        return totalLoss;
    }

    public Line CalculateLossLine(Point point, Line lineA) {
        if (_Debug) Debug.Log("CalculateLossLine");
        if (_Debug) Debug.Log(lineA);
        Point xIntercept = new Point(point.X, 0);
        Line lineB = new Line(point, xIntercept);
        if (_Debug) Debug.Log(lineB);
        Point intersect = LineIntersection.FindIntersection(lineA, lineB);
        // check if our intersection is out of bounds, draw screen intersection if so
        if (intersect.Y < Corners[0].Point.Y || intersect.Y > Corners[2].Point.Y) {
            if (point.Y >= intersect.Y) {
                intersect = LineIntersection.FindIntersection(lineB, Sides[0].Line);
            } else {
                intersect = LineIntersection.FindIntersection(lineB, Sides[1].Line);
            }
        }
        Line lossLine = new Line(point, intersect);
        return lossLine;
    }
}

public enum GraphMode {
    Default,
    PositiveY
}

public enum LossMode {
    Default,
    Zero
}

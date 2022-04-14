using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{

    public LineRenderer LineRenderer;
    public Camera Camera;
    public  List<NodeObject> Nodes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Redraw Line")]
    public void RedrawLine() {
        RedrawLine(Nodes);
    }

    public void RedrawLine(List<NodeObject> _points) {
        // get our line of best fit
        List<Point> points = new List<Point>();
        foreach (NodeObject _p in _points) {
            points.Add(new Point(_p.transform));
        }
        Line bestFit = LineOfBestFit(points);
        Debug.Log(bestFit.A.X);
        Debug.Log(bestFit.A.Y);
        Debug.Log(bestFit.B.X);
        Debug.Log(bestFit.B.Y);

        Debug.Log("width: " + Screen.width);
        Debug.Log("height: " + Screen.height);

        // find our corners
        Vector3 bottomLeft = Camera.ScreenToWorldPoint(new Vector3(0,0,0));
        Debug.Log(bottomLeft);
        Vector3 bottomRight = Camera.ScreenToWorldPoint(new Vector3(Screen.width,0,0));
        Debug.Log(bottomRight);
        Vector3 upperLeft = Camera.ScreenToWorldPoint(new Vector3(0,Screen.height,0));
        Debug.Log(upperLeft);
        Vector3 upperRight = Camera.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,0));
        Debug.Log(upperRight);

        // create line and point objects from corners
        Line bottom = new Line(new Point(bottomLeft.x, bottomLeft.y), new Point(bottomRight.x, bottomRight.y));
        Debug.Log("bottom: " + bottom.ToString());
        Line top = new Line(new Point(upperLeft.x, upperLeft.y), new Point(upperRight.x, upperRight.y));
        Debug.Log("top: " + top.ToString());
        Line left = new Line(new Point(bottomLeft.x, bottomLeft.y), new Point(upperLeft.x, upperLeft.y));
        Debug.Log("left: " + left.ToString());
        Line right = new Line(new Point(bottomRight.x, bottomRight.y), new Point(upperRight.x, upperRight.y));
        Debug.Log("right: " + right.ToString());

        // find our points of intersection
        List<Point> intersects = new List<Point>();
        try {
            intersects.Add(LineIntersection.FindIntersection(bestFit, bottom));
        } catch (System.SystemException e) {
            Debug.LogError("bottom line error");
            Debug.LogError(e);
        }
        try {
            intersects.Add(LineIntersection.FindIntersection(bestFit, top));
        } catch (System.SystemException e) {
            Debug.LogError("top line error");
            Debug.LogError(e);
        }
        try {
            intersects.Add(LineIntersection.FindIntersection(bestFit, left));
        } catch (System.SystemException e) {
            Debug.LogError("left line error");
            Debug.LogError(e);
        }
        try {
            intersects.Add(LineIntersection.FindIntersection(bestFit, right));
        } catch (System.SystemException e) {
            Debug.LogError("right line error");
            Debug.LogError(e);
        }

        // find the points which are within the bounds of teh screen edge
        List<Point> screenIntersects = new List<Point>();
        for (int i = 0; i < intersects.Count; i++) {
            if (intersects[i].X >= bottomLeft.x && intersects[i].X <= bottomRight.x) {
                if (intersects[i].Y >= bottomLeft.y && intersects[i].Y <= upperLeft.y) {
                    Debug.Log(intersects[i].X + ", " + intersects[i].Y);
                    screenIntersects.Add(intersects[i]);
                }
            }
        }

        // build line renderer position values
        Vector3[] positions =  new Vector3[2];
        positions[0] = new Vector3(screenIntersects[0].X, screenIntersects[0].Y, 0);
        positions[1] = new Vector3(screenIntersects[1].X, screenIntersects[1].Y, 0);
        LineRenderer.SetPositions(positions);
    }

    Line LineOfBestFit(List<Point> points) {
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
}

public struct Line {
    public Point A { get; set; }
    public Point B { get; set; }

    public Line(Point a, Point b) {
        A = a;
        B = b;
    }

    public override string ToString()
    {
        return A.ToString() + "----->" + B.ToString();
    }
}

public struct Point {
    public float X { get; set; }
    public float Y { get; set; }

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

/*
Adapted from https://stackoverflow.com/questions/4543506/algorithm-for-intersection-of-2-lines
*/
public class LineIntersection
{
    //  Returns Point of intersection if do intersect otherwise default Point (null)
    public static Point FindIntersection(Line lineA, Line lineB, float tolerance = 0.001f)
    {
        float x1 = lineA.A.X, y1 = lineA.A.Y;
        float x2 = lineA.B.X, y2 = lineA.B.Y;

        float x3 = lineB.A.X, y3 = lineB.A.Y;
        float x4 = lineB.B.X, y4 = lineB.B.Y;

        // equations of the form x = c (two vertical lines)
        if (Mathf.Abs(x1 - x2) < tolerance && Mathf.Abs(x3 - x4) < tolerance && Mathf.Abs(x1 - x3) < tolerance)
        {
            throw new System.Exception("Both lines overlap vertically, ambiguous intersection points.");
        }

        //equations of the form y=c (two horizontal lines)
        if (Mathf.Abs(y1 - y2) < tolerance && Mathf.Abs(y3 - y4) < tolerance && Mathf.Abs(y1 - y3) < tolerance)
        {
            throw new System.Exception("Both lines overlap horizontally, ambiguous intersection points.");
        }

        //equations of the form x=c (two vertical parallel lines)
        if (Mathf.Abs(x1 - x2) < tolerance && Mathf.Abs(x3 - x4) < tolerance)
        {   
            //return default (no intersection)
            Debug.Log("A");
            return default(Point);
        }

        //equations of the form y=c (two horizontal parallel lines)
        if (Mathf.Abs(y1 - y2) < tolerance && Mathf.Abs(y3 - y4) < tolerance)
        {
            //return default (no intersection)
            Debug.Log("B");
            return default(Point);
        }

        //general equation of line is y = mx + c where m is the slope
        //assume equation of line 1 as y1 = m1x1 + c1 
        //=> -m1x1 + y1 = c1 ----(1)
        //assume equation of line 2 as y2 = m2x2 + c2
        //=> -m2x2 + y2 = c2 -----(2)
        //if line 1 and 2 intersect then x1=x2=x & y1=y2=y where (x,y) is the intersection point
        //so we will get below two equations 
        //-m1x + y = c1 --------(3)
        //-m2x + y = c2 --------(4)

        float x, y;

        //lineA is vertical x1 = x2
        //slope will be infinity
        //so lets derive another solution
        if (Mathf.Abs(x1 - x2) < tolerance)
        {
            //compute slope of line 2 (m2) and c2
            float m2 = (y4 - y3) / (x4 - x3);
            float c2 = -m2 * x3 + y3;

            //equation of vertical line is x = c
            //if line 1 and 2 intersect then x1=c1=x
            //subsitute x=x1 in (4) => -m2x1 + y = c2
            // => y = c2 + m2x1 
            x = x1;
            y = c2 + m2 * x1;
        }
        //lineB is vertical x3 = x4
        //slope will be infinity
        //so lets derive another solution
        else if (Mathf.Abs(x3 - x4) < tolerance)
        {
            //compute slope of line 1 (m1) and c2
            float m1 = (y2 - y1) / (x2 - x1);
            float c1 = -m1 * x1 + y1;

            //equation of vertical line is x = c
            //if line 1 and 2 intersect then x3=c3=x
            //subsitute x=x3 in (3) => -m1x3 + y = c1
            // => y = c1 + m1x3 
            x = x3;
            y = c1 + m1 * x3;
        }
        //lineA & lineB are not vertical 
        //(could be horizontal we can handle it with slope = 0)
        else
        {
            //compute slope of line 1 (m1) and c2
            float m1 = (y2 - y1) / (x2 - x1);
            float c1 = -m1 * x1 + y1;

            //compute slope of line 2 (m2) and c2
            float m2 = (y4 - y3) / (x4 - x3);
            float c2 = -m2 * x3 + y3;

            //solving equations (3) & (4) => x = (c1-c2)/(m2-m1)
            //plugging x value in equation (4) => y = c2 + m2 * x
            x = (c1 - c2) / (m2 - m1);
            y = c2 + m2 * x;

            //verify by plugging intersection point (x, y)
            //in orginal equations (1) & (2) to see if they intersect
            //otherwise x,y values will not be finite and will fail this check
            if (!(Mathf.Abs(-m1 * x + y - c1) < tolerance
                && Mathf.Abs(-m2 * x + y - c2) < tolerance))
            {
                //return default (no intersection)
                Debug.Log("C");
                return default(Point);
            }
        }

        /*//x,y can intersect outside the line segment since line is infinitely long
        //so finally check if x, y is within both the line segments
        if (IsInsideLine(lineA, x, y) &&
            IsInsideLine(lineB, x, y))
        {
            Debug.Log("D");
            
        }*/

        return new Point { X = x, Y = y };

        //return default (no intersection)
        //Debug.Log("E");
        //return default(Point);

    }

    // Returns true if given point(x,y) is inside the given line segment
    private static bool IsInsideLine(Line line, float x, float y)
    {
        return (x >= line.A.X && x <= line.B.X
                    || x >= line.B.X && x <= line.A.X)
               && (y >= line.A.Y && y <= line.B.Y
                    || y >= line.B.Y && y <= line.A.Y);
    }
}

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

[System.Serializable]
public struct Line {
    public Point A;
    public Point B;
    
    public float Slope {
        get {
            return (B.Y-A.Y)/(B.X-A.X);
        }
    }

    public Line(Point a, Point b) {
        A = a;
        B = b;
    }

    public Line(float slope, float yIntercept) {
        float xIntercept = yIntercept/slope;
        A = new Point(0, yIntercept);
        B = new Point(xIntercept, 0);
    }

    public override string ToString()
    {
        return A.ToString() + "----->" + B.ToString();
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
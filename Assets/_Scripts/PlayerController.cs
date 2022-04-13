using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private List<Vector2> clickedPos;
    [SerializeField] private double w;
    [SerializeField] private double b;
    [SerializeField] private float learningRate;
    [SerializeField] private float normalize;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private LayerMask backMask;

    private Rigidbody2D rb;
    private Vector2 targetPos;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        clickedPos = new List<Vector2>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray,out RaycastHit hit, Mathf.Infinity, backMask);
            //Debug.Log(hit.point);
            
            targetPos = hit.point - transform.position;

            clickedPos.Add(hit.point);
            float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            if (clickedPos.Count < 1)
            {
                w = b = 0;
            }
            else
            {
                GradientDescent();
            }
            SetLine((float)w,(float)b);
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            rb.AddForce(transform.right * speed);
        }
    }
    
    private void SetLine(float w, float b)
    {
        //float rad = Mathf.Atan(w);
        //float deg = rad * 180 / Mathf.PI;
        //transform.localPosition = new Vector3(0, (float) b, 0);
        //transform.localRotation = Quaternion.Euler(0, 0, (float) deg);

        line.SetPosition(0, new Vector3(-2.5f, (-0.5f* (float) w + (float) 0), 0f));
        line.SetPosition(1, new Vector3(2.5f, (0.5f* (float) w + (float) 0), 0f));


    }
    
    private void GradientDescent()
    {
        w = 0;
        b = 0;
        Vector2[] pointList = new Vector2[0];
        pointList = clickedPos.ToArray();
        var positions = pointList.Select(p => p);
        var X = positions.Select(pos => pos.x);
        // foreach (var x in X)
        // {
        //     Debug.Log(x);
        // }
        var Y = positions.Select(pos => pos.y);
        int n = pointList.Length;
        var Y_pred = X.Select(x => x * w + b);

        double dW = -2.0 / n * Y
            .Zip(Y_pred, (y, yPred) => y - yPred)
            .Zip(X, (y_p, x) => y_p * x)
            .Sum();
        double dB = -2.0 / n * Y
            .Zip(Y_pred, (y, yPred) => y - yPred)
            .Sum();

        // Debug.Log(dW + " : " + dB);

        w -= learningRate * dW * normalize;
        b -= learningRate * dB * normalize;
    }
}

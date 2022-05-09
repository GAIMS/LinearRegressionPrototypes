using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoController : MonoBehaviour
{
    [SerializeField] private GameObject tornadoPrefab;
    [SerializeField] private LineRenderer line;
    [SerializeField] private LayerMask backMask;
    [SerializeField] private float length;

    private bool click1 = true;
    private Vector3 clickedPos1, clickedPos2;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray,out RaycastHit hit, Mathf.Infinity, backMask);
            
            
            if(!click1)
            {
                clickedPos2 = hit.point;
                line.SetPosition(1,clickedPos2);
                click1 = true;
                Vector2 tornadoStart = CalculateNewPoint(clickedPos1, clickedPos2, clickedPos1);
                //Vector2 tornadoEnd = CalculateNewPoint(clickedPos1, clickedPos2, clickedPos2);
                Vector2 targetPos = clickedPos2 - clickedPos1;

                float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;

                GameObject tornado = Instantiate(tornadoPrefab, clickedPos1, Quaternion.Euler(90,0,0));
                tornado.transform.rotation = Quaternion.AngleAxis(angle, tornado.transform.up);
                return;
            }
            if (click1)
            {
                clickedPos1 = hit.point;
                line.SetPosition(0,clickedPos1);
                click1 = false;
                return;
            }
        }

        if (!click1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray,out RaycastHit hit, Mathf.Infinity, backMask);
            line.SetPosition(1,hit.point);
        }
        
    }

    public Vector2 CalculateNewPoint(Vector2 pos1, Vector2 pos2, Vector2 nextPoint)
    {
        float m = (pos2.y - pos1.y) / (pos2.x - pos1.x);

        float b = (pos1.y) - (m * pos1.x);
        
        Vector2 newPoint = new Vector2();
        newPoint.y = (m * nextPoint.x) + b;
        newPoint.x = (nextPoint.y / m) - b;
        
        return newPoint;
    }
}

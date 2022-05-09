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

                GameObject tornado = Instantiate(tornadoPrefab, clickedPos1, Quaternion.Euler(90,0,0));
                tornado.GetComponent<TornadoScript>().endPos = clickedPos2;
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
}

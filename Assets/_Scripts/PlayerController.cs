using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask backMask;

    private Rigidbody2D rb;
    private Vector2 targetPos;
    private GameManager gm;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gm = FindObjectOfType<GameManager>();

    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray,out RaycastHit hit, Mathf.Infinity, backMask);
            //Debug.Log(hit.point);
            
            targetPos = hit.point - transform.position;

            float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            rb.AddForce(transform.right * speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        gm.score = 0;
        
        gm.Invoke("NewGraph", .1f);
    }
}

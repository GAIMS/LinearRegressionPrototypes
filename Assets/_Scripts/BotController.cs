using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BotController : MonoBehaviour
{
    [Range(.5f,1)]
    [SerializeField] public float runSpeed;
    [Range(.5f,1)]
    [SerializeField] public float climbSpeed;
    [Range(.5f,1)]
    [SerializeField] public float flySpeed;
    [Range(.5f,1)]
    [SerializeField] public float swimSpeed;
    [Range(.75f,1)]
    [SerializeField] public float stamina;

    [SerializeField] float speedMultiplier = 10;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundPos;
    [SerializeField] private float radius;
    
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool isClimbing = false;
    [SerializeField] private bool isflying = false;
    [SerializeField] private bool isSwimming = false;

    public int placement;
    public float raceTime;

    [SerializeField] private bool usingForce;
    
    private Rigidbody2D rb;
    
    void Start()
    {
        /*runSpeed = Random.Range(.5f, 1f);
        climbSpeed = Random.Range(.5f, 1f);
        flySpeed = Random.Range(.5f, 1f);
        swimSpeed = Random.Range(.5f, 1f);
        stamina = Random.Range(.75f, 1f);

        rb = GetComponent<Rigidbody2D>();*/
    }

    public void Init()
    {
        runSpeed = Random.Range(.5f, 1f);
        climbSpeed = Random.Range(.5f, 1f);
        flySpeed = Random.Range(.5f, 1f);
        swimSpeed = Random.Range(.5f, 1f);
        stamina = Random.Range(.75f, 1f);

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(groundPos.position, radius,Vector2.zero ,0f,groundMask);
        isSwimming = false;
        isClimbing = false;
        if(hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                switch (hit[i].collider.gameObject.layer)
                {
                    case 3:
                        isClimbing = true;
                        isGrounded = false;
                        break;
                    case 4:
                        isSwimming = true;
                        isGrounded = false;
                        break;
                    case 6 when hit[i].collider.gameObject.layer != 4 && hit[i].collider.gameObject.layer != 3 
                                                                   && hit[i].collider.gameObject.layer == 6:
                        isGrounded = true;
                        //isSwimming = false;
                        //isClimbing = false;
                        break;
                }
            }
            isflying = false;
        }
        else
        {
            isClimbing = false;
            isGrounded = false;
            isSwimming = false;
            isflying = true;
        }

        //isClimbing = Physics2D.Raycast(transform.position, Vector2.right, radius,3);
    }

    void FixedUpdate()
    {
        //Debug.Log((bool)Physics2D.CircleCast(groundPos.position, radius, Vector2.zero, 0, groundMask));

        if (isGrounded)
        {
            if(!usingForce)
                rb.velocity = new Vector2( runSpeed * speedMultiplier * Time.deltaTime, rb.velocity.y);
            if(usingForce)
                rb.AddForce(new Vector2( runSpeed * speedMultiplier * Time.deltaTime, 0));
        }
        if (isSwimming)
        {
            if(!usingForce)
                rb.velocity = new Vector2( swimSpeed * speedMultiplier * Time.deltaTime, rb.velocity.y);
            if(usingForce)
                rb.AddForce(new Vector2( swimSpeed * speedMultiplier * Time.deltaTime, 0));
        }
        if (isflying)
        {
            if(!usingForce)
                rb.velocity = new Vector2( flySpeed * speedMultiplier * Time.deltaTime, rb.velocity.y);
            if(usingForce)
                rb.AddForce(new Vector2( flySpeed * speedMultiplier * Time.deltaTime, 0));
        }
        if (isClimbing)
        {
            if(!usingForce)
                rb.velocity = new Vector2( rb.velocity.x, climbSpeed * speedMultiplier * Time.deltaTime);
            if(usingForce)
                rb.AddForce(new Vector2( 0, climbSpeed * speedMultiplier * Time.deltaTime) - Physics2D.gravity);
        }
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     isGrounded = true;
    //     isflying = false;
    // }
    //
    // private void OnCollisionExit2D(Collision2D other)
    // {
    //     isGrounded = false;
    //     isflying = true;
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 3)
        {
            isClimbing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 4)
        {
            isSwimming = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundPos.position, radius);
    }
}

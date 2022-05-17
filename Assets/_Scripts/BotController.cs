using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BotController : MonoBehaviour
{
    [Range(.25f,1)]
    [SerializeField] private float runSpeed;
    [Range(.25f,1)]
    [SerializeField] private float climbSpeed;
    [Range(.5f,1)]
    [SerializeField] private float flySpeed;
    [Range(.25f,1)]
    [SerializeField] private float swimSpeed;
    [Range(.75f,1)]
    [SerializeField] private float stamina;

    [SerializeField] float speedMultiplier = 10;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundPos;
    [SerializeField] private float radius;
    
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool isClimbing = false;
    [SerializeField] private bool isflying = false;
    [SerializeField] private bool isSwimming = false;
    
    private Rigidbody2D rb;
    
    void Start()
    {
        runSpeed = Random.Range(.25f, 1f);
        climbSpeed = Random.Range(.25f, 1f);
        flySpeed = Random.Range(.5f, 1f);
        swimSpeed = Random.Range(.25f, 1f);
        stamina = Random.Range(.75f, 1f);

        rb = GetComponent<Rigidbody2D>();
    }

    
    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.CircleCast(groundPos.position, radius,Vector2.zero ,0f,groundMask);

        //Debug.Log((bool)Physics2D.CircleCast(groundPos.position, radius, Vector2.zero, 0, groundMask));
        if(Physics2D.CircleCast(groundPos.position, radius, Vector2.zero, 0, groundMask))
        {
            isflying = false;
            if (hit.collider.gameObject.layer == 4)
            {
                isSwimming = true;
                isGrounded = false;
            }
            else if (hit.collider.gameObject.layer == 3)
            {
                isClimbing = true;
            }
            else if (hit.collider.gameObject.layer == 6)
            {
                isGrounded = true;
            }
            if(hit.collider.gameObject.layer != 4)
            {
                isSwimming = false;
            }
            if(hit.collider.gameObject.layer != 3)
            {
                isClimbing = false;
            }
        }
        else
        {
            isClimbing = false;
            isGrounded = false;
            isSwimming = false;
            isflying = true;
        }

        if (isGrounded)
        {
            rb.velocity = new Vector2( runSpeed * speedMultiplier * Time.deltaTime, rb.velocity.y);
        }
        if (isSwimming)
        {
            rb.velocity = new Vector2( swimSpeed * speedMultiplier * Time.deltaTime, rb.velocity.y);
        }
        if (isflying)
        {
            rb.velocity = new Vector2( flySpeed * speedMultiplier * Time.deltaTime, rb.velocity.y);
        }
        if (isClimbing)
        {
            rb.velocity = new Vector2( rb.velocity.x, climbSpeed * speedMultiplier * Time.deltaTime);
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

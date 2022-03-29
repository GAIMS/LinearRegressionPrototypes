using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private float maxSpeed = 10;
    [SerializeField] private float jumpHeight = 10;
    [SerializeField] private float gravity = -10;
    [SerializeField] private bool isGrounded;
    
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private Texture2D noise;

    private CharacterController controller;
    private Vector3 velocity;
    private float x, z;
    private bool jump;
    
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        noise = FindObjectOfType<NoiseGenerator>().noiseTex;
    }

    void Update()
    {
        GetInput();
        Move();

    }

    void GetInput()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        jump = Input.GetButtonDown("Jump");
    }

    private void FixedUpdate()
    {
    }

    void Move()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (jump && isGrounded)
        {
            velocity.y = Mathf.Sqrt(noise.GetPixel((int)transform.position.x,(int)transform.position.y).r * jumpHeight * -2f * gravity);
        }
        
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}

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

    [SerializeField] private GameObject highestFlag;
    [SerializeField] private GameObject lowestFlag;

    private Vector2 lowestPoint, highestPoint;
    
    private CharacterController controller;
    private Vector3 velocity;
    private float x, z;
    private bool jump;
    private NoiseGenerator noiseGen;
    
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        noiseGen = FindObjectOfType<NoiseGenerator>();
        noise = noiseGen.noiseTex;
    }

    void Update()
    {
        GetInput();
        Move();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Instantiate(highestFlag, new Vector3(transform.position.x, 1, transform.position.z), Quaternion.identity);
            highestPoint = new Vector2(transform.position.x, transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(lowestFlag, new Vector3(transform.position.x, 1, transform.position.z), Quaternion.identity);
            lowestPoint = new Vector2(transform.position.x, transform.position.z);
        }
            
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

        //Debug.Log(noise.GetPixel((int)transform.position.x * 1,(int)transform.position.z * 1).r);
        
        //noise.SetPixel(-(int)transform.position.x + 50,-(int)transform.position.z + 50,Color.red);
        //noise.Apply();
        float pxlVal = noise.GetPixel(-(int) transform.position.x + (noiseGen.pxlWidth/2),
            -(int) transform.position.z + (noiseGen.pxlHeight/2)).r;
        Debug.Log(pxlVal);
        if (jump && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * pxlVal * -2f * gravity);
        }
        
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}

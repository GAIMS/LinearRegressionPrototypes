using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public CharacterController Controller;
    public Vector3 Velocity;
    public GameObject Grid;
    public float Speed;
    public float Jump;
    public float Gravity = -9.81f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool groundedPlayer = Controller.isGrounded;
        if (groundedPlayer && Velocity.y < 0)
        {
            Velocity.y = 0f;
        }

        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Controller.Move(movement * Time.deltaTime * Speed);

        if (movement != Vector3.zero)
        {
            gameObject.transform.forward = movement;
        }

        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            Velocity.y += Mathf.Sqrt(Jump * Gravity);
        }

        Velocity.y += Gravity * Time.deltaTime;
        Controller.Move(Velocity * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public CharacterController Controller;
    public Vector3 Velocity;
    public GameObject Grid;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            // forward
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            // left
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            // right
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            // back
        }
        if (Input.GetKey(KeyCode.Space)) {
            // jump
        }
        if (Input.GetKey(KeyCode.R)) {
            Grid.SetActive(!Grid.activeInHierarchy);
        }
        if (Input.GetKey(KeyCode.F)) {
            // check for a node
        }
    }
}

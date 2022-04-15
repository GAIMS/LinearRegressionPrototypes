using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float sensitivity;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform body;
    [SerializeField] private LayerMask ground;
    [SerializeField] private Transform cursor;

    private float xRotation, yRotation;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        float x = Input.GetAxis("Vertical");
        float y = Input.GetAxis("Horizontal");
        float z = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            z = -1;
        }
        if (Input.GetKey(KeyCode.E))
        {
            z = 1;
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ground, QueryTriggerInteraction.Ignore);

        cursor.position = hit.point;
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(ballPrefab, cursor.position + (Vector3.up * 5), Quaternion.identity);
        }
        
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        body.Rotate(Vector3.up * mouseX);
        
        body.Translate(new Vector3(y,z,x) * speed * Time.deltaTime,transform);
    }
}

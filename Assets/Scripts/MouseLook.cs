using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float Sensitivity = 100f;
    public float Clamp = 80f;

    private float X;
    private float Y;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 rotation = transform.localRotation.eulerAngles;
        Y = rotation.y;
        X = rotation.x;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Y += mouseX * Sensitivity * Time.deltaTime;
        X += mouseY * Sensitivity * Time.deltaTime * -1;
        X = Mathf.Clamp(X, -Clamp, Clamp);
        Quaternion localRotation = Quaternion.Euler(X, Y, 0f);
        transform.rotation = localRotation;
    }
}

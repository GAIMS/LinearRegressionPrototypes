using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private ParticleSystem pc;
    public Vector2 endPos;
    
    void Start()
    {
        
    }


    void Update()
    {
        pc.transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("House"))
        {
            other.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}

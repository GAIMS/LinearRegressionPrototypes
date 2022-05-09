using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoScript : MonoBehaviour
{
    [SerializeField] private float speed;
    public Vector2 endPos;
    void Start()
    {
        
    }


    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("House"))
        {
            other.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    private Transform player;
    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
    }

    
    void Update()
    {
        transform.LookAt(new Vector3(player.position.x, player.position.y, player.position.z));
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField] private GameObject winUI;
    [SerializeField] private bool noisePlacement;

    private void Start()
    {
        if (noisePlacement)
        {
            transform.position = FindObjectOfType<NoiseGenerator>().lowestCoord;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (col.GetComponent<BotController>())
            {
                col.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                col.GetComponent<Rigidbody2D>().isKinematic = false;
                col.GetComponent<BotController>().enabled = false;
                col.enabled = false;
                col.GetComponentInChildren<TrailRenderer>().enabled = false;
            }
            else
            {
                winUI.SetActive(true);

            }
        }
    }
}

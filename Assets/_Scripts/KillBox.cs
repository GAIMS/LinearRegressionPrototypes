using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    public int dead = 0;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bot"))
        {
            col.GetComponent<BotController>().placement = 10;
            dead++;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    public int dead = 0;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bot"))
        {
            col.GetComponent<BotController>().placement = 10;

            CinemachineTargetGroup.Target[] targets = FindObjectOfType<CinemachineTargetGroup>().m_Targets;

            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].target == col.transform)
                {
                    targets[i].weight = 0;
                }
            }

            dead++;
        }
    }
}

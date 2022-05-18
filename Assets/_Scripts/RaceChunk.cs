using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChunkType
{
    Ground,
    Swim,
    Climb,
    Fly,
    Finish
}

public class RaceChunk : MonoBehaviour
{
    public ChunkType chunkType;

    public Transform lftPoint, rtPoint;

    public int finishedRacers = 0;
    private GameManager gm;

    private bool win = false;
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (chunkType == ChunkType.Finish && col.CompareTag("Bot"))
        {
            col.attachedRigidbody.simulated = false;
            if (!win)
            {
                gm.winner = col.GetComponent<BotController>();
                win = true;
            }
            finishedRacers++;
        }
    }
}

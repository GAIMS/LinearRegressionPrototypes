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

    public bool zeroGravity = false;
    
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (chunkType == ChunkType.Finish && col.CompareTag("Bot"))
        {
            col.attachedRigidbody.simulated = false;
            col.GetComponent<BotController>().placement = finishedRacers + 1;
            col.GetComponent<BotController>().raceTime = FindObjectOfType<RaceTrackGenerator>().raceTimer;
            finishedRacers++;
        }

        if (col.CompareTag("Bot"))
        {
            col.attachedRigidbody.gravityScale = -0.25f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bot"))
        {
            other.attachedRigidbody.gravityScale = 0.5f;
        }
    }
}

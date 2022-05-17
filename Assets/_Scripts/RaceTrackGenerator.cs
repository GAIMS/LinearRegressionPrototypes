using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTrackGenerator : MonoBehaviour
{
    public GameObject[] raceChunks;

    [SerializeField] private int raceSegments;
    
    [Range(.25f,1)]
    [SerializeField] private float runWeight;
    [Range(.25f,1)]
    [SerializeField] private float climbWeight;
    [Range(.25f,1)]
    [SerializeField] private float flyWeight;
    [Range(.25f,1)]
    [SerializeField] private float swimWeight;

    private RaceChunk lastChunk;
    private bool firstChunk = true;
    void Awake()
    {
        for (int i = 0; i < raceSegments; i++)
        {
            GenerateTrack();
            firstChunk = false;
        }
    }

    
    void Update()
    {
        
    }

    public void GenerateTrack()
    {
        float runVal = Random.Range(.25f, runWeight);
        float climbVal = Random.Range(.25f, climbWeight);
        float flyVal = Random.Range(.25f, flyWeight);
        float swimVal = Random.Range(.25f, swimWeight);

        if (firstChunk) flyVal = climbVal = 0 ;
        
        float max = Mathf.Max(runVal, climbVal, flyVal, swimVal);

        if (runVal == max)
        {
            PlaceSegment(ChunkType.Ground);
        }
        if (climbVal == max)
        {
            PlaceSegment(ChunkType.Climb);
        }
        if (flyVal == max)
        {
            PlaceSegment(ChunkType.Fly);
        }
        if (swimVal == max)
        {
            PlaceSegment(ChunkType.Swim);
        }
    }

    public void PlaceSegment(ChunkType type)
    {
        List<GameObject> useableChunks = new List<GameObject>();
        foreach (var chunk in raceChunks)
        {
            if (chunk.GetComponent<RaceChunk>().chunkType == type)
            {
                useableChunks.Add(chunk);
            }
        }

        GameObject newChunk = gameObject;
        int rand = Random.Range(0, useableChunks.Count);
        if (firstChunk)
        {
            newChunk = Instantiate(useableChunks[rand], Vector3.zero, Quaternion.identity,transform);
        }
        else
        {
            newChunk = Instantiate(useableChunks[rand], lastChunk.rtPoint.position,Quaternion.identity,transform);
            //newChunk.transform.position += lastChunk.rtPoint.localPosition;
            newChunk.transform.position -= newChunk.GetComponent<RaceChunk>().lftPoint.localPosition;
        }
        lastChunk = newChunk.GetComponent<RaceChunk>();
    }
}

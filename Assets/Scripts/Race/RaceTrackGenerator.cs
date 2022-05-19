using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTrackGenerator : MonoBehaviour
{
    public GameObject[] raceChunks;

    [SerializeField] private GameObject start, finish;
    
    [SerializeField] private int raceSegments;
    
    [Range(.25f,1)]
    [SerializeField] private float runWeight;
    [Range(.25f,1)]
    [SerializeField] private float climbWeight;
    [Range(.25f,1)]
    [SerializeField] private float flyWeight;
    [Range(.25f,1)]
    [SerializeField] private float swimWeight;

//    private KillBox killbox;
    private GameManager gm;
    
    private RaceChunk lastChunk;
    private RaceChunk finishChunk;
    private bool firstChunk = true;
    void Awake()
    {
    //    killbox = FindObjectOfType<KillBox>();
        gm = FindObjectOfType<GameManager>();
        for (int i = 0; i < raceSegments; i++)
        {
			if (i == 0) {
				PlaceStart();
				firstChunk = false;
			} else {
			
				GenerateTrack();
				if (i == raceSegments - 1)
				{
					PlaceFinish();
				}
			}
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
	
	public void PlaceStart() {
		GameObject newChunk = Instantiate(start, Vector3.zero, Quaternion.identity);
		lastChunk = newChunk.GetComponent<RaceChunk>();
	}

    public void PlaceFinish()
    {
        GameObject newChunk = Instantiate(finish, lastChunk.rtPoint.position,Quaternion.identity,transform);
        newChunk.transform.position -= newChunk.GetComponent<RaceChunk>().lftPoint.localPosition;
        finishChunk = newChunk.GetComponent<RaceChunk>();
    }

    public void Restart()
    {
    //    gm.UpdateLists(raceSegments);
        
        firstChunk = true;
    //    killbox.dead = 0;
        RaceChunk[] oldChunks = FindObjectsOfType<RaceChunk>();
        foreach (var chunk in oldChunks)
        {
            Destroy(chunk.gameObject);
        }
        for (int i = 0; i < raceSegments; i++)
        {
            GenerateTrack();
            firstChunk = false;
            if (i == raceSegments - 1)
            {
                PlaceFinish();
            }
        }
    }
}
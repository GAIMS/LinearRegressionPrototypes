using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTrackGenerator : MonoBehaviour
{
    public GameObject[] raceChunks;

    [SerializeField] private GameObject finish;
    [SerializeField] private GameObject racerPrefab;
    [SerializeField] private CameraController camController;
    
    [SerializeField] private int raceSegments;
    
    [Range(.25f,1)]
    [SerializeField] public float runWeight;
    [Range(.25f,1)]
    [SerializeField] public float climbWeight;
    [Range(.25f,1)]
    [SerializeField] public float flyWeight;
    [Range(.25f,1)]
    [SerializeField] public float swimWeight;

    private KillBox killbox;
    private GameManager gm;
    
    private RaceChunk lastChunk;
    private RaceChunk finishChunk;
    private bool firstChunk = true;

    public int racers;
    [HideInInspector]
    public bool gameOver = true;
    void Awake()
    {
        killbox = FindObjectOfType<KillBox>();
        gm = FindObjectOfType<GameManager>();
        for (int i = 0; i < racers; i++)
        {
            GameObject newRacer = Instantiate(racerPrefab, Vector3.up, Quaternion.identity, null);
            newRacer.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
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

    
    void Update()
    {
        if (finishChunk.finishedRacers + killbox.dead == racers && gameOver)
        {
            gm.UpdateLists(raceSegments);
            gm.EndRace();
            gameOver = false;
            //Restart();
        }
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

    public void PlaceFinish()
    {
        GameObject newChunk = Instantiate(finish, lastChunk.rtPoint.position,Quaternion.identity,transform);
        newChunk.transform.position -= newChunk.GetComponent<RaceChunk>().lftPoint.localPosition;
        finishChunk = newChunk.GetComponent<RaceChunk>();
    }

    public void Restart()
    {
        gm.UpdateLists(raceSegments);
        
        firstChunk = true;
        killbox.dead = 0;

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

        BotController[] bots = FindObjectsOfType<BotController>();
        foreach (var bot in bots)
        {
            Destroy(bot.gameObject);
        }
        
        for (int i = 0; i < racers; i++)
        {
            GameObject newRacer = Instantiate(racerPrefab, Vector3.up, Quaternion.identity, null);
            newRacer.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        }
        camController.FindBots();
        gm.GetPrediction();
        gameOver = true;
    }

    public void RestartButton()
    {
        firstChunk = true;
        killbox.dead = 0;

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

        BotController[] bots = FindObjectsOfType<BotController>();
        foreach (var bot in bots)
        {
            Destroy(bot.gameObject);
        }
        
        for (int i = 0; i < racers; i++)
        {
            GameObject newRacer = Instantiate(racerPrefab, Vector3.up, Quaternion.identity, null);
            newRacer.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        }
        camController.FindBots();
    }

    public void RunWeight(float weight)
    {
        runWeight = weight;
    }
    public void FlyWeight(float weight)
    {
        flyWeight = weight;
    }
    public void SwimWeight(float weight)
    {
        swimWeight = weight;
    }
    public void ClimbWeight(float weight)
    {
        climbWeight = weight;
    }
}

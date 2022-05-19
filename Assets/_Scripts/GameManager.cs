using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Race> races;
    public List<Agent> agents;
    private int raceNum = 0;
    public BotController winner;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void UpdateLists(int segments)
    {
        raceNum++;
        Race newRace = new Race();
        newRace.RaceUpdate(segments, FindObjectsOfType<RaceChunk>());
        races.Add(newRace);

        BotController[] newAgents = FindObjectsOfType<BotController>();
        foreach (var obj in newAgents)
        {
            Agent temp = new Agent();
            temp.climbSpeed = obj.climbSpeed;
            temp.runSpeed = obj.runSpeed;
            temp.flySpeed = obj.flySpeed;
            temp.swimSpeed = obj.swimSpeed;
            temp.raceNumber = raceNum;
            if (winner == obj) temp.wins = true;
            agents.Add(temp);
        }
    }
}

[Serializable]
public class Race
{
    public float runPercent;
    public float flyPercent;
    public float swimPercent;
    public float climbPercent;

    public void RaceUpdate(float segments, RaceChunk[] chunks)
    {
        List<RaceChunk> runChunks = new List<RaceChunk>();
        List<RaceChunk> flyChunks = new List<RaceChunk>();
        List<RaceChunk> swimChunks = new List<RaceChunk>();
        List<RaceChunk> climbChunks = new List<RaceChunk>();

        foreach (var chunk in chunks)
        {
            switch (chunk.GetComponent<RaceChunk>().chunkType)
            {
                case ChunkType.Climb:
                    climbChunks.Add(chunk);
                    break;
                case ChunkType.Ground:
                    runChunks.Add(chunk);
                    break;
                case ChunkType.Fly:
                    flyChunks.Add(chunk);
                    break;
                case ChunkType.Swim:
                    swimChunks.Add(chunk);
                    break;
            }
        }
        
        runPercent = (runChunks.Count / segments) * 100;
        flyPercent = (flyChunks.Count / segments) * 100;
        swimPercent = (swimChunks.Count / segments) * 100;
        climbPercent = (climbChunks.Count / segments) * 100;
    }
}

[Serializable]
public class Agent
{
    public float runSpeed;
    public float climbSpeed;
    public float flySpeed;
    public float swimSpeed;
    public bool wins;
    public int raceNumber;
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DataType
{
    Run,
    Fly,
    Swim,
    Climb,
    Position
}
public class GameManager : MonoBehaviour
{
    public List<Race> races;
    public List<Agent> agents;
    private int raceNum = 0;
    public DataType firstDataPoint, secondDataPoint;
    [SerializeField] private GraphObject graph;
    [SerializeField] private HexGridManager hexGrid;

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
            temp.placement = obj.placement;
            temp.color = obj.GetComponent<SpriteRenderer>().color;

            agents.Add(temp);
        }
    }

    public void UpdateGraph()
    {
        graph.ClearPoints();
        foreach (var agent in agents)
        {
            Vector2 pos = Vector2.zero;
            switch (firstDataPoint)
            {
                case DataType.Run:
                    pos.x = agent.runSpeed;
                    break;
                case DataType.Climb:
                    pos.x = agent.climbSpeed;
                    break;
                case DataType.Swim:
                    pos.x = agent.swimSpeed;
                    break;
                case DataType.Fly:
                    pos.x = agent.flySpeed;
                    break;
                case DataType.Position:
                    pos.x = agent.placement;
                    break;
            }
            switch (secondDataPoint)
            {
                case DataType.Run:
                    pos.y = agent.runSpeed;
                    break;
                case DataType.Climb:
                    pos.y = agent.climbSpeed;
                    break;
                case DataType.Swim:
                    pos.y = agent.swimSpeed;
                    break;
                case DataType.Fly:
                    pos.y = agent.flySpeed;
                    break;
                case DataType.Position:
                    pos.y = agent.placement;
                    break;
            }
            graph.AddPoint(pos,agent.color);
        }

        hexGrid.UpdateHexes();
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
    public int placement;
    public int raceNumber;
    public Color color;
}
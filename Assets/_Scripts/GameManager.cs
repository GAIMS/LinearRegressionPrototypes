using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum DataType
{
    Run,
    Fly,
    Swim,
    Climb,
    Position,
    __________,
    Plus,
    Minus,
    Multiply,
    Divide
}
public class GameManager : MonoBehaviour
{
    public List<Race> races;
    public List<Agent> agents;
    private int raceNum = 0;
    
    public DataType[] DataPoint;
    public List<SavedData> savedData;

    [SerializeField] private GraphObject graph;
    [SerializeField] private HexGridManager hexGrid;
    private RaceTrackGenerator rtg;
    [SerializeField] private Button hexOn;
    [SerializeField] private Button hexOff;

    private List<float> predictionOutput;

    private void Awake()
    {
        rtg = FindObjectOfType<RaceTrackGenerator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void EndRace()
    {
        hexOn.onClick.Invoke();
        Time.timeScale = 1;
        //graph.gameObject.SetActive(true);
        //hexGrid.gameObject.SetActive(true);
        //UpdateGraph();
        //hexGrid.UpdateHexes();
    }

    public void NextRace()
    {
        hexOff.onClick.Invoke();
        graph.gameObject.SetActive(false);
        hexGrid.gameObject.SetActive(false);
        rtg.Restart();
    }

    public void GetPrediction()
    {
        BotController[] bots = FindObjectsOfType<BotController>();
        predictionOutput = new List<float>();
        for (int j = 0; j < bots.Length; j++)
        {
            Agent agent = new Agent();
            agent.climbSpeed = bots[j].climbSpeed;
            agent.runSpeed = bots[j].runSpeed;
            agent.flySpeed = bots[j].flySpeed;
            agent.swimSpeed = bots[j].swimSpeed;
            agent.raceNumber = raceNum;
            agent.placement = bots[j].placement;
            agent.color = bots[j].GetComponent<SpriteRenderer>().color;
            
            Vector2 pos = Vector2.zero;
            bool yAxis = false;
            for (int i = 0; i < DataPoint.Length; i++)
            {
                switch (DataPoint[i])
                    {
                        case DataType.Run:
                            if(!yAxis)
                                pos.x = agent.runSpeed;
                            break;
                        case DataType.Climb:
                            if(!yAxis)
                                pos.x = agent.climbSpeed;
                            break;
                        case DataType.Swim:
                            if(!yAxis)
                                pos.x = agent.swimSpeed;
                            break;
                        case DataType.Fly:
                            if(!yAxis)
                                pos.x = agent.flySpeed;
                            break;
                        case DataType.Position:
                            if(!yAxis)
                                pos.x = agent.placement;
                            break;
                        case DataType.Plus:
                            if(!yAxis)
                                pos.x = DataPointOperation(agent, DataPoint[i + 1], DataType.Plus, pos.x);
                            break;
                        case DataType.Minus:
                            if(!yAxis)
                                pos.x = DataPointOperation(agent, DataPoint[i + 1], DataType.Minus, pos.x);
                            break;
                        case DataType.Multiply:
                            if(!yAxis)
                                pos.x = DataPointOperation(agent, DataPoint[i + 1], DataType.Multiply, pos.x);
                            break;
                        case DataType.Divide:
                            if(!yAxis)
                                pos.x = DataPointOperation(agent, DataPoint[i + 1], DataType.Divide, pos.x);
                            break;
                        
                        case DataType.__________:
                            yAxis = true;
                            switch (DataPoint[i + 1])
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
                                    //Debug.Log(pos);
                                    break;
                                case DataType.Plus:
                                    pos.y = DataPointOperation(agent, DataPoint[i + 2], DataType.Plus, pos.y);
                                    break;
                                case DataType.Minus:
                                    pos.y = DataPointOperation(agent, DataPoint[i + 2], DataType.Minus, pos.y);
                                    break;
                                case DataType.Multiply:
                                    pos.y = DataPointOperation(agent, DataPoint[i + 2], DataType.Multiply, pos.y);
                                    break;
                                case DataType.Divide:
                                    pos.y = DataPointOperation(agent, DataPoint[i + 2], DataType.Divide, pos.y);
                                    break;
                                case DataType.__________:
                                    break;
                            }
                            break;
                    }
            }

            float pred = graph.Line.Line.Slope * pos.x + graph.Line.Line.YIntercept;
            predictionOutput.Add(pred);
        }

        int hightestIndex = 0;
        float hightestVal = 0;
        for (int i = 0; i < predictionOutput.Count; i++)
        {
            if (predictionOutput[i] > hightestVal)
            {
                hightestVal = predictionOutput[i];
                hightestIndex = i;
            }
        }
        Debug.Log(hightestIndex);
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
            bool yAxis = false;
            for (int i = 0; i < DataPoint.Length; i++)
            {
                switch (DataPoint[i])
                    {
                        case DataType.Run:
                            if(!yAxis)
                                pos.x = agent.runSpeed;
                            break;
                        case DataType.Climb:
                            if(!yAxis)
                                pos.x = agent.climbSpeed;
                            break;
                        case DataType.Swim:
                            if(!yAxis)
                                pos.x = agent.swimSpeed;
                            break;
                        case DataType.Fly:
                            if(!yAxis)
                                pos.x = agent.flySpeed;
                            break;
                        case DataType.Position:
                            if(!yAxis)
                                pos.x = agent.placement;
                            break;
                        case DataType.Plus:
                            if(!yAxis)
                                pos.x = DataPointOperation(agent, DataPoint[i + 1], DataType.Plus, pos.x);
                            break;
                        case DataType.Minus:
                            if(!yAxis)
                                pos.x = DataPointOperation(agent, DataPoint[i + 1], DataType.Minus, pos.x);
                            break;
                        case DataType.Multiply:
                            if(!yAxis)
                                pos.x = DataPointOperation(agent, DataPoint[i + 1], DataType.Multiply, pos.x);
                            break;
                        case DataType.Divide:
                            if(!yAxis)
                                pos.x = DataPointOperation(agent, DataPoint[i + 1], DataType.Divide, pos.x);
                            break;
                        
                        case DataType.__________:
                            yAxis = true;
                            switch (DataPoint[i + 1])
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
                                    //Debug.Log(pos);
                                    break;
                                case DataType.Plus:
                                    pos.y = DataPointOperation(agent, DataPoint[i + 2], DataType.Plus, pos.y);
                                    break;
                                case DataType.Minus:
                                    pos.y = DataPointOperation(agent, DataPoint[i + 2], DataType.Minus, pos.y);
                                    break;
                                case DataType.Multiply:
                                    pos.y = DataPointOperation(agent, DataPoint[i + 2], DataType.Multiply, pos.y);
                                    break;
                                case DataType.Divide:
                                    pos.y = DataPointOperation(agent, DataPoint[i + 2], DataType.Divide, pos.y);
                                    break;
                                case DataType.__________:
                                    break;
                            }
                            break;
                    }
            }
            Debug.Log(pos);
            graph.AddPoint(pos,agent.color);
        }
        SavedData data = new SavedData();
        data.operation = DataPoint;
        savedData.Add(data);
        hexGrid.UpdateHexes();
    }

    public float DataPointOperation(Agent agent,DataType dataType, DataType operation, float value)
    {
        switch (operation)
        {
            case DataType.Plus:
                return value += GetAgentDataValue(agent, dataType);
            case DataType.Minus:
                return value -= GetAgentDataValue(agent, dataType);
            case DataType.Multiply:
                return value *= GetAgentDataValue(agent, dataType);
            case DataType.Divide:
                return value /= GetAgentDataValue(agent, dataType);
        }

        return 0;
    }

    public float GetAgentDataValue(Agent agent,DataType dataType)
    {
        switch (dataType)
        {
            case DataType.Run:
                return agent.runSpeed;
            case DataType.Climb:
                return agent.climbSpeed;
            case DataType.Swim:
                return agent.swimSpeed;
            case DataType.Fly:
                return agent.flySpeed;
            case DataType.Position:
                return agent.placement;
        }

        return 0f;
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

[Serializable]
public class SavedData
{
    public DataType[] operation;
}
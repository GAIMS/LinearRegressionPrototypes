using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HexGridManager : MonoBehaviour
{
    [SerializeField] public int gridHeight = 15;
    [SerializeField] public int gridWidth = 15;
    [SerializeField] private float hexSize = .75f;
    [SerializeField] private GameObject hexPrefab;
    [SerializeField] private float offset = .05f;
    [SerializeField] private GraphObject graphObject;
    [SerializeField] private float slopeMax = 2;
    [SerializeField] private float hexValueMultiplier = .1f;
    [SerializeField] private float hexColorMultiplier = .025f;
    [SerializeField] public bool usingGraph;
    [SerializeField] public bool usingNoise;

    
    public Hex[,] hexes;

    private PerlinNoiseGeneration noiseGen;

    private Vector3[] cubeDirectionVectors = new Vector3[]
    {
        new Vector3(1, 0, -1),
        new Vector3(1, -1, 0),
        new Vector3(0, -1, 1),
        new Vector3(-1, 0, 1),
        new Vector3(-1, 1, 0),
        new Vector3(0, 1, -1)
    };
    
    private void Awake()
    {
        hexes = new Hex[gridWidth, gridHeight];
        graphObject = FindObjectOfType<GraphObject>();
        noiseGen = FindObjectOfType<PerlinNoiseGeneration>();
        CreateHexGrid(hexSize);
        UpdateHexes();
    }
    public void CreateHexGrid(float hexSize)
    {

        for (int x = 0; x < hexes.GetLength(0); x++)
        {
            for (int y = 0; y < hexes.GetLength(1); y++)
            {

                Hex hex = new Hex();
                float r = y - (x + (x & 1)) / 2;
                hex.position = new Vector3(x, r, -x - r);
                hexes[x,y] = hex;

                CreateHex(new Vector2(x, y), hexSize, x, y);
            }
        }
    }

    public void CreateHex(Vector2 pos, float size, int x, int y)
    {
        if (pos.x % 2 == 0)
        {
            pos = pos * (new Vector2(size * .75f, size * .865f) + (Vector2.one * offset));
        }
        else
        {
            pos = pos * (new Vector2(size * .75f, size * .865f) + 
            (Vector2.one * offset)) - Vector2.up * .44f * size;
        }

        GameObject hexObj = Instantiate(hexPrefab, Vector3.zero, Quaternion.identity, transform);
        hexObj.transform.localScale = Vector3.one * hexSize;
        hexObj.transform.localPosition = pos;
        hexes[x,y].hexObject = hexObj;
    }

    public List<Hex> GetExtendedNeighbors(Hex pickedHex, int hexRadius)
    {
        List<Hex> output = new List<Hex>();
        Vector3 pos = pickedHex.position + (cubeDirectionVectors[4] * hexRadius);
        Hex startHex = GetHex(pos);

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < hexRadius; j++)
            {
                pos = pos + cubeDirectionVectors[i];
                startHex = GetHex(pos);
                if (startHex != null)
                {
                    output.Add(startHex);
                }
            }
        }

        return output;
    }

    Hex GetHex(Vector3 pos)
    {
        foreach (var hex in hexes)
        {
            if (hex.position == pos)
            {
                return hex;
            }
        }

        return null;
    }
    
    public void UpdateHexes()
    {
        for (int x = 0; x < hexes.GetLength(0); x++)
        {
            for (int y = 0; y < hexes.GetLength(1); y++)
            {
                hexes[x,y].neighbors = GetExtendedNeighbors(hexes[x,y],1);
                
                hexes[x,y].hexObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;


                if (usingGraph)
                {
                    hexes[x,y].lineOfBestFit = new Line(
                        (-1 * slopeMax)+((2 * slopeMax * x)/hexes.GetLength(0)),
                        (-1 * (graphObject.Rect.height/2))+((2 * (graphObject.Rect.height/2) * y)/hexes.GetLength(1)));
                    graphObject.RedrawLine(hexes[x, y].lineOfBestFit);
                    hexes[x, y].hexValue = graphObject.CalculateTotalLoss() * hexValueMultiplier;
                }

                if (usingNoise)
                {
                    noiseGen.CalcNoise(this);
                }
                SetText(hexes[x,y]);
            }
        }
        if(usingGraph)
            graphObject.RedrawLine(graphObject.Points);
    }

    void SetText(Hex hex)
    {
        hex.hexObject.GetComponentInChildren<Text>().text = Mathf.Floor(Mathf.Abs(hex.hexValue)).ToString(); //((int) (hexes[x,y].hexValue * 10)).ToString();
        hex.hexObject.GetComponent<SpriteRenderer>().color = Color.red * Mathf.Abs(hex.hexValue * hexColorMultiplier);
        
        Vector3 randPos = LowestNeighbor(hex).hexObject.transform.position;
        Vector3 objectPos = hex.hexObject.transform.position;
        randPos.x = randPos.x - objectPos.x;
        randPos.y = randPos.y - objectPos.y;
                
        float angle =  Mathf.Atan2(randPos.y, randPos.x) * Mathf.Rad2Deg;
        hex.hexObject.GetComponentsInChildren<Text>()[1].transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        
        for (int i = 2; i < hex.hexObject.GetComponentsInChildren<Text>().Length; i++)
        {
            if (i == 2)
                hex.hexObject.GetComponentsInChildren<Text>()[i].text = hex.position.y.ToString();
            if (i == 3)
                hex.hexObject.GetComponentsInChildren<Text>()[i].text = hex.position.z.ToString();
            if (i == 4)
                hex.hexObject.GetComponentsInChildren<Text>()[i].text = hex.position.x.ToString();
        }
    }
    
    public Hex LowestNeighbor(Hex hex)
    {
        float min = 1000000;
        Hex lowest = null;
        for (int i = 0; i < hex.neighbors.Count; i++)
        {
            if (Mathf.Abs(hex.neighbors[i].hexValue) < min)
            {
                min = Mathf.Abs(hex.neighbors[i].hexValue);
                lowest = hex.neighbors[i];
            }
        }

        hex.lowestNeighbors = lowest;
        return lowest;
    }

    public class Hex
    {
        public Vector3 position;
        public float hexValue;
        public GameObject hexObject;
        
        public List<Hex> neighbors;
        public Hex lowestNeighbors;

        public Line lineOfBestFit;
    }
}

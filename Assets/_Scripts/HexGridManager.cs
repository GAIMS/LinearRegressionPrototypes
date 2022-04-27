using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGridManager : MonoBehaviour
{
    [SerializeField] public int gridHeight;
    [SerializeField] public int gridWidth;
    [SerializeField] private float hexSize;
    [SerializeField] private GameObject hexPrefab;
    [SerializeField] private float offset;
    [SerializeField] private GraphObject graphObject;
    [SerializeField] private float slopeMax;
    [SerializeField] private LayerMask hexMax;
    [SerializeField] private Text lossText;
    [SerializeField] private int hexRadius;

    private Hex pickedHex;
    private bool turn = false;
    private bool firstPick = true;
    
    public Hex[,] hexes;

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
        CreateHexGrid(hexSize);
        UpdateHexes();
        lossText.text = "Total Loss: " + graphObject.CalculateTotalLoss();
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

        graphObject.RedrawLine(graphObject.Points);
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out RaycastHit hit, Mathf.Infinity, hexMax))
            {
                if (firstPick)
                {
                    foreach (var hex in hexes)
                    {
                        if (hex.hexObject == hit.transform.gameObject)
                        {
                            hex.hexObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                            graphObject.RedrawLine(hex.lineOfBestFit);
                            graphObject.RedrawLossLines();
                            pickedHex = hex;
                            lossText.text = "Total Loss: " + graphObject.CalculateTotalLoss();
                        }
                    }
                    GetExtendedNeighbors();

                    firstPick = false;
                    turn = true;
                }

                if (turn)
                {
                    PickPont(hit);
                }

            }
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            graphObject.RedrawGraph();
            UpdateHexes();
            lossText.text = "Total Loss: " + graphObject.CalculateTotalLoss();
        }
    }

    public void PickPont(RaycastHit hit)
    {

        GetExtendedNeighbors();
        
        foreach (var hex in hexes)
        {
            if (hex.hexObject == hit.transform.gameObject && hex.hexObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled && hex.hexObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color == Color.white)
            {
                hex.hexObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                graphObject.RedrawLine(hex.lineOfBestFit);
                graphObject.RedrawLossLines();
                pickedHex = hex;
                lossText.text = "Total Loss: " + graphObject.CalculateTotalLoss();
            }
        }
        GetExtendedNeighbors();
    }

    public void GetExtendedNeighbors()
    {
        //Hex hex;
        Vector3 pos = pickedHex.position + (cubeDirectionVectors[4] * hexRadius);
        Hex startHex = GetHex(pos);

        foreach (var hex in hexes)
        {
            hex.hexObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.black;
        }
        
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < hexRadius; j++)
            {
                pos = pos + cubeDirectionVectors[i];
                startHex = GetHex(pos);
                if (startHex != null)
                {
                    startHex.hexObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }
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
        int thing = 0;
        for (int x = 0; x < hexes.GetLength(0); x++)
        {
            for (int y = 0; y < hexes.GetLength(1); y++)
            {
                hexes[x,y].hexObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                
                hexes[x,y].lineOfBestFit = new Line(
                    (-1 * slopeMax)+((2 * slopeMax * x)/hexes.GetLength(0)),
                    (-1 * (graphObject.Rect.height/2))+((2 * (graphObject.Rect.height/2) * y)/hexes.GetLength(1)));

                graphObject.RedrawLine(hexes[x, y].lineOfBestFit);
                
                hexes[x, y].hexValue = graphObject.CalculateTotalLoss();
                
                Debug.Log("Loss Value for :" +x + "," + y+ " :" + hexes[x, y].hexValue);
                
                thing++;
                hexes[x, y].hexObject.GetComponentInChildren<Text>().text = Mathf.Floor(Mathf.Abs(hexes[x, y].hexValue * 1f)).ToString(); //((int) (hexes[x,y].hexValue * 10)).ToString();
                hexes[x, y].hexObject.GetComponent<SpriteRenderer>().color = Color.red * Mathf.Abs(hexes[x, y].hexValue * .0025f);
                    
                GetNeighbors(hexes[x,y]);

                Vector3 randPos = LowestNeighbor(hexes[x, y]).hexObject.transform.position;
                Vector3 objectPos = hexes[x,y].hexObject.transform.position;
                randPos.x = randPos.x - objectPos.x;
                randPos.y = randPos.y - objectPos.y;
                
                float angle =  Mathf.Atan2(randPos.y, randPos.x) * Mathf.Rad2Deg;
                hexes[x,y].hexObject.GetComponentsInChildren<Text>()[1].transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
                    
                for (int i = 2; i < hexes[x,y].hexObject.GetComponentsInChildren<Text>().Length; i++)
                {
                    if (i == 2)
                        hexes[x, y].hexObject.GetComponentsInChildren<Text>()[i].text = hexes[x, y].position.y.ToString();
                    if (i == 3)
                        hexes[x, y].hexObject.GetComponentsInChildren<Text>()[i].text = hexes[x, y].position.z.ToString();
                    if (i == 4)
                        hexes[x, y].hexObject.GetComponentsInChildren<Text>()[i].text = hexes[x, y].position.x.ToString();
                }
                    
            }
        }
        graphObject.RedrawLine(graphObject.Points);
    }
    
    void GetNeighbors(Hex hex)
    {
        hex.neighbors = new List<Hex>();
        for (int x = 0; x < hexes.GetLength(0); x++)
        {
            for (int y = 0; y < hexes.GetLength(1); y++)
            {
                if (hexes[x, y].position == hex.position + new Vector3(1, 0, -1) ||
                    hexes[x, y].position == hex.position + new Vector3(1, -1, 0) ||
                    hexes[x, y].position == hex.position + new Vector3(0, -1, 1) ||
                    hexes[x, y].position == hex.position + new Vector3(-1, 0, 1) ||
                    hexes[x, y].position == hex.position + new Vector3(-1, 1, 0) ||
                    hexes[x, y].position == hex.position + new Vector3(0, 1, -1))
                {
                    hex.neighbors.Add(hexes[x,y]);
                }
            }
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
                Debug.Log("Min " + min);
                Debug.Log("Hex Value " + lowest.hexValue);

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGridGame : MonoBehaviour
{
    [SerializeField] private LayerMask hexLayer;
    [SerializeField] private Text lossText;
    [SerializeField] private int hexRadius;
    [SerializeField] private GraphObject graphObject;
    [SerializeField] private bool randomFirstPick;
    [SerializeField] private bool pickLowestPoint;
    [SerializeField] private bool cumulativeScore;
    [SerializeField] private bool adaptiveRadius;
    [SerializeField] private HexGridGame otherPlayer;
    [SerializeField] private Text gameOverText;

    public bool myTurn = false;
    private HexGridManager.Hex pickedHex;
    private bool turn = false;
    private bool firstPick = true;
    private int score;

    public HexGridManager.Hex[,] hexes;

    private HexGridManager hm;
    

    void Start()
    {
        hm = GetComponent<HexGridManager>();
        hexes = hm.hexes;
        if(hm.usingGraph)
            lossText.text = "Total Loss: " + graphObject.CalculateTotalLoss();
    }

    // Update is called once per frame
    void Update()
    {

        if (firstPick && randomFirstPick && !pickLowestPoint)
        {
            RandomFirstPick();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (firstPick||myTurn)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, hexLayer))
                {
                    Debug.Log("test");
                    foreach (var hex in hexes)
                    {
                        if (hex.position == hit.transform.position)
                        {
                            pickedHex = hex;
                            //Debug.Log("hit");
                        }
                    }
                    if (myTurn)
                    {
                        SwapTurn(hit);
                        if (firstPick)
                        {
                            FirstPick(hit);
                            UpdateColor();
                            firstPick = false;
                            turn = true;
                        }
                    
                        if (turn && !firstPick)
                        {
                            //FirstPick(hit);
                            PickPont(hit);
                        }

                    }

                    if (pickLowestPoint)
                    {
                        //Debug.Log("Test");
                        foreach (var hex in hexes)
                        {
                            if (hex.hexObject == hit.transform.gameObject)
                            {
                                hm.GenLowestPoint(hex);
                                pickLowestPoint = false;
                                firstPick = true;
                                myTurn = true;
                            }
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGraph();
        }
    }
    public void FirstPick(RaycastHit hit)
    {
        foreach (var hex in hexes)
        {
            if (hex.hexObject == hit.transform.gameObject)
            {
                hex.hexObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                if (hm.usingGraph)
                {
                    graphObject.RedrawLine(hex.lineOfBestFit);
                    graphObject.RedrawLossLines();
                }

                pickedHex = hex;
                if (cumulativeScore)
                {
                    score += (int) Mathf.Abs(pickedHex.hexValue);
                    lossText.text = "Total Loss: " + score;
                }
                else
                {
                    lossText.text = "Total Loss: " + pickedHex.hexValue;
                }
            }
            
        }
    }
    public void PickPont(RaycastHit hit)
    {
        Debug.Log("Here");
        UpdateColor();
        foreach (var hex in hexes)
        {
            if (hex.hexObject == hit.transform.gameObject&&
                hex.hexObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled &&
                hex.hexObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color == Color.white)
            {
                hex.hexObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                if (hm.usingGraph)
                {
                    graphObject.RedrawLine(hex.lineOfBestFit);
                    graphObject.RedrawLossLines();
                }

                pickedHex = hex;
                if (cumulativeScore)
                {
                    score += (int) Mathf.Abs(pickedHex.hexValue);
                    lossText.text = "Total Loss: " + score;
                }
                else
                {
                    lossText.text = "Total Loss: " + pickedHex.hexValue;
                }
            }
            
        }
        UpdateColor();
        if (pickedHex == hm.lowestHex && !firstPick)
        {
            //Debug.Log("Game Over");
            gameOverText.enabled = true;
        }
    }
    public void UpdateColor()
    {
        foreach (var hexCol in hexes)
        {
            hexCol.hexObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.black;
        }
        foreach (var hexCol in hexes)
        {
            hexCol.hexObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color =
                Color.black;
        }
        if(pickedHex != null)
        {
            foreach (var hexCol in hm.GetExtendedNeighbors(pickedHex, hexRadius))
            {
                hexCol.hexObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color =
                    Color.white;
            }
        }
    }
    public void SwapTurn(RaycastHit hit)
    {
        foreach (var hex in hexes)
        {
            if (hex.hexObject.transform.position == hit.transform.position)
            {
                if (firstPick || 
                    hex.hexObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color == Color.white)
                {
                    myTurn = false;
                    otherPlayer.myTurn = true;
                }
            }
        }
    }
    public void RandomFirstPick()
    {
        HexGridManager.Hex hex = hexes[Random.Range(0, hexes.GetLength(0)), Random.Range(0, hexes.GetLength(1))];
        hex.hexObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        if (hm.usingGraph)
        {
            graphObject.RedrawLine(hex.lineOfBestFit);
            graphObject.RedrawLossLines();
        }
        pickedHex = hex;
        if (cumulativeScore)
        {
            score += (int) Mathf.Abs(pickedHex.hexValue);
            lossText.text = "Total Loss: " + score;
        }
        else
        {
            lossText.text = "Total Loss: " + pickedHex.hexValue;
        }

        foreach (var hexCol in hexes)
        {
            hexCol.hexObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.black;
        }
        if(adaptiveRadius)
        {
            foreach (var hexCol in hm.GetExtendedNeighbors(pickedHex, Mathf.CeilToInt(pickedHex.hexValue / 10)))
            {
                hexCol.hexObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        else
        {
            foreach (var hexCol in hm.GetExtendedNeighbors(pickedHex, hexRadius))
            {
                hexCol.hexObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
            

        firstPick = false;
        turn = true;
    }
    public void ResetGraph()
    {
        if(hm.usingGraph)
            graphObject.RedrawGraph();
        hm.UpdateHexes();
        gameOverText.enabled = false;
        foreach (var hex in hexes)
        {
            hex.hexObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.black;
        }

        firstPick = true;
        pickLowestPoint = true;
        turn = false;

        if (cumulativeScore)
        {
            if(pickedHex != null)
                score += (int) Mathf.Abs(pickedHex.hexValue);
            lossText.text = "Total Loss: " + score;
        }
        else
        {
            lossText.text = "Total Loss: " + pickedHex.hexValue;
        }
    }
}
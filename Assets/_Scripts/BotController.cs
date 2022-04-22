using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotController : MonoBehaviour
{
    
    private HexGridManager _hexGridManager;
    private HexGridManager.Hex[,] hexes;
    private HexGridManager.Hex currentHex;
    private HexGridManager.Hex lastHex;
    
    
    void Start()
    {
        _hexGridManager = FindObjectOfType<HexGridManager>();
        hexes = _hexGridManager.hexes;
        currentHex = hexes[Random.Range(0, hexes.GetLength(0)),
                Random.Range(0, hexes.GetLength(1))];
        transform.position = currentHex.hexObject.transform.position;
        lastHex = currentHex;
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            currentHex = hexes[Random.Range(0, hexes.GetLength(0)),
                Random.Range(0, hexes.GetLength(1))];
            transform.position = currentHex.hexObject.transform.position;
            lastHex = currentHex;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Move();
        }
    }

    void Move()
    {
        currentHex = _hexGridManager.LowestNeighbor(lastHex);
        
        transform.position = currentHex.hexObject.transform.position;

        lastHex = currentHex;
    }
}

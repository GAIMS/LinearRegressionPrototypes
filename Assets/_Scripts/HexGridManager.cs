using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGridManager : MonoBehaviour
{
    [SerializeField] private int gridHeight;
    [SerializeField] private int gridWidth;
    [SerializeField] private float hexSize;
    [SerializeField] private GameObject hexPrefab;
    [SerializeField] private float offset;

    private Hex[] hexes;
    private Vector3 currentPos;
    
    private void Start()
    {
        int[,] grid = new int[gridHeight, gridWidth];
        float[,] value = new float[gridWidth, gridHeight];

        hexes = new Hex[gridHeight + gridWidth];
        
        //for (int x = 0; x < gridWidth; x++)
        //{
        //    for (int y = 0; y < gridHeight; y++)
        //    {
        //        //grid[x,y] = ;
        //    }
        //}
        CreateHexGrid(grid, value, hexSize);
    }

    public void CreateHexGrid(int[,] grid, float[,] value, float hexSize)
    {

        for (int x = 0; x < grid.GetLength(1); x++)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                float val = value[x,y];
                Hex test = new Hex(new Vector3(x, y, -x-y), 0);
                hexes[x+y] = test;
                currentPos = test.position;
                Debug.Log(test.position);
                CreateHex(new Vector2(x, y), 0, hexSize);
            }
        }
    }
    
    public void CreateHex(Vector2 pos, float value, float size)
    {
        if (pos.x % 2 == 0)
        {
            pos = pos * (new Vector2(size * .75f, size * .865f) + (Vector2.one * offset));
        }
        else
        {
            pos = pos * (new Vector2(size * .75f, size * .865f) + (Vector2.one * offset)) + Vector2.up * .44f * size;
        }
        GameObject hexObj = Instantiate(hexPrefab, Vector3.zero, Quaternion.identity, transform);
        hexObj.transform.localScale = Vector3.one * hexSize;
        hexObj.transform.localPosition = pos;
        hexObj.GetComponentInChildren<Text>().text = currentPos.ToString();
        //hexes[(int) pos.x, (int) pos.y] = hexObj;
    }
}

public class HexGrid
{
    public HexGrid(int[,] grid, float[,] value, float hexSize)
    {
        for (int x = 0; x < grid.GetLength(1); x++)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                new Hex(new Vector2(x, y), value[x, y]);
            }
        }
    }
}

public class Hex
{
    public Vector3 position;
    public float hexValue;
    public Hex(Vector3 pos, float value)
    {
        position = pos;
        hexValue = value;
    }
}

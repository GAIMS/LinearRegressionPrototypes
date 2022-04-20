using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridManager : MonoBehaviour
{
    [SerializeField] private int gridHeight;
    [SerializeField] private int gridWidth;
    [SerializeField] private float hexSize;
    [SerializeField] private GameObject hexPrefab;
    [SerializeField] private float offset;

    private Vector3[] hexesPos;
    
    private void Start()
    {
        int[,] grid = new int[gridHeight, gridWidth];
        float[,] value = new float[gridWidth, gridHeight];
        
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
        Debug.Log("x: " + value.GetLength(1) + " y: " + value.GetLength(0));

        for (int x = 0; x < grid.GetLength(1); x++)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                float val = value[x,y];
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
                new Hex(new Vector2(x, y), value[x, y], hexSize);
            }
        }
    }
}

public class Hex
{
    public Hex(Vector2 pos, float value, float size)
    {
        
    }
}

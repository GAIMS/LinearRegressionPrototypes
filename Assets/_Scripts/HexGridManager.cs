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

    private GameObject[,] hexes;
    
    private void Start()
    {
        int[,] grid = new int[gridWidth, gridHeight];
        //for (int x = 0; x < gridWidth; x++)
        //{
        //    for (int y = 0; y < gridHeight; y++)
        //    {
        //        //grid[x,y] = ;
        //    }
        //}
        CreateHexGrid(grid,new float[gridWidth, gridHeight], hexSize);
    }

    public void CreateHexGrid(int[,] grid, float[,] value, float hexSize)
    {
        for (int x = 0; x < grid.GetLength(1); x++)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                CreateHex(new Vector2(x, y), value[x, y], hexSize);
            }
        }
    }
    
    public void CreateHex(Vector2 pos, float value, float size)
    {
        if (pos.x % 2 == 0)
        {
            Vector2 position = pos * (new Vector2(.78f, .9f) * (size));
        }
        else
        {
            Vector2 position = pos * (new Vector2(.78f, .9f) * (size)) + Vector2.up * .44f * size;
        }
        GameObject hexObj = Instantiate(hexPrefab, pos, Quaternion.identity, transform);
        hexObj.transform.localScale = Vector3.one * hexSize;
        hexes[(int) pos.x, (int) pos.y] = hexObj;
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

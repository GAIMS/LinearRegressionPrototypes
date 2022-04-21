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

    public Hex[,] hexes;

    private void Awake()
    {
        hexes = new Hex[gridWidth, gridHeight];
        CreateHexGrid(hexSize);
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            int thing = 0;
            for (int x = 0; x < hexes.GetLength(0); x++)
            {
                for (int y = 0; y < hexes.GetLength(1); y++)
                {
                    thing++;
                    hexes[x,y].hexObject.GetComponentInChildren<Text>().text = ((int) (hexes[x,y].hexValue * 10)).ToString();
                    hexes[x, y].hexObject.GetComponent<SpriteRenderer>().color = Color.red * hexes[x, y].hexValue;
                    
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
        }
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

    Hex LowestNeighbor(Hex hex)
    {
        float min = 1000000;
        Hex lowest = null;
        for (int i = 0; i < hex.neighbors.Count; i++)
        {
            if (hex.neighbors[i].hexValue < min)
            {
                min = hex.neighbors[i].hexValue;
                lowest = hex.neighbors[i];
            }
        }
        return lowest;
    }

    public class Hex
    {
        public Vector3 position;
        public float hexValue;
        public GameObject hexObject;
        
        public List<Hex> neighbors;
    }
}

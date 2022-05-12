using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PerlinNoiseGeneration : MonoBehaviour
{
    [SerializeField] public int pxlWidth;
    [SerializeField] public int pxlHeight;

    [SerializeField] private float xOrg;
    [SerializeField] private float yOrg;

    [SerializeField] private float scale = 1f;
    //[SerializeField] private GameObject numPrefab;
    
    [SerializeField] private string seed;
    [SerializeField] private bool useRandomSeed;

    public GridLayoutGroup layoutGroup;
    public float lowestPoint = .5f, highestPoint = .5f;
    public Vector2 lowestCoord, highestCoord;
    
    private Color[] pix;
    private Renderer rend;
    private float randPosX,randPosY;

    private HexGridManager hexManager;
    
    void Start()
    {
        hexManager = FindObjectOfType<HexGridManager>();
        pxlWidth = hexManager.gridWidth;
        pxlHeight = hexManager.gridHeight;
        CalcNoise();
    }

    void CalcNoise()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        randPosX = pseudoRandom.Next(-100, 100);
        randPosY = pseudoRandom.Next(-100, 100);
        
        layoutGroup.constraintCount = pxlHeight;
        float y = 0;
        Vector2 test1 = Vector2.zero, test2 = Vector2.zero;
        
        while (y<hexManager.hexes.GetLength(1))
        {
            float x = 0;
            while (x < hexManager.hexes.GetLength(0))
            {
                float xCoord = (xOrg + randPosX) + x / pxlWidth * scale;
                float yCoord = (yOrg + randPosY) + y / pxlHeight * scale;

                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                //Text text = Instantiate(numPrefab, layoutGroup.transform).GetComponent<Text>();
                //text.text = ((int)(sample * 10)).ToString();

                hexManager.hexes[(int)x , (int)y].hexValue = sample;
                
                if (sample > highestPoint)
                {
                    Debug.Log("High: " + sample);
                    highestPoint = sample;
                    highestCoord = new Vector2(x, y);
                    test1 = new Vector2(x, y);
                }

                if (sample < lowestPoint)
                {
                    Debug.Log("Low: " + sample);
                    lowestPoint = sample;
                    lowestCoord = new Vector2(x, y);
                    test2 = new Vector2(x, y);
                }
                x++;
            }

            y++;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            CalcNoise();
        }
    }
}

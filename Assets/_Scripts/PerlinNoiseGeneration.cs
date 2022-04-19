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
    [SerializeField] private GameObject numPrefab;

    public GridLayoutGroup layoutGroup;
    public float lowestPoint = .5f, highestPoint = .5f;
    public Vector2 lowestCoord, highestCoord;
    
    private Color[] pix;
    private Renderer rend;
    private float randPosX,randPosY;
    
    
    void Awake()
    {
        //layoutGroup.constraintCount = pxlHeight;
        //rend = GetComponent<Renderer>();

        //noiseTex = new Texture2D(pxlWidth, pxlHeight);
        //pix = new Color[noiseTex.width * noiseTex.height];
        //rend.material.mainTexture = noiseTex;
        randPosX = Random.Range(0, 1000);
        randPosY = Random.Range(0, 1000);
        CalcNoise();
    }

    void CalcNoise()
    {
        float y = 0;
        Vector2 test1 = Vector2.zero, test2 = Vector2.zero;
        while (y<pxlHeight)
        {
            float x = 0;
            while (x < pxlWidth)
            {
                float xCoord = (xOrg + randPosX) + x / pxlWidth * scale;
                float yCoord = (yOrg + randPosY) + y / pxlHeight * scale;

                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                //pix[(int) y * noiseTex.width + (int) x] = new Color(sample, sample, sample);
                Text text = Instantiate(numPrefab, layoutGroup.transform).GetComponent<Text>();
                text.text = ((int)(sample * 10)).ToString();
                
                if (sample > highestPoint)
                {
                    Debug.Log("High: " + sample);
                    highestPoint = sample;
                    highestCoord = new Vector2(((int) -x + (pxlWidth/2))*.1f, ((int) -y + (pxlHeight/2))*.1f);
                    test1 = new Vector2(x, y);
                }

                if (sample < lowestPoint)
                {
                    Debug.Log("Low: " + sample);
                    lowestPoint = sample;
                    lowestCoord = new Vector2(((int) -x + (pxlWidth/2))*.1f, ((int) -y + (pxlHeight/2))*.1f);
                    test2 = new Vector2(x, y);
                }
                x++;
            }

            y++;
        }
        //noiseTex.SetPixels(pix);
        //noiseTex.SetPixel((int)test1.x,(int)test1.y,Color.green);
        //noiseTex.SetPixel((int)test2.x,(int)test2.y,Color.blue);
        //noiseTex.Apply();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Text[] obj = FindObjectsOfType<Text>();

            foreach (var text in obj)
            {
                Destroy(text.gameObject);
            }
            
            CalcNoise();
        }
    }
}

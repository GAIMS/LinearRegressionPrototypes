using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    [SerializeField] public int pxlWidth;
    [SerializeField] public int pxlHeight;

    [SerializeField] private float xOrg;
    [SerializeField] private float yOrg;

    [SerializeField] private float scale = 1f;
    
    [SerializeField] private string seed;
    [SerializeField] private bool useRandomSeed;

    public Texture2D noiseTex;
    public float lowestPoint, highestPoint = .5f;
    public Vector2 lowestCoord, highestCoord;
    
    private Color[] pix;
    private Renderer rend;
    private float randPosX,randPosY;
    
    
    void Awake()
    {
        rend = GetComponent<Renderer>();

        noiseTex = new Texture2D(pxlWidth, pxlHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
        rend.material.mainTexture = noiseTex;
        
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        randPosX = pseudoRandom.Next(-100, 100);
        randPosY = pseudoRandom.Next(-100, 100);
        
        CalcNoise();
    }

    void CalcNoise()
    {
        float y = 0;
        Vector2 test1 = Vector2.zero, test2 = Vector2.zero;
        while (y<noiseTex.height)
        {
            float x = 0;
            while (x < noiseTex.width)
            {
                float xCoord = (xOrg + randPosX) + x / noiseTex.width * scale;
                float yCoord = (yOrg + randPosY) + y / noiseTex.height * scale;

                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pix[(int) y * noiseTex.width + (int) x] = new Color(sample, sample, sample);
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
        noiseTex.SetPixels(pix);
        //noiseTex.SetPixel((int)test1.x,(int)test1.y,Color.green);
        //noiseTex.SetPixel((int)test2.x,(int)test2.y,Color.blue);
        noiseTex.Apply();
    }
    
    void Update()
    {

    }
}
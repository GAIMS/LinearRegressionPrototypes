using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    [SerializeField] private int pxlWidth;
    [SerializeField] private int pxlHeight;

    [SerializeField] private float xOrg;
    [SerializeField] private float yOrg;

    [SerializeField] private float scale = 1f;

    public Texture2D noiseTex;
    private Color[] pix;
    private Renderer rend;
    
    
    void Start()
    {
        rend = GetComponent<Renderer>();

        noiseTex = new Texture2D(pxlWidth, pxlHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
        rend.material.mainTexture = noiseTex;
    }

    void CalcNoise()
    {
        float y = 0;

        while (y<noiseTex.height)
        {
            float x = 0;
            while (x < noiseTex.width)
            {
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;

                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pix[(int) y * noiseTex.width + (int) x] = new Color(sample, sample, sample);
                x++;
            }

            y++;
        }
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }
    
    void Update()
    {
        CalcNoise();
    }
}

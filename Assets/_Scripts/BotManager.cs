using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BotManager : MonoBehaviour
{
    [SerializeField] private int botNum;
    [SerializeField] private GameObject botPrefab;
    [SerializeField] private bool usingNoise;
    [SerializeField] private bool refreshSlopeIndicators;
    [SerializeField] private float resetVelocity;
    [SerializeField] private float avgVelocity;
    [SerializeField] private int generationNum = 0;
    [SerializeField] private Text generationText;
    [SerializeField] private float generationTime;

    private float timer = 10;
    private float genTimer = 0;
    private NoiseGenerator _noiseGen;
    private List<BotController> bots;
    void Start()
    {
        if (usingNoise)
        {
            _noiseGen = FindObjectOfType<NoiseGenerator>();
            transform.position = _noiseGen.highestCoord;
        }
        StartGen();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        genTimer -= Time.deltaTime;

        if (genTimer <= 0)
        {
            foreach (var bot in bots)
            {
                bot.ResetGen();
                Destroy(bot.gameObject);
            }
            StartGen();
        }
        
        if(timer <= 0)
        {
            avgVelocity = 0;
            for (int i = 0, z = 1; i < bots.Count; i++)
            { 
                if(!bots[i].inHole)
                {
                    float vel = bots[i].distanceDelta;
                    Debug.Log(vel);
                    avgVelocity = ((avgVelocity + vel) / (z)) * 10;
                    z++;
                }
                //Debug.Log(i);
                if (i >= (bots.Count - 1))
                {
                    if (avgVelocity < resetVelocity)
                    {
                        timer = 10;
                        foreach (var bot in bots)
                        {
                            bot.ResetGen();
                            Destroy(bot.gameObject);
                        }
                        StartGen();
                    }
                }
                else
                {
                    timer = .1f;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void StartGen()
    {
        for (int x = 0; x < _noiseGen.pxlWidth; x++)
        {
            for (int y = 0; y < _noiseGen.pxlHeight; y++)
            {
                _noiseGen.noiseTex.SetPixel(x,y, new Color(_noiseGen.noiseTex.GetPixel(x,y).r, _noiseGen.noiseTex.GetPixel(x,y).b, _noiseGen.noiseTex.GetPixel(x,y).b));
            }
        }
        _noiseGen.noiseTex.Apply();
        genTimer = generationTime;
        generationNum++;
        generationText.text = "Generation: " + generationNum;
        bots = new List<BotController>();
        for (int i = 0; i < botNum; i++)
        {
            GameObject bot = Instantiate(botPrefab, transform.position, quaternion.identity);
            bots.Add(bot.GetComponent<BotController>());
        }
    }
}

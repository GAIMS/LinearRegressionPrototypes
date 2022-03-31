using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BotManager : MonoBehaviour
{
    [SerializeField] private int botNum;
    [SerializeField] private GameObject botPrefab;
    [SerializeField] private bool usingNoise;
    [SerializeField] private float resetVelocity;
    [SerializeField] private float avgVelocity;

    private float timer = 5;
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
        if(timer <= 0)
        {
            for (int i = 0; i < bots.Count; i++)
            {
                float vel = bots[i]._rb.velocity.magnitude;
                Debug.Log(vel);
                avgVelocity = ((avgVelocity + vel) / (i + 1)) * 1;
                if (i > (bots.Count / 2))
                {
                    if (avgVelocity < resetVelocity)
                    {
                        timer = 5;
                        foreach (var bot in bots)
                        {
                            bot.ResetGen();
                            Destroy(bot);
                            StartGen();
                        }
                    }
                }
            }

            timer = 3;
        }
    }

    public void StartGen()
    {
        bots = new List<BotController>();
        for (int i = 0; i < botNum; i++)
        {
            GameObject bot = Instantiate(botPrefab, transform.position, quaternion.identity);
            bots.Add(bot.GetComponent<BotController>());
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private List<BotController> bots;
    private RaceTrackGenerator rtg;
    void Start()
    {
        rtg = FindObjectOfType<RaceTrackGenerator>();
        //bots = FindObjectsOfType<BotController>();
        FindBots();
    }

    public void FindBots()
    {
        bots = new List<BotController>();
        for (int i = 0; i < rtg.racers; i++)
        {
            bots.Add(FindObjectsOfType<BotController>()[i]);
        }
    }
    
    void Update()
    {
        var totalX = 0f;
        var totalY = 0f;
        foreach(var bot in bots)
        {
            if(bot != null)
            {
                totalX += bot.transform.position.x;
                totalY += bot.transform.position.y;
            }
        }
        var centerX = totalX / bots.Count;
        var centerY = totalY / bots.Count;
        transform.position = new Vector3(centerX, centerY, transform.position.z);
    }
}

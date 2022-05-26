using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private List<BotController> bots;
    private RaceTrackGenerator rtg;
    private CinemachineTargetGroup targetGroup;
    [SerializeField] private float radius = 25f;
    void Start()
    {
        GetComponent<CinemachineVirtualCamera>().enabled = true;
        rtg = FindObjectOfType<RaceTrackGenerator>();
        //bots = FindObjectsOfType<BotController>();
        targetGroup = GetComponentInChildren<CinemachineTargetGroup>();
        FindBots();
    }

    public void FindBots()
    {
        bots = new List<BotController>();
        targetGroup.m_Targets = new CinemachineTargetGroup.Target[rtg.racers];
        for (int i = 0; i < rtg.racers; i++)
        {
            bots.Add(FindObjectsOfType<BotController>()[i]);
            targetGroup.m_Targets[i].target = bots[i].transform;
            targetGroup.m_Targets[i].weight = 1;
            targetGroup.m_Targets[i].radius = radius;
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
        //transform.position = new Vector3(centerX, centerY, transform.position.z);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 35;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
    }
}

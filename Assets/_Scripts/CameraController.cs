using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private BotController[] bots;
    void Start()
    {
        bots = FindObjectsOfType<BotController>();
    }

    
    void Update()
    {
        var totalX = 0f;
        var totalY = 0f;
        foreach(var bot in bots)
        {
            totalX += bot.transform.position.x;
            totalY += bot.transform.position.y;
        }
        var centerX = totalX / bots.Length;
        var centerY = totalY / bots.Length;
        transform.position = new Vector3(centerX, centerY, transform.position.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BotManager : MonoBehaviour
{
    [SerializeField] private int botNum;
    [SerializeField] private GameObject botPrefab;
    void Start()
    {
        for (int i = 0; i < botNum; i++)
        {
            Instantiate(botPrefab, transform.position, quaternion.identity);
        }
    }
    
}

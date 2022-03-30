using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BotManager : MonoBehaviour
{
    [SerializeField] private int botNum;
    [SerializeField] private GameObject botPrefab;
    [SerializeField] private bool usingNoise;
    private NoiseGenerator _noiseGen;
    void Start()
    {
        if (usingNoise)
        {
            _noiseGen = FindObjectOfType<NoiseGenerator>();
            transform.position = _noiseGen.highestCoord;
        }
        for (int i = 0; i < botNum; i++)
        {
            Instantiate(botPrefab, transform.position, quaternion.identity);
        }
    }
    
}

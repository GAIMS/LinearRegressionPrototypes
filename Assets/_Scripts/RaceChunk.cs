using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChunkType
{
    Ground,
    Swim,
    Climb,
    Fly
}

public class RaceChunk : MonoBehaviour
{
    public ChunkType chunkType;

    public Transform lftPoint, rtPoint;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}

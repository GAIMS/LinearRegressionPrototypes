using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChunkType
{
	Start,
    Ground,
    Swim,
    Climb,
    Fly,
    Finish
}

public class RaceChunk : MonoBehaviour
{
    public ChunkType chunkType;

    public Transform lftPoint, rtPoint;
	
}
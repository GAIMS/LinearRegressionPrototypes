using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{
    [SerializeField] private TrailRenderer trail;
    void Start()
    {
        Color col = Random.ColorHSV();
        gameObject.GetComponent<SpriteRenderer>().color = col;
        trail.startColor = col;
        trail.endColor = col;
    }

    void Update()
    {
        
    }
}

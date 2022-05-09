using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomPlacer : MonoBehaviour
{
    [SerializeField] private Vector2 placementArea;
    [SerializeField] private int numOfObjects;
    [SerializeField] private GameObject objPrefab;

    private List<GameObject> objects;
    void Start()
    {
        objects = new List<GameObject>();
        PlaceRandom();
    }


    void Update()
    {
        
    }

    public void PlaceRandom()
    {
        for (int i = 0; i < numOfObjects; i++)
        {
            Vector2 randPos = new Vector2(Random.Range(-placementArea.x / 2, placementArea.x / 2),
                Random.Range(-placementArea.y / 2, placementArea.y / 2));

            GameObject newObj = Instantiate(objPrefab, randPos, Quaternion.identity);
            
            objects.Add(newObj);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, placementArea);
        Gizmos.color = Color.red;
    }
}

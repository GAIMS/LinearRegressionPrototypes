using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GraphObject playerGraph;
    [SerializeField] private GraphObject targetGraph;
    [SerializeField] private float errorMargin = .1f;
    [SerializeField] private Text scoreUI;

    public int score = 0;
    void Start()
    {
        NewGraph();
    }

    
    void Update()
    {
        if (playerGraph.Line.Line.A.X >= targetGraph.Line.Line.A.X - errorMargin &&
            playerGraph.Line.Line.A.X <= targetGraph.Line.Line.A.X + errorMargin &&
            playerGraph.Line.Line.B.X <= targetGraph.Line.Line.B.X + errorMargin &&
            playerGraph.Line.Line.B.X >= targetGraph.Line.Line.B.X - errorMargin &&
            playerGraph.Line.Line.A.Y >= targetGraph.Line.Line.A.Y - errorMargin &&
            playerGraph.Line.Line.A.Y <= targetGraph.Line.Line.A.Y + errorMargin &&
            playerGraph.Line.Line.B.Y >= targetGraph.Line.Line.B.Y - errorMargin &&
            playerGraph.Line.Line.B.Y <= targetGraph.Line.Line.B.Y + errorMargin)
        {
            score++;
            NewGraph();
        }
    }

    public void NewGraph()
    {
        scoreUI.text = "Score: " + score;
        targetGraph.RedrawGraph();
    }
}

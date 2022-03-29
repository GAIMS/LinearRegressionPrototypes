using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Layer Incoming;
    public Layer Outgoing;
    public List<int> Weights;
    public List<Tower> Towers;

    public void UpdateIncoming(Layer layer) {

    }

    public void UpdateOutgoing(Layer layer) {

    }
}

public struct Connection {
    public Node Node;
    public float Weight;
}
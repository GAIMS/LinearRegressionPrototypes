using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Network
{
    public Node Start;
    public Node End;
    public List<Layer> Layers = new List<Layer>();

    public void AddLayer(int size) {
        Layer layer = new Layer();
        for (int i = 0; i < size; i++) {
            Node node = new Node();
            layer.Nodes.Add(node);
        }
        Layers.Add(layer);
    }
}

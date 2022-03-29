using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GraphTest : MonoBehaviour
{
    public Network Network;
    public int LayerSize;

    public NodeObject Node;
    public LineObject Line;

    public List<GameObject> NetworkObjects = new List<GameObject>();

    public float dM;

    [ContextMenu("Render Graph")]
    public void RenderGraph() {
        ClearGraph();

        // create nodes
        // create start node
        NodeObject obj = Instantiate(Node, this.transform);
        NetworkObjects.Add(obj.gameObject);
        obj.transform.localPosition = new Vector3(0*dM, 0*dM, 0*dM);
        for(int i = 0; i < Network.Layers.Count; i++)
        {
            Layer layer = Network.Layers[i];
            for (int j = 0; j < layer.Nodes.Count; j++) {
                Node node = layer.Nodes[j];
                obj = Instantiate(Node, this.transform);
                NetworkObjects.Add(obj.gameObject);
                obj.transform.localPosition = new Vector3((i + 1)*dM, (j-(layer.Nodes.Count-1)/2f)*dM, 0*dM);
                // set weight text
            }
        }
        // create end node
        obj = Instantiate(Node, this.transform);
        NetworkObjects.Add(obj.gameObject);
        obj.transform.localPosition = new Vector3((Network.Layers.Count + 1)*dM, 0*dM, 0*dM);

        // create lines
        // create lines from start node
        for (int i = 0; i < Network.Layers[0].Nodes.Count; i++) {
            LineObject line = Instantiate(Line, this.transform);
            NetworkObjects.Add(line.gameObject);
            Vector3[] positions = new Vector3[] { new Vector3(0*dM,0*dM,0*dM), new Vector3(1*dM,(i-(Network.Layers[0].Nodes.Count-1)/2f)*dM,0*dM)};
            line.Line.SetPositions(positions);
        }
        // create lines from layer to layer
        for(int i = 0; i < Network.Layers.Count - 1; i++)
        {
            Layer layerA = Network.Layers[i];
            for (int j = 0; j < layerA.Nodes.Count; j++) {
                Node node = layerA.Nodes[j];
                Layer layerB = Network.Layers[i+1];
                for (int k = 0; k < layerB.Nodes.Count; k++) {
                    LineObject line = Instantiate(Line, this.transform);
                    NetworkObjects.Add(line.gameObject);
                    Vector3[] positions = new Vector3[] { new Vector3((i+1)*dM,(j-(layerA.Nodes.Count-1)/2f)*dM,0*dM), new Vector3((i+2)*dM,(k-(layerB.Nodes.Count-1)/2f)*dM,0*dM)};
                    line.Line.SetPositions(positions);
                }
            }
        }
        // create lines to final node
        for (int i = 0; i < Network.Layers[Network.Layers.Count - 1].Nodes.Count; i++) {
            LineObject line = Instantiate(Line, this.transform);
            NetworkObjects.Add(line.gameObject);
            Vector3[] positions = new Vector3[] { 
                new Vector3( (Network.Layers.Count)*dM,
                (i-((Network.Layers[Network.Layers.Count-1].Nodes.Count-1)/2f))*dM,
                0*dM), 
                new Vector3( (Network.Layers.Count+1)*dM, 0*dM, 0*dM)
            };
            line.Line.SetPositions(positions);
        }
    }

    [ContextMenu("Clear Graph")]
    public void ClearGraph() {
        for (int i = NetworkObjects.Count - 1; i >= 0; i--) {
            DestroyImmediate(NetworkObjects[i]);
        }
        NetworkObjects.Clear();
    }

    [ContextMenu("Add Layer")]
    public void AddLayer() {
        Network.AddLayer(LayerSize);
        Debug.Log("New Layer Total: " + Network.Layers.Count);
    }
}

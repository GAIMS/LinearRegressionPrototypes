using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class VoxelGrid : MonoBehaviour
{
    [SerializeField] private int resolutionWidth;
    [SerializeField] private int resolutionHeight;
    [SerializeField] private GameObject voxelPrefab;

    private bool[] voxels;
    private float voxelSize;
    
    public void Initilize(int resolution, float size)
    {
        voxelSize = size / resolution;
        voxels = new bool[resolution * resolution];

        for (int i = 0, y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++, i++)
            {
                CreateVoxel(i, x, y);
            }
        }
    }

    void CreateVoxel(int i, int x, int y)
    {
        GameObject o = Instantiate(voxelPrefab) as GameObject;
        o.transform.parent = transform;
        o.transform.localPosition = new Vector3((x + .5f) * voxelSize, (y + .5f) * voxelSize);
        o.transform.localScale = Vector3.one * voxelSize * .9f;
    }

    void Update()
    {
        
    }
}

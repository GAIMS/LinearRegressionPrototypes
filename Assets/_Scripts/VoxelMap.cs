using System;
using UnityEngine;


    public class VoxelMap : MonoBehaviour
    {
        [SerializeField] private float size = 2f;
        
        [SerializeField] private int voxelResolution = 8;
        [SerializeField] private int chunkResolution = 2;
        
        [SerializeField] private VoxelGrid voxelGridPrefab;

        private VoxelGrid[] chunks;
        private float chunkSize, voxelSize, halfSize;

        private void Awake()
        {
            halfSize = size * .5f;
            chunkSize = size / chunkResolution;
            voxelSize = chunkSize / voxelResolution;

            chunks = new VoxelGrid[chunkResolution * chunkResolution];
            for (int i = 0, y = 0; y < chunkResolution; y++)
            {
                for (int x = 0; x < chunkResolution; x++, i++)
                {
                    CreateChunk(i, x, y);
                }
            }
        }

        void CreateChunk(int i, int x, int y)
        {
            VoxelGrid chunk = Instantiate(voxelGridPrefab) as VoxelGrid;
            chunk.Initilize(voxelResolution,chunkSize);
            chunk.transform.parent = transform;
            chunk.transform.localPosition = new Vector3(x * chunkSize - halfSize, y * chunkSize - halfSize);
            chunks[i] = chunk;
        }
    }
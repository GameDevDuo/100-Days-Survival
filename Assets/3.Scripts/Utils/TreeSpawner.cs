using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] treePrefab;
    [SerializeField] private Terrain terrain;
    [SerializeField] private int[] treeCount;

    void Start()
    {
        for (int i = 0; i < treePrefab.Length; i++)
        {
            for (int j = 0; j < treeCount[i]; j++)
            {
                Debug.Log(j);
                float x = Random.Range(0, terrain.terrainData.size.x);
                float z = Random.Range(0, terrain.terrainData.size.z);
                float y = terrain.SampleHeight(new Vector3(x, 0, z));
                Vector3 position = new Vector3(x, y, z);
                Instantiate(treePrefab[i], position, Quaternion.identity);
            }
        }
    }
}
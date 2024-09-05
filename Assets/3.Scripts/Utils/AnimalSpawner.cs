using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] animalPrefab;
    [SerializeField] private Terrain terrain;
    [SerializeField] private int[] animalCount;
    [SerializeField] private int[] texture;
    [SerializeField] private float seaHeight;

    void Start()
    {
        for (int i = 0; i < animalPrefab.Length; i++)
        {
            for (int j = 0; j < animalCount[i]; j++)
            {
                float x = Random.Range(0, terrain.terrainData.size.x);
                float z = Random.Range(0, terrain.terrainData.size.z);
                float y = terrain.SampleHeight(new Vector3(x, 0, z));
                Vector3 position = new Vector3(x, y, z);

                if (OnGroundTexture(position, i) && SeaHeight(y))
                {
                    GameObject tree = Instantiate(animalPrefab[i], position, Quaternion.identity);
                    tree.transform.parent = transform;
                }
            }
        }
    }

    bool OnGroundTexture(Vector3 position, int index)
    {
        Vector3 terrainPosition = position - terrain.transform.position;
        float x = terrainPosition.x / terrain.terrainData.size.x;
        float z = terrainPosition.z / terrain.terrainData.size.z;

        int mapX = Mathf.RoundToInt(x * terrain.terrainData.alphamapWidth);
        int mapZ = Mathf.RoundToInt(z * terrain.terrainData.alphamapHeight);

        float[,,] Map = terrain.terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        return Map[0, 0, texture[index]] > 0.5f;
    }

    bool SeaHeight(float y)
    {
        return y > seaHeight;
    }
}
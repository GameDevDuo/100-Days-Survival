using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] treePrefab;
    [SerializeField] private Terrain terrain;
    [SerializeField] private int[] treeCount;
    [SerializeField] private int[] texture;
    [SerializeField] private float seaHeight;
    [SerializeField] private bool[] useCustomSize;
    [SerializeField] private Vector2[] customSizeRange;

    void Start()
    {
        for (int i = 0; i < treePrefab.Length; i++)
        {
            for (int j = 0; j < treeCount[i]; j++)
            {
                float x = Random.Range(0, terrain.terrainData.size.x);
                float z = Random.Range(0, terrain.terrainData.size.z);
                float y = terrain.SampleHeight(new Vector3(x, 0, z));
                Vector3 position = new Vector3(x, y, z);

                if (OnGroundTexture(position, i) && SeaHeight(y))
                {
                    GameObject tree = Instantiate(treePrefab[i], position, Quaternion.identity);
                    tree.transform.parent = transform;

                    if (useCustomSize.Length > i && useCustomSize[i])
                    {
                        float scale = Random.Range(customSizeRange[i].x, customSizeRange[i].y);
                        tree.transform.localScale = new Vector3(scale, scale, scale);
                    }
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

        float[,,] map = terrain.terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        return map[0, 0, texture[index]] > 0.5f;
    }

    bool SeaHeight(float y)
    {
        return y > seaHeight;
    }
}
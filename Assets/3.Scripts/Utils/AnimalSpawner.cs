using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] animalPrefab;
    [SerializeField] private Terrain terrain;
    [SerializeField] private int[] animalCount;
    [SerializeField] private int[] texture;
    [SerializeField] private float seaHeight;
    [SerializeField] private float navMeshSampleDistance = 100.0f;
    [SerializeField] private float maxSlope = 30.0f;

    void Start()
    {
        for (int i = 0; i < animalPrefab.Length; i++)
        {
            for (int j = 0; j < animalCount[i]; j++)
            {
                Vector3 randomPosition = GetRandomPositionOnTerrain();

                if (OnGroundTexture(randomPosition, i) && SeaHeight(randomPosition.y) && IsFlatEnough(randomPosition))
                {
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(randomPosition, out hit, navMeshSampleDistance, NavMesh.AllAreas))
                    {
                        GameObject animal = Instantiate(animalPrefab[i], hit.position, Quaternion.identity);
                        animal.transform.parent = transform;

                        NavMeshAgent agent = animal.GetComponent<NavMeshAgent>();
                        if (agent != null)
                        {
                            agent.Warp(hit.position);
                            StartCoroutine(RepositionAfterTime(agent, 2f));
                        }
                    }
                }
            }
        }
    }

    IEnumerator RepositionAfterTime(NavMeshAgent agent, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (agent.isOnNavMesh)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(agent.transform.position, out hit, navMeshSampleDistance, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
            }
        }
    }

    bool IsFlatEnough(Vector3 position)
    {
        Vector3 terrainPosition = position - terrain.transform.position;
        float xNorm = terrainPosition.x / terrain.terrainData.size.x;
        float zNorm = terrainPosition.z / terrain.terrainData.size.z;
        float slope = terrain.terrainData.GetSteepness(xNorm, zNorm);

        return slope <= maxSlope;
    }

    Vector3 GetRandomPositionOnTerrain()
    {
        float x = Random.Range(0, terrain.terrainData.size.x);
        float z = Random.Range(0, terrain.terrainData.size.z);
        float y = terrain.SampleHeight(new Vector3(x, 0, z));
        return new Vector3(x, y, z);
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

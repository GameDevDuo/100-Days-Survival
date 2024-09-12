using UnityEngine;

public abstract class RandomPosBase : MonoBehaviour, IRandomPos, IFindTerrain
{
    protected Terrain terrain;
    protected Collider terrainCollider;

    public virtual Vector3 GetRandomPointInRange()
    {
        Vector3 randomPoint = GenerateRandomPoint(terrain.terrainData.size.x, terrain.terrainData.size.z);
        return randomPoint;
    }

    public virtual Vector3 GenerateRandomPoint(float x, float z)
    {
        float randomX = Random.Range(-x, x);
        float randomZ = Random.Range(-z, z);
        float y = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));

        return new Vector3(randomX, y, randomZ);
    }

    public bool IsPointOnTerrain(Vector3 point)
    {
        return terrainCollider.bounds.Contains(point);
    }

    public void FindTerrain()
    {
        terrain = FindObjectOfType<Terrain>();
    }
}

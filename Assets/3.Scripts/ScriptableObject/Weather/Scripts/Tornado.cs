using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour, IFindTerrain
{
    private Terrain terrain;

    private int numberOfPoints = 10;
    private float areaRadius = 40f;
    private float moveSpeed = 0.05f;

    private List<Vector3> controlPoints = new List<Vector3>();
    private float t = 0;
    private int currentSegment = 0;

    private void Start()
    {
        FindTerrain();
        GenerateRandomPoints();
    }

    private void Update()
    {
        if (controlPoints.Count < 4) return;

        Vector3 p0 = controlPoints[currentSegment];
        Vector3 p1 = controlPoints[(currentSegment + 1) % controlPoints.Count];
        Vector3 p2 = controlPoints[(currentSegment + 2) % controlPoints.Count];
        Vector3 p3 = controlPoints[(currentSegment + 3) % controlPoints.Count];

        Vector3 pos = BezierCurve.GetPoint(p0, p1, p2, p3, t);
        transform.position += pos;

        t += Time.deltaTime * moveSpeed;

        if (t > 1f)
        {
            t = 0f;
            currentSegment++;

            if (currentSegment >= controlPoints.Count - 3)
            {
                currentSegment = 0;
                GenerateRandomPoints();
            }
        }
    }

    private void GenerateRandomPoints()
    {
        controlPoints.Clear();
        for (int i = 0; i < numberOfPoints; i++)
        {
            float randomX = Random.Range(-areaRadius, areaRadius);
            float randomZ = Random.Range(-areaRadius, areaRadius);

            float y = terrain.SampleHeight(new Vector3(randomX, 0, randomZ)) + terrain.transform.position.y;

            Vector3 randomPoint = new Vector3(randomX, y, randomZ);
            controlPoints.Add(randomPoint);
        }
    }

    public void FindTerrain()
    {
        terrain = FindObjectOfType<Terrain>();
    }
}

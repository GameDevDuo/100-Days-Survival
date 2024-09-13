using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : RandomPosBase
{
    private int numberOfPoints = 10;
    private float areaRadius = 40f;
    private float moveSpeed = 1f;

    private List<Vector3> controlPoints = new List<Vector3>();
    private Vector3 lastPos;
    private float t = 0;
    private int currentSegment = 0;

    private void Start()
    {
        FindTerrain();
        terrainCollider = terrain.GetComponent<Collider>();
        GenerateRandomPoints();
        lastPos = transform.position;
    }

    private void Update()
    {
        if (controlPoints.Count < 4) return;

        Vector3 p0 = controlPoints[currentSegment];
        Vector3 p1 = controlPoints[(currentSegment + 1) % controlPoints.Count];
        Vector3 p2 = controlPoints[(currentSegment + 2) % controlPoints.Count];
        Vector3 p3 = controlPoints[(currentSegment + 3) % controlPoints.Count];

        Vector3 pos = BezierCurve.GetPoint(p0, p1, p2, p3, t);
        transform.position = Vector3.Lerp(lastPos, pos, Time.deltaTime * moveSpeed);

        t += Time.deltaTime * moveSpeed;

        if (t > 1f)
        {
            t = 0f;
            currentSegment++;

            if (currentSegment >= controlPoints.Count - 3)
            {
                currentSegment = 0;
                lastPos = transform.position;
                GenerateRandomPoints();
            }
        }
    }

    private void GenerateRandomPoints()
    {
        controlPoints.Clear();
        for (int i = 0; i < numberOfPoints; i++)
        {
            Vector3 randomPoint = GetRandomPointInRange();
            controlPoints.Add(randomPoint);
        }
    }

    public override Vector3 GetRandomPointInRange()
    {
        Vector3 randomPoint;
        do
        {
            randomPoint = GenerateRandomPoint(lastPos.x, lastPos.z);
        } while (!IsPointOnTerrain(randomPoint));

        return randomPoint;
    }

    public override Vector3 GenerateRandomPoint(float x, float z)
    {
        float randomX = Random.Range(x - areaRadius, x + areaRadius);
        float randomZ = Random.Range(z - areaRadius, z + areaRadius);
        float y = terrain.SampleHeight(new Vector3(randomX, 0, randomZ)) + terrain.transform.position.y;
        return new Vector3(randomX, y, randomZ);
    }
}

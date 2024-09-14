using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : RandomPosBase
{
    private int numberOfPoints = 10;
    private float moveSpeed = 0.5f;

    private List<Vector3> controlPoints = new List<Vector3>();
    private float t = 0;
    private int currentSegment = 0;

    private void Start()
    {
        FindTerrain();
        terrainCollider = terrain.GetComponent<Collider>();
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
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * moveSpeed);

        t += Time.deltaTime * moveSpeed * 0.1f;

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
        controlPoints.Add(transform.position);
        for (int i = 0; i < numberOfPoints - 1; i++)
        {
            Vector3 randomPoint = GetRandomPointInRange();
            controlPoints.Add(randomPoint);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    private int numberOfPoints = 10;
    private float areaRadius = 40f;
    private float moveSpeed = 5f;

    private List<Vector3> controlPoints = new List<Vector3>();
    private float t = 0;
    private int currentSegment = 0;

    private void Start()
    {
        GenerateRandomPoints();
    }

    private void Update()
    {
        if (controlPoints.Count < 10) return;
        
        Vector3 p0 = controlPoints[currentSegment];
        Vector3 p1 = controlPoints[currentSegment + 1];
        Vector3 p2 = controlPoints[currentSegment + 2];
        Vector3 p3 = controlPoints[currentSegment + 3];

        Vector3 pos = BezierCurve.GetPoint(p0, p1, p2, p3, t);
        transform.position = pos;

        t += Time.deltaTime * moveSpeed;

        if(t > 1f)
        {
            t = 0f;
            currentSegment++;

            if(currentSegment >= controlPoints.Count - 3)
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
            Vector3 randomPoint = new Vector3(
                Random.Range(-areaRadius, areaRadius),
                0,
                Random.Range(-areaRadius, areaRadius)
            );
            controlPoints.Add(randomPoint);
        }
    }
}

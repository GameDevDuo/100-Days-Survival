using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    private int numberOfPoints = 4;
    private float areaRadius = 40f;
    private float moveSpeed = 5f;

    private List<Vector3> controlPoints = new List<Vector3>();
    private float t = 0;
    private int currentSegment = 0;

    private void Start()
    {
        GenerateRandomPoints();
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

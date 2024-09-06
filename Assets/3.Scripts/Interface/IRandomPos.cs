using UnityEngine;

public interface IRandomPos
{
    public Vector3 GetRandomPointInRange();
    public Vector3 GenerateRandomPoint(float x, float z);
}

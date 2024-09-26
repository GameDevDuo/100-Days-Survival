using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool CheckForObject(out Collider[] colliders, Vector3 pos, float radius, LayerMask layer)
    {
        colliders = Physics.OverlapSphere(pos, radius, layer);
        if(colliders.Length > 0)
        {
            return true;
        }
        return false;
    }
}
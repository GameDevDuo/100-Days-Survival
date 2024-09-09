using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : RandomPosBase
{
    [SerializeField] 
    private List<GameObject> meteor = new List<GameObject>();

    [SerializeField] Transform spawnPos;

    private Vector3 target;

    private float smallAngle = 60f;
    private float largeAngle = 120f;
    private float time;

    private int maxCount;

    private void Awake()
    {
        FindTerrain();
    }

    private void Start()
    {
        maxCount = Random.Range(20, 40);
        StartCoroutine(SpawnMeteorsWithCooldown());
    }

    private IEnumerator SpawnMeteorsWithCooldown()
    {
        for (int i = 0; i < maxCount; i++)
        {
            SpawnMeteor();
            yield return new WaitForSeconds(Random.Range(5f, 20f));
        }
    }

    private void SpawnMeteor()
    {
        target = GetRandomPointInRange();
        GameObject selectedMeteor = meteor[Random.Range(0, meteor.Count)];

        GameObject meteorInstance = Instantiate(selectedMeteor, spawnPos.position, Quaternion.identity);
        Rigidbody rb = meteorInstance.GetComponent<Rigidbody>();

        float randomAngle = Random.Range(smallAngle, largeAngle);
        Vector3 velocity = GetVelocity(spawnPos.position, target, randomAngle);

        rb.velocity = velocity;
    }

    private Vector3 GetVelocity(Vector3 start, Vector3 target, float initialAngle)
    {
        float gravity = Physics.gravity.magnitude;
        float angle = initialAngle * Mathf.Deg2Rad;

        Vector3 planarTarget = new Vector3(target.x, 0f, target.z);
        Vector3 planarPos = new Vector3(start.x, 0f, start.z);

        float distance = Vector3.Distance(planarTarget, planarPos);
        float yOffset = start.y - target.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) *
            Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) /
            (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0f,
            initialVelocity * Mathf.Sin(angle),
            initialVelocity * Mathf.Cos(angle));

        float angleBetweenObject = Vector3.Angle(Vector3.forward,
            planarTarget - planarPos) * (target.x > start.x ? 1 : -1);

        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObject,
            Vector3.up) * velocity;

        return finalVelocity;
    }
}

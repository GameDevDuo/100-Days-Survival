using UnityEngine;

public class Animal : AnimalBase
{
    [SerializeField]
    private AnimalData animalData;

    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Collider animalCollider;

    [SerializeField]
    private Terrain terrain;
    [SerializeField]
    private Transform centerPoint;
    

    private Vector3 targetPosition;

    private float moveSpeed = 3f;

    private float rangeRadius = 10f;

    private float moveTime = 5f;
    private float waitTime = 2f;
    private float currentTime;

    private bool isAction;
    private bool isWalking;

    private void Start() => Init();

    private void Update() => Move();

    public override void Move()
    {
        if (isWalking)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);

            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                isWalking = false;
                currentTime = waitTime;
            }
        }
        else
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                targetPosition = GetRandomPointInRange();
                isWalking = true;
                currentTime = moveTime;
            }
        }
    }

    private void Init()
    {
        rb = GetComponent<Rigidbody>();
        animalCollider = GetComponent<Collider>();

        currentTime = waitTime;
        isAction = true;
    }

    private Vector3 GetRandomPointInRange()
    {
        Vector3 randomPoint = Vector3.zero;
        bool isPointOnTerrain = false;

        while (!isPointOnTerrain)
        {
            float randomX = Random.Range(centerPoint.position.x - rangeRadius, centerPoint.position.x + rangeRadius);
            float randomZ = Random.Range(centerPoint.position.z - rangeRadius, centerPoint.position.z + rangeRadius);

            float y = terrain.SampleHeight(new Vector3(randomX, 0, randomZ)) + terrain.transform.position.y;

            randomPoint = new Vector3(randomX, y, randomZ);

            if (terrain.GetComponent<Collider>().bounds.Contains(randomPoint))
            {
                isPointOnTerrain = true;
            }
        }

        return randomPoint;
    }
}

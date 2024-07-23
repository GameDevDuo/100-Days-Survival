using UnityEngine;
using UnityEngine.AI;

public class Animal : AnimalBase
{
    [SerializeField]
    private AnimalData animalData;

    private NavMeshAgent agent;
    private Collider animalCollider;

    [SerializeField]
    private Terrain terrain;
    [SerializeField]
    private Transform centerPoint;
    
    private Vector3 targetPosition;

    private float rangeRadius = 10f;

    private void Start() => Init();

    protected override void Update()
    {
        base.Update();
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

    private void Init()
    {
        agent = GetComponent<NavMeshAgent>();
        animalCollider = GetComponent<Collider>();

        ChangeState(State.Idle, RandomTime(2.5f));
    }

    public override void Idle()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            targetPosition = GetRandomPointInRange();
            ChangeState(State.Move, RandomTime(5f));
        }
    }

    public override void Move()
    {
        agent.SetDestination(targetPosition);

        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            ChangeState(State.Idle, RandomTime(2.5f));
        }
    }

    public override void Attak()
    {
        throw new System.NotImplementedException();
    }

    public override void Dead()
    {
        throw new System.NotImplementedException();
    }
}

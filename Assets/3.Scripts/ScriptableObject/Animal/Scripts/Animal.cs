using UnityEngine;
using UnityEngine.AI;

public class Animal : AnimalBase
{
    private const float rangeRadius = 10f;
    private const float IdleStateDuration = 2.5f;
    private const float MoveStateDuration = 5f;

    [SerializeField]
    private AnimalData animalData;

    private NavMeshAgent agent;
    private Collider animalCollider;
    private Collider terrainCollider;

    [SerializeField]
    private Terrain terrain;
    [SerializeField]
    private Transform centerPoint;
    
    private Vector3 targetPosition;

    private void Start() => Init();

    protected override void Update()
    {
        base.Update();
    }
    private Vector3 GetRandomPointInRange()
    {
        Vector3 randomPoint;
        do
        {
            randomPoint = GenerateRandomPoint();
        } while (!IsPointOnTerrain(randomPoint));

        return randomPoint;
    }

    private Vector3 GenerateRandomPoint()
    {
        float randomX = Random.Range(centerPoint.position.x - rangeRadius, centerPoint.position.x + rangeRadius);
        float randomZ = Random.Range(centerPoint.position.z - rangeRadius, centerPoint.position.z + rangeRadius);
        float y = terrain.SampleHeight(new Vector3(randomX, 0, randomZ)) + terrain.transform.position.y;
        return new Vector3(randomX, y, randomZ);
    }

    private bool IsPointOnTerrain(Vector3 point)
    {
        return terrain.GetComponent<Collider>().bounds.Contains(point);
    }


    private void Init()
    {
        agent = GetComponent<NavMeshAgent>();
        animalCollider = GetComponent<Collider>();
        terrainCollider = terrain.GetComponent<Collider>();

        ChangeState(State.Idle, RandomTime(IdleStateDuration));
    }

    public override void Idle()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            targetPosition = GetRandomPointInRange();
            ChangeState(State.Move, RandomTime(MoveStateDuration));
        }
    }

    public override void Move()
    {
        agent.SetDestination(targetPosition);

        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            ChangeState(State.Idle, RandomTime(IdleStateDuration));
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

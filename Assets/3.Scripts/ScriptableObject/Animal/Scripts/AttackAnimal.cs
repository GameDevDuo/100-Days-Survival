using UnityEngine;
using UnityEngine.AI;

public class AttackAnimal : AnimalBase
{
    private const float rangeRadius = 10f;
    private const float IdleStateDuration = 2.5f;
    private const float MoveStateDuration = 5f;

    [SerializeField] private AnimalData animalData;
    [SerializeField] private Terrain terrain;
    [SerializeField] private Transform centerPoint;

    private Animator animator;
    private NavMeshAgent agent;
    private Collider animalCollider;
    private Collider terrainCollider;

    private Vector3 targetPosition;

    private float hp;

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
        return terrainCollider.bounds.Contains(point);
    }

    private void Init()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        animalCollider = GetComponent<Collider>();
        terrainCollider = terrain.GetComponent<Collider>();

        hp = animalData.MaxHP;

        ChangeState(State.Idle, RandomTime(IdleStateDuration));
    }

    private bool CheckForPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(centerPoint.position, animalData.FindRange, LayerMask.GetMask("Player"));
        if(colliders.Length > 0)
        {
            Vector3 playerPos = colliders[0].transform.position;
            targetPosition = new Vector3(playerPos.x - animalData.AttakDistance, playerPos.y, playerPos.z - animalData.AttakDistance);
        }
        return colliders.Length > 0;
    }

    public override void Idle()
    {
        if(hp <= 0)
        {
            ChangeState(State.Dead);
        }
        else
        {
            if(CheckForPlayer())
            {
                ChangeState(State.Attak);
            }
            else
            {
                currentTime -= Time.deltaTime;
                if (currentTime <= 0)
                {
                    targetPosition = GetRandomPointInRange();
                    animator.Play("walk");
                    ChangeState(State.Move, RandomTime(MoveStateDuration));
                }
            }
        }
    }

    public override void Move()
    {
        if(hp <= 0)
        {
            ChangeState(State.Dead);
        }
        else
        {
            if (CheckForPlayer())
            {
                ChangeState(State.Attak);
            }
            else
            {
                agent.SetDestination(targetPosition);

                currentTime -= Time.deltaTime;
                if (currentTime <= 0)
                {
                    animator.Play("idle");
                    ChangeState(State.Idle, RandomTime(IdleStateDuration));
                }
            }
        }
    }

    public override void Attak()
    {
        agent.SetDestination(targetPosition);
        if(targetPosition == transform.position)
        {
            animator.Play("attack");
        }
    }

    public override void Dead()
    {
        animator.Play("die");
    }
}
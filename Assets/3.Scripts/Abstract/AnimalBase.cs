using UnityEngine;
using UnityEngine.AI;

public enum State
{    
    Idle,
    Move,
    Attak,
    Dead,
}

public abstract class AnimalBase : RandomPosBase, IMove, IFindTerrain, IFindWater
{
    private State currentState;

    [SerializeField] protected AnimalData animalData;

    protected GameObject waterObj;
    protected Rigidbody rb;
    protected Transform centerPoint;
    protected Animator animator;
    protected NavMeshAgent agent;
    protected Collider terrainCollider;

    protected Vector3 targetPosition;

    protected const float rangeRadius = 10f;
    protected const float IdleStateDuration = 2.5f;
    protected const float MoveStateDuration = 5f;

    protected float currentTime;
    protected float minTime = 1.5f;
    protected float hp;

    public virtual void Start()
    {
        centerPoint = this.transform;
        FindTerrain();
        FindWaterPlane();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        terrainCollider = terrain.GetComponent<Collider>();

        hp = animalData.MaxHP;

        ChangeState(State.Idle, RandomTime(IdleStateDuration));
    }

    protected virtual void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Move:
                Move();
                break;
            case State.Attak:
                Attack();
                break;
            case State.Dead:
                Dead();
                break;
        }
    }

    public abstract void Idle();
    public abstract void Move();
    public virtual void Attack()
    {
        //attack Logjc
    }
    public virtual void Dead()
    {
        animator.Play("die");
        agent.isStopped = true;
        RigidFreezeHandler(ref rb, RigidbodyConstraints.FreezeAll);
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
    }

    public override Vector3 GetRandomPointInRange()
    {
        Vector3 randomPoint;
        do
        {
            randomPoint = GenerateRandomPoint(centerPoint.position.x, centerPoint.position.z);
        } while (!IsPointOnTerrain(randomPoint));

        return randomPoint;
    }

    public override Vector3 GenerateRandomPoint(float x, float z)
    {
        float randomX = Random.Range(x - rangeRadius, x + rangeRadius);
        float randomZ = Random.Range(z - rangeRadius, z + rangeRadius);
        float y = terrain.SampleHeight(new Vector3(randomX, 0, randomZ)) + terrain.transform.position.y;
        if (y < waterObj.transform.position.y)
        {
            GenerateRandomPoint(centerPoint.position.x, centerPoint.position.z);
        }
        return new Vector3(randomX, y, randomZ);
    }

    private bool IsPointOnTerrain(Vector3 point)
    {
        return terrainCollider.bounds.Contains(point);
    }

    public bool IsNearDistination(NavMeshAgent agent)
    {
        if (!agent.pathPending)
        {
            if(agent.remainingDistance <= 1.5f)
            {
                return true;
            }
        }
        return false;
    }

    public void RigidFreezeHandler(ref Rigidbody rb, RigidbodyConstraints constraints)
    {
        rb.constraints = constraints;
    }

    public void ChangeState(State newState, float time)
    {
        currentState = newState;
    }

    public void ChangeState(State newState)
    {
        currentState = newState;
    }

    public float RandomTime(float maxTime)
    {
        return currentTime = Random.Range(minTime, maxTime);
    }

    public void FindWaterPlane()
    {
        waterObj = GameObject.Find("WaterPlane");
    }
}

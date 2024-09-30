using UnityEngine;
using UnityEngine.AI;

public enum State
{    
    Idle,
    Move,
    Attak,
    Dead,
}

public abstract class AnimalBase : RandomPosBase, IMove, IFindWater
{
    private State currentState;

    [SerializeField] protected AnimalData animalData;

    protected GameObject playerObj;

    protected GameObject waterObj;
    protected Rigidbody rb;
    protected Transform centerPoint;
    protected Animator animator;
    protected NavMeshAgent agent;

    protected Vector3 targetPosition;

    protected const float rangeRadius = 10f;
    protected const float IdleStateDuration = 2.5f;
    protected const float MoveStateDuration = 5f;

    protected float currentTime;
    protected float minTime = 1.5f;
    protected float hp;

    protected float walkSpeed;
    protected float runSpeed;

    public virtual void Start()
    {
        centerPoint = this.transform;
        FindTerrain();
        FindWaterPlane();
        playerObj = GameObject.Find("Player").gameObject;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        terrainCollider = terrain.GetComponent<Collider>();

        if (agent && NavMesh.SamplePosition(agent.transform.position, out NavMeshHit hit, 10.0f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
        hp = animalData.MaxHP;

        walkSpeed = agent.speed;
        runSpeed = agent.speed * 2;

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
        GetComponent<ResourceItem>().enabled = true;
        gameObject.layer = LayerMask.NameToLayer("ResourceItem");
    }

    protected bool CheckForPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(centerPoint.position, animalData.FindRange, LayerMask.GetMask("Player"));
        if (colliders.Length > 0)
        {
            playerObj = colliders[0].gameObject;
            return true;
        }
        playerObj = null;
        return false;
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
        Vector3 randomPoint = Vector3.zero;
        int maxAttempts = 100;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            float randomX = Random.Range(x - rangeRadius, x + rangeRadius);
            float randomZ = Random.Range(z - rangeRadius, z + rangeRadius);
            float y = terrain.SampleHeight(new Vector3(randomX, 0, randomZ)) + terrain.transform.position.y;

            if (y >= waterObj.transform.position.y)
            {
                randomPoint = new Vector3(randomX, y, randomZ);
                break;
            }
            attempts++;
        }
        if (attempts >= maxAttempts)
        {
            randomPoint = new Vector3(x, terrain.SampleHeight(new Vector3(x, 0, z)) + terrain.transform.position.y, z);
        }
        return randomPoint;
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

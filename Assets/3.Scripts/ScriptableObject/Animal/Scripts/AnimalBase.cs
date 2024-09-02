using UnityEngine;
using UnityEngine.AI;

public enum State
{    
    Idle,
    Move,
    Attak,
    Dead,
}

public abstract class AnimalBase : MonoBehaviour, IMove
{
    private State currentState;

    protected Terrain terrain;

    [SerializeField] protected AnimalData animalData;

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
    public abstract void Dead();

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

    protected void FindTerrain()
    {
        terrain = FindObjectOfType<Terrain>();
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
}

using UnityEngine;
using UnityEngine.AI;

public class AttackAnimal : AnimalBase
{
    private const float rangeRadius = 10f;
    private const float IdleStateDuration = 2.5f;
    private const float MoveStateDuration = 7f;

    [SerializeField] private AnimalData animalData;

    private Transform centerPoint;
    private GameObject player;
    private Animator animator;
    private NavMeshAgent agent;
    private Rigidbody rb;
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
        centerPoint = this.transform;
        FindTerrain();
        rb = GetComponent<Rigidbody>();
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
        if (colliders.Length > 0)
        {
            player = colliders[0].gameObject;
            return true;
        }
        player = null;
        return false;
    }

    private void TakeDamage(int damage)
    {
        hp -= damage;
    }

    public override void Idle()
    {
        if (hp <= 0)
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
                currentTime -= Time.deltaTime;

                animator.enabled = false;
                if (currentTime <= 0)
                {
                    targetPosition = GetRandomPointInRange();
                    ChangeState(State.Move, RandomTime(MoveStateDuration));
                }
                else
                {
                    RigidFreezeHandler(ref rb, RigidbodyConstraints.FreezeAll);
                    animator.enabled = true;
                    animator.Play("idle");
                }
            }
        }
    }

    public override void Move()
    {
        if (hp <= 0)
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

                animator.enabled = false;
                if (currentTime <= 0 && IsNearDistination(agent))
                {
                    ChangeState(State.Idle, RandomTime(IdleStateDuration));
                }
                else
                {
                    RigidFreezeHandler(ref rb, RigidbodyConstraints.None);
                    animator.enabled = true;
                    animator.Play("walk");
                }
            }
        }
    }

    public override void Attack()
    {
        if (hp <= 0)
        {
            ChangeState(State.Dead);
        }
        else
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            RigidFreezeHandler(ref rb, RigidbodyConstraints.None);
            animator.enabled = false;
            transform.LookAt(player.transform);

            if (distanceToPlayer <= animalData.AttackDistance)
            {
                agent.ResetPath();
                animator.enabled = true;
                animator.Play("attack");
            }
            else if (distanceToPlayer <= animalData.FindRange)
            {
                agent.SetDestination(player.transform.position);
                animator.enabled = true;
                animator.Play("run");
            }
            else
            {
                ChangeState(State.Idle, RandomTime(IdleStateDuration));
            }
        }
    }

    public override void Dead()
    {
        animator.Play("die");
        agent.isStopped = true;
    }
}

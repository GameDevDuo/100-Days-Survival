using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : AnimalBase
{
    private const float rangeRadius = 10f;
    private const float IdleStateDuration = 2.5f;
    private const float MoveStateDuration = 5f;

    [SerializeField] private AnimalData animalData;
    [SerializeField] private Terrain terrain;

    private Transform centerPoint;
    private Animator animator;
    private NavMeshAgent agent;
    private Collider terrainCollider;

    private Vector3 targetPosition;
    private float hp;
    private string animationState;

    private bool isAttack = false;

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
        centerPoint = transform;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        terrainCollider = terrain.GetComponent<Collider>();
        hp = animalData.MaxHP;

        animationState = "idle";

        ChangeState(State.Idle, RandomTime(IdleStateDuration));
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            ChangeState(State.Dead);
        }
    }

    public override void Idle()
    {
        if (hp <= 0)
        {
            ChangeState(State.Dead);
        }
        else
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                targetPosition = GetRandomPointInRange();
                ChangeState(State.Move, RandomTime(MoveStateDuration));
            }

            if (animationState != "idle")
            {
                animator.Play("idle");
                animationState = "idle";
            }
        }
    }

    public override void Move()
    {
        if (hp <= 0)
        {
            ChangeState(State.Dead);
        }
        else if (!isAttack)
        {
            agent.SetDestination(targetPosition);

            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                ChangeState(State.Idle, RandomTime(IdleStateDuration));
            }
            else if (animationState != "walk")
            {
                animator.Play("walk");
                animationState = "walk";
            }
        }
    }

    public override void Attack()
    {
        isAttack = true;
        agent.isStopped = true;

        if (animationState != "attack")
        {
            animator.Play("attack");
            animationState = "attack";
        }
        isAttack = false;
        ChangeState(State.Idle, RandomTime(IdleStateDuration));
    }

    public override void Dead()
    {
        if (animationState != "die")
        {
            animator.Play("die");
            animationState = "die";
        }
        agent.isStopped = true;
    }

    public void StartAttack()
    {
        if (hp > 0)
        {
            ChangeState(State.Attak);
        }
    }
}
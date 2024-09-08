using UnityEngine;
using UnityEngine.AI;

public class AttackAnimal : AnimalBase
{
    private GameObject player;
    private Collider animalCollider;

    public override void Start()
    {
        base.Start();
        animalCollider = GetComponent<Collider>();
    }
    protected override void Update()
    {
        base.Update();
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
        RigidFreezeHandler(ref rb, RigidbodyConstraints.FreezeAll);
    }
}

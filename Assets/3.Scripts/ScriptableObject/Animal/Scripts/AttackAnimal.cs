using UnityEngine;
using UnityEngine.AI;

public class AttackAnimal : AnimalBase
{
    private Player player;

    private bool isAttack;

    public override void Start()
    {
        base.Start();
        player = playerObj.GetComponent<Player>();
    }
    protected override void Update()
    {
        base.Update();
    }

    public override void Idle()
    {
        if (agent.isOnNavMesh)
        {
            if (hp <= 0)
            {
                ChangeState(State.Dead);
            }
            else
            {
                if (CheckForPlayer())
                {
                    ChangeState(State.Attak, animalData.AttackCoolTime);
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
                ChangeState(State.Attak, animalData.AttackCoolTime);
            }
            else
            {
                agent.speed = walkSpeed;
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

                currentTime -= Time.deltaTime;

                if (currentTime > 0f)
                {
                    animator.Play("idle");
                    Debug.Log("쿨타임!");
                }
                else if(!isAttack)
                {
                    animator.Play("attack");
                    isAttack = true;
                    player.TakeDamage(animalData.Damage);

                    currentTime = animalData.AttackCoolTime;
                }
            }
            else if (distanceToPlayer <= animalData.FindRange)
            {
                agent.speed = runSpeed;
                agent.SetDestination(player.transform.position);
                animator.enabled = true;
                animator.Play("run");
            }
            else
            {
                agent.ResetPath();
                targetPosition = GetRandomPointInRange();
                ChangeState(State.Move, RandomTime(MoveStateDuration));
            }

            if(animator.GetCurrentAnimatorStateInfo(0).IsName("attack") &&
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                isAttack = false;
            }
        }
    }

    public override void Dead()
    {
        base.Dead();
    }
}
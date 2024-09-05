using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PassiveAnimal : AnimalBase
{
    protected override void Update()
    {
        base.Update();
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

    public override void Move()
    {
        if (hp <= 0)
        {
            ChangeState(State.Dead);
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

    public override void Dead()
    {
        animator.Play("die");
        agent.isStopped = true;
    }
}


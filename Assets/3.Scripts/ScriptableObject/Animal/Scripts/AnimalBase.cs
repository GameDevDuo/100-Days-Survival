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

    protected float currentTime;
    protected float minTime = 1.5f;

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

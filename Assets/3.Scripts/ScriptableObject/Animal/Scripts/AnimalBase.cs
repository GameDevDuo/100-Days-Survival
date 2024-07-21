using UnityEngine;

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

    protected float currentTime;
    protected float minTime = 1.5f;

    protected virtual void Update()
    {
        switch (currentState) 
        { 
            case State.Idle:
                RandomTime(2.5f);
                Idle();
                break;
            case State.Move:
                RandomTime(5f);
                Move();
                break;
            case State.Attak:
                Attak();
                break;
            case State.Dead:
                Dead();
                break;
        }
    }

    public abstract void Idle();
    public abstract void Move();
    public abstract void Attak();
    public abstract void Dead();

    public void ChangeState(State newState)
    {
        currentState = newState;
    }

    public void RandomTime(float maxTime)
    {
        currentTime = Random.Range(minTime, maxTime);
    }
}

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
    protected State currentState;

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
}

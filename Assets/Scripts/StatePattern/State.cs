using UnityEngine;

public abstract class State<T>
{
    public T owner { get; private set; }

    public float timer;

    public virtual void Enter(T owner)
    {
        Debug.Log("Entering state: " + GetType().Name);
    }

    public virtual void Update(T owner) { }
    
    public virtual void Exit(T owner)
    {
        Debug.Log("Exiting state: " + GetType().Name);
    }
}
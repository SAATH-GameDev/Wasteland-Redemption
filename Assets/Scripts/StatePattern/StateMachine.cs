using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<T>
{
    public T owner;

    public State<T> CurrentState { get; private set; }

    public Dictionary<Type, State<T>> statesDictionary = new Dictionary<Type, State<T>>();

    public StateMachine(T owner)
    {
        this.owner = owner;
    }

    public void InitState(Type newState)
    {
        CurrentState = statesDictionary[newState];
        CurrentState.Enter(owner);
    }
    
    public void ChangeState(Type newState)
    {
        if (newState == CurrentState.GetType())
        {
            Debug.Log("Re-entering the same state not allowed");
            return;
        }
           
        
        if (CurrentState != null)
        {
            CurrentState.Exit(owner);
        }

        CurrentState = statesDictionary[newState];
        CurrentState.Enter(owner);
    }
    
    public void AddState(State<T> newState)
    {
        if(statesDictionary.ContainsKey(newState.GetType()))
            return;
        
        statesDictionary.Add(newState.GetType(), newState);
    }
    
    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update(owner);
        }
    }

    public void FixedUpdate()
    {
        if (CurrentState != null)
        {
            CurrentState.FixedUpdate(owner);
        }
    }
}
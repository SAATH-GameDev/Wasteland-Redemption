using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State currentState { get; private set; }

    private List<State> states = new List<State>();
    private List<Detector> detectors = new List<Detector>();

    public State GetDefault()
    {
        return states[states.Count - 1];
    }
    
    private void SetCurrent(State state = null)
    {
        if(!state)
            state = GetDefault();

        if(state == currentState)
            return;

        currentState.Exit();

        state.Enter();
        currentState = state;
    }

    private void Awake()
    {
        states.AddRange(GetComponents<State>());
        detectors.AddRange(GetComponents<Detector>());
        for(int i = 0; i < detectors.Count; i++)
        {
            states[i].detector = detectors[i];
            states[i].enabled = false;
        }
    }

    private void Start()
    {
        currentState = GetDefault();
        currentState.Enter();
    }
    
    private void Update()
    {
        bool detectionFound = false;
        foreach(State state in states)
        {
            if(state.detector != null && state.detector.detectedTarget != null)
            {
                SetCurrent(state);
                detectionFound = true;
                break;
            }
        }

        if(!detectionFound)
            SetCurrent();
    }
}
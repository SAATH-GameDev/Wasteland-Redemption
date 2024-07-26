using UnityEngine;

public class AIControllerOld : GameEntity
{
    public AIControllerBase currentState;
    
    void Update()
    {
        RunCurrentState();
    }

    private void RunCurrentState()
    {
        AIControllerBase nextState = currentState?.RunStateMeachine();
        if (nextState != null)
        {
            SwitchCurrentState(nextState);
        }
    }
    private void SwitchCurrentState(AIControllerBase nextState)
    {
        currentState = nextState;
    }
}

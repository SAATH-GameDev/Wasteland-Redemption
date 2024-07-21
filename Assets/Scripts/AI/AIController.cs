using UnityEngine;

public class AIController : GameEntity
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

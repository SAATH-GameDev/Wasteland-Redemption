using UnityEngine;

public class StateIdle : AIControllerState
{
    public override void Enter()
    {
        base.Enter();
        controller.StopAgent(true);
    }

    public override void Process()
    {
        base.Process();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
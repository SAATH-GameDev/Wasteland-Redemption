using UnityEngine;

public class StateChase : State
{
    private AIController controller;

    public override void Enter()
    {
        base.Enter();
        
        if(!controller)
        {
            controller = GetComponent<AIController>();
            if(!controller)
                controller = GetComponentInParent<AIController>();
        }
        
        controller.StopAgent(false);
    }

    public override void Process()
    {
        base.Process();
        
        //controller.HandleTargetTimer();
        controller.MoveTowardsTarget();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
using UnityEngine;

public class StateAttack : State
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
    }

    public override void Process()
    {
        base.Process();
        controller.RotateTowardsTarget();
        controller.StopAgent(true);
        controller.Attack();
        controller.HandleTargetTimer();
    }
    
    void FixedUpdate()
    {
        controller.RotateTowardsTarget();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
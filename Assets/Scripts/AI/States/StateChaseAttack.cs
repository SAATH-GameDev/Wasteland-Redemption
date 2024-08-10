using System;
using UnityEngine;

public class StateChaseAttack : AIControllerState
{
    public override void Enter()
    {
        base.Enter();
        controller.StopAgent(false);
    }

    public override void Process()
    {
        base.Process();
        
        controller.Attack();
     
        if (controller.TargetInRange(controller.attackRange + 1.25f))
        {
            controller.StopAgent(true);
            return;
        }
        
        controller.MoveTowardsTarget();
    }
    
    public override void Exit()
    {
        base.Exit();
    }
}
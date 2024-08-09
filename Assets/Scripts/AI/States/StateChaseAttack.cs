using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class StateChaseAttack : AIControllerState
{
    private float timer;

    public override void Enter()
    {
        base.Enter();
        timer = controller.attackTimer - Random.Range(0.2f, 0.5f);
        
        controller.StopAgent(false);
    }

    public override void Process()
    {
        base.Process();
        
        timer -= Time.deltaTime;
        
        if (timer <= 0.0f)
        {
            controller.Attack();
            timer = controller.attackTimer;
        }
     
        if (controller.TargetInRange(controller.attackRange + 1.25f))
        {
            controller.StopAgent(true);
            return;
        }
         
        controller.HandleTargetTimer();
        controller.MoveTowardsTarget();
    }
    
    public override void Exit()
    {
        base.Exit();
    }
}
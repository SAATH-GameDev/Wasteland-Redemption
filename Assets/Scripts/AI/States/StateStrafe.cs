using UnityEngine;

public class StateStrafe : AIControllerState
{
    int strafeDirection = 1;

    private float timer;
    private float attackTimer;
    
    public override void Enter()
    {
        base.Enter();
        timer = controller.strafeTimer;
        attackTimer = controller.attackTimer;
        
        strafeDirection = Random.Range(0, 2) == 0 ? -1 : 1;
        
        controller.StopAgent(false);
    }

    public override void Process()
    {
        base.Process();
        
        timer -= Time.deltaTime;
        
        if (timer <= 0)
        {
            strafeDirection *= -1;
            timer = controller.strafeTimer;
        }

        if (controller.TargetInRange(controller.strafeAttackRange))
        {
            attackTimer -= Time.deltaTime;
        
            if (attackTimer <= 0.0f)
            {
                controller.Attack();
                attackTimer = controller.attackTimer;
            }  
        }
        
        controller.StrafeAroundTarget(strafeDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
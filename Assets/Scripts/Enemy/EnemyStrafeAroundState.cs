using UnityEngine;

public class EnemyStrafeAroundState : State<AIController>
{
    int strafeDirection = 1;

    private float attackTimer;
    
    public override void Enter(AIController owner)
    {
        base.Enter(owner);
        timer = owner.strafeTimer;
        attackTimer = owner.attackTimer;
        
        strafeDirection = Random.Range(0, 2) == 0 ? -1 : 1;
    }

    public override void Update(AIController owner)
    {
        base.Update(owner);
        
        timer -= Time.deltaTime;
        
        if (timer <= 0)
        {
            strafeDirection *= -1;
            timer = owner.strafeTimer;
        }

        if (owner.TargetInRange(owner.attackRange))
        {
            owner.StateMachine.ChangeState(typeof(EnemyAttackState));
        } 
        else if (owner.TargetInRange(owner.strafeAttackRange))
        {
            attackTimer -= Time.deltaTime;
        
            if (attackTimer <= 0.0f)
            {
                owner.Attack();
                attackTimer = owner.attackTimer;
            }  
            
            owner.StopAgent(true);
            return;
        } 
        else if (owner.TargetOutOfRange(owner.strafeAttackRange))
        {
            owner.StateMachine.ChangeState(typeof(IdleState));
        }
        
        owner.StrafeAroundTarget(strafeDirection);
    }

    public override void Exit(AIController owner)
    {
        base.Exit(owner);
    }
}
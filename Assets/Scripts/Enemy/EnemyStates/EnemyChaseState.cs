using UnityEngine;

public class EnemyChaseState : State<AIController>
{
    public override void Enter(AIController owner)
    {
        base.Enter(owner);
        timer = owner.chaseTimer;
        
        owner.StopAgent(false);
    }

    public override void Update(AIController owner)
    {
        base.Update(owner);
        
        timer -= Time.deltaTime;
        
        if(timer <= 0)
        {
            owner.StateMachine.ChangeState(typeof(EnemyIdleState));
            return;
        }

        if (owner.TargetInRange(owner.attackRange))
        {
            owner.StateMachine.ChangeState(typeof(EnemyAttackState));
        }
        else if(owner.TargetOutOfRange(owner.chaseRange) && !owner.chaseAfterDamage)
        {
            owner.StateMachine.ChangeState(typeof(EnemyIdleState));
        }
        
        owner.HandleTargetTimer();
        
        owner.MoveTowardsTarget();
    }

    public override void FixedUpdate(AIController owner)
    {
        base.FixedUpdate(owner);
        
       
    }

    public override void Exit(AIController owner)
    {
        base.Exit(owner);
        
        owner.chaseAfterDamage = false;
    }
}
using UnityEngine;

public class EnemyChaseState : State<AIController>
{
    public override void Enter(AIController owner)
    {
        base.Enter(owner);
        timer = owner.chaseTime;            
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
            // todo change state to attack after implementing attack state
        }
        else if(owner.TargetOutOfRange(owner.chaseRange) && !owner.chaseAfterDamage)
        {
            owner.StateMachine.ChangeState(typeof(EnemyIdleState));
        }
        else
        {
          owner.MoveAndRotateTowardsTarget();
        }
    }

    public override void Exit(AIController owner)
    {
        base.Exit(owner);
        
        owner.chaseAfterDamage = false;
    }
}
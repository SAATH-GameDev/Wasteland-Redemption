using UnityEngine;

public class EnemyIdleState : State<AIController>
{
    public override void Enter(AIController owner)
    {
        base.Enter(owner);
    }

    public override void Update(AIController owner)
    {
        base.Update(owner);
        
        timer -= Time.deltaTime;
        
        if (timer <= 0)
        {
           // todo: Implement change state to patrolling if required later after timer runs out 
        }

        if (owner.TargetInRange(owner.chaseRange))
        {
            owner.StateMachine.ChangeState(typeof(EnemyChaseState));
        }
    }

    public override void Exit(AIController owner)
    {
        base.Exit(owner);
    }
}
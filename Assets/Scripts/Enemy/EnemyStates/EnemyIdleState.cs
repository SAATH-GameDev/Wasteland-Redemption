using UnityEngine;

public class EnemyIdleState : State<AIController>
{
    public override void Enter(AIController owner)
    {
        base.Enter(owner);

        owner.StopAgent(true);
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
           int randomStateIndex = Random.Range(0, 3);

           if (randomStateIndex == 0)
           {
               owner.StateMachine.ChangeState(typeof(EnemyChaseState));
           }
           else
           {
               Debug.Log("moving to chase attack combined state");
               owner.StateMachine.ChangeState(typeof(EnemyChaseAttackCombinedState));
           }
        }
    }

    public override void Exit(AIController owner)
    {
        base.Exit(owner);
    }
}
using UnityEngine;

public class EnemyAttackState : State<AIController>
{
    public override void Enter(AIController owner)
    {
        base.Enter(owner);

        timer = owner.attackTimer;
    }

    public override void Update(AIController owner)
    {
        base.Update(owner);
        
        if (owner.TargetInRange(owner.attackRange))
        {
            owner.RotateTowards();
            
            timer -= Time.deltaTime;
            
            if (timer <= 0.0f)
            {
                owner.Attack();
                timer = owner.attackTimer;
            }
        }
        else
        {
            owner.StateMachine.ChangeState(typeof(EnemyChaseState));
        }
        
    }

    public override void Exit(AIController owner)
    {
        base.Exit(owner);
    }
}
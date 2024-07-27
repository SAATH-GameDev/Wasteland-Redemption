using UnityEngine;

public class EnemyAttackState : State<AIController>
{
    public override void Enter(AIController owner)
    {
        base.Enter(owner);
        owner.inAttackState = true;
        timer = owner.attackTimer;
    }

    public override void Update(AIController owner)
    {
        base.Update(owner);
        
        if (owner.TargetInRange(owner.attackRange + 1.5f))
        {
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
        
        owner.HandleTargetTimer();
    }
    
    public override void FixedUpdate(AIController owner)
    {
        base.FixedUpdate(owner);
        owner.RotateTowards();
    }

    public override void Exit(AIController owner)
    {
        base.Exit(owner);
        
        owner.inAttackState = false;
    }
}
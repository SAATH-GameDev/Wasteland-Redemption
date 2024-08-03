using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyChaseAttackCombinedState : State<AIController>
{
    public override void Enter(AIController owner)
    {
        base.Enter(owner);
        owner.inAttackState = true;
        timer = owner.attackTimer - Random.Range(0.2f, 0.5f);
        
        owner.StopAgent(false);
    }

    public override void Update(AIController owner)
    {
        base.Update(owner);
        
        timer -= Time.deltaTime;
        
        if (timer <= 0.0f)
        {
            owner.Attack();
            timer = owner.attackTimer;
        }
        if (owner.TargetOutOfRange(owner.chaseRange))
        {
            owner.StateMachine.ChangeState(typeof(EnemyIdleState));
        }
     
        if (owner.TargetInRange(owner.attackRange + 1.25f))
        {
            owner.StopAgent(true);
            return;
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
        
        owner.inAttackState = false;
    }
}
public class EnemyChaseState : State<AIController>
{
    public override void Enter(AIController owner)
    {
        base.Enter(owner);
    }

    public override void Update(AIController owner)
    {
        base.Update(owner);

        if (owner.TargetInRange(owner.attackRange))
        {
            // todo change state to attack after implementing attack state
        }
        else if(owner.TargetOutOfRange(owner.chaseRange))
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
    }
}
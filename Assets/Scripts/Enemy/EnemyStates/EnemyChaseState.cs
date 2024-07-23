public class EnemyChaseState : State<AIController>
{
    public override void Enter(AIController owner)
    {
        base.Enter(owner);
    }

    public override void Update(AIController owner)
    {
        base.Update(owner);

        if (owner.PlayerInRange(owner.attackRange))
        {
            // todo change state to attack
        }
        else
        {
            owner.MoveTowards(owner.player);
        }
    }

    public override void Exit(AIController owner)
    {
        base.Exit(owner);
    }
}
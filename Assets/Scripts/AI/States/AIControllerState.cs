
public class AIControllerState : State
{
    protected AIController controller;

    public override void Enter()
    {
        base.Enter();

        if(!controller)
            controller = GetComponentInParent<AIController>();
    }
}
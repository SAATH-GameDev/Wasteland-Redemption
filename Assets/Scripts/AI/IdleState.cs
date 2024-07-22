using UnityEngine;

public class IdleState : AIControllerBase
{
    public bool canSee;
    public ChaseState chase;
    public override AIControllerBase RunStateMeachine()
    {
        if (canSee)
        {
            return chase;
        }
        else
        {
            return this;
        }
    }

  
}

using UnityEngine;

public class ChaseState : AIControllerBase
{
    public bool isAttack;
    public AttackState attackState;
    public override AIControllerBase RunStateMeachine()
    {
        if(isAttack)
        {
            return attackState;
        }
        else
        {
            return this;
        }
    }

    
}

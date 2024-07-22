using UnityEngine;
using UnityEngine.Rendering;

public class AttackState : AIControllerBase
{
    public override AIControllerBase RunStateMeachine()
    {
        Debug.Log("attack");
        return this;
    }
}

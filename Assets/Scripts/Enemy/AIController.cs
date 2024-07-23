using UnityEngine;

public class AIController : GameEntity
{
    public float idleTimer = 1f;

    public float chaseRange = 5f;
    public float attackRange = 3f;
    
    // todo: in coop multiple players target can be implemented here
    public Transform player;
    
    public StateMachine<AIController> StateMachine { get; private set; }

    private void Awake()
    {
        StateMachine = new EnemyStateMachine(this);
        StateMachine.AddState(new EnemyIdleState());
        StateMachine.InitState(typeof(EnemyIdleState));

        player = FindFirstObjectByType<PlayerController>().transform;
    }
    
    private void Update()
    {
        StateMachine?.Update();
    }
    
    public bool PlayerInRange(float range)
    {
        return DistanceToPlayer() <= range;
    }
    
    public float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }

    public void MoveTowards(Transform ownerPlayer)
    {
        Vector3 direction = (ownerPlayer.position - transform.position).normalized;
        transform.position += direction * Time.deltaTime;
        
        transform.LookAt(ownerPlayer);
    }
}
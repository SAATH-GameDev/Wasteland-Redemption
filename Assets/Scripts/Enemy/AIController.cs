using UnityEngine;

public class AIController : GameEntity
{
    // todo: in coop multiple players target can be implemented here
    public Transform currentTarget;

    public EnemyProfile enemyProfile;
    
    [Space]
    public float chaseRange = 5f;
    public float attackRange = 3f;

    [Space]
    public float idleTimer = 1f;
    public float chaseTimer = 5f;
    public float attackTimer = 2f;

    public float targetSearchInterval = 0.5f;

    
    [Space]
    public bool chaseAfterDamage;
    
    public StateMachine<AIController> StateMachine { get; private set; }
    
    private Vector3 _directionToTarget;

    private EnemyWeaponController _weaponController;
    private Rigidbody _rigidbody;
    
    private float targetSearchTimer;

    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _weaponController = GetComponentInChildren<EnemyWeaponController>();
        
        StateMachine = new EnemyStateMachine(this);
        
        StateMachine.AddState(new EnemyIdleState());
        StateMachine.AddState(new EnemyChaseState());    
        StateMachine.AddState(new EnemyAttackState());
        
        StateMachine.InitState(typeof(EnemyIdleState));
        

        currentTarget = FindFirstObjectByType<PlayerController>().transform;
        
        maxHealth = health = enemyProfile.health;
        targetSearchTimer = targetSearchInterval;
    }
    override protected void Update()
    {
        base.Update();
        StateMachine?.Update();
        
       
        _directionToTarget = (currentTarget.position - transform.position).normalized;
    }


    private void FixedUpdate()
    {
        StateMachine?.FixedUpdate();
    }
    
    public void MoveAndRotateTowardsTarget()
    {
        RotateTowards();
        MoveTowardsTarget();
    }

    
    public bool TargetInRange(float range)
    {
        return DistanceToTarget() <= range;
    }
    
    public bool TargetOutOfRange(float range)
    {
        return DistanceToTarget() > range;
    }
    
    public float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, currentTarget.position);
    }
    
    public void Attack()
    {
        _weaponController.HandleShooting();
    }
    
    
    public void RotateTowards()
    {
        if(Time.timeScale <= 0.0f) return;
        _rigidbody.MoveRotation( Quaternion.LookRotation(_directionToTarget, Vector3.up));
    }

    private void MoveTowardsTarget()
    {
        if(Time.timeScale <= 0.0f) return;
        
        Vector3 targetVelocity = _directionToTarget * (enemyProfile.speed);
        _rigidbody.MovePosition(_rigidbody.position + targetVelocity * Time.deltaTime);
    }
    
    public void ChaseTargetAfterShot(Transform target)
    {
        if(chaseAfterDamage) return;
        
        chaseAfterDamage = true;
        currentTarget = target;
        StateMachine.ChangeState(typeof(EnemyChaseState));
    }
    
    public void HandleTargetTimer()
    {
        if(chaseAfterDamage) return;
        
        targetSearchTimer -= Time.deltaTime;

        if(targetSearchTimer <= 0)
        {
            FindNearestTarget();
            targetSearchTimer = targetSearchInterval;
        }
    }
    
    private void FindNearestTarget()
    {
        var players = PlayerController.activePlayers;
        if(players.Count == 0) return;
        
        float minDistance = float.MaxValue;
        foreach (var player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(distance < minDistance)
            {
                minDistance = distance;
                currentTarget = player.transform;
            }
        }
    }

}


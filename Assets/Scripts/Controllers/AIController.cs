using UnityEngine;
using UnityEngine.AI;

public class AIController : GameEntity
{
    // todo: in coop multiple players target can be implemented here
    public Transform currentTarget;

    public EnemyProfile enemyProfile;
    
    [Space]
    public float chaseRange = 5f;
    public float attackRange = 3f;
    public float strafeAttackRange = 5f;

    [Space]
    public float idleTimer = 1f;
    public float chaseTimer = 5f;
    public float attackTimer = 2f;
    public float strafeTimer = 7f;

    public float targetSearchInterval = 0.5f;

    [Space]
    public bool chaseAfterDamage;
    public bool inAttackState;
    
    public StateMachine<AIController> StateMachine { get; private set; }
    
    private Vector3 _directionToTarget;

    private NavMeshAgent _navMeshAgent;
    private EnemyWeaponController _weaponController;
    private Rigidbody _rigidbody;
    private Animator _animator;

    private int isWalkingAnimParam = Animator.StringToHash("isWalking");
    
    private float targetSearchTimer;

    
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        _weaponController = GetComponentInChildren<EnemyWeaponController>();
        _animator = GetComponentInChildren<Animator>();
        
        StateMachine = new EnemyStateMachine(this);
        
        StateMachine.AddState(new EnemyIdleState());
        StateMachine.AddState(new EnemyChaseState());    
        StateMachine.AddState(new EnemyAttackState());
        StateMachine.AddState(new EnemyChaseAttackCombinedState());
        StateMachine.AddState(new EnemyStrafeAroundState());
        
        StateMachine.InitState(typeof(EnemyIdleState));

        currentTarget = FindFirstObjectByType<PlayerController>().transform;
        
        maxHealth = health = enemyProfile.health;
        targetSearchTimer = targetSearchInterval;

        _navMeshAgent.speed = enemyProfile.speed;
    }
    
    override protected void Update()
    {
        if(currentTarget == null) return;
        
        base.Update();
        StateMachine?.Update();

        _animator.SetBool(isWalkingAnimParam, _rigidbody.linearVelocity.sqrMagnitude > 0.25f);
       
        _directionToTarget = (currentTarget.position - transform.position).normalized;
    }

    private void FixedUpdate()
    {
        StateMachine?.FixedUpdate();
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
    
    public void RotateTowardsTarget()
    {
        if(Time.timeScale <= 0.0f) return;
        
        transform.rotation = Quaternion.LookRotation( _directionToTarget );
    }

    public void MoveTowardsTarget()
    {
        if(Time.timeScale <= 0.0f) return;
        
        // Vector3 targetVelocity = _directionToTarget * (enemyProfile.speed);
        // _rigidbody.MovePosition(_rigidbody.position + targetVelocity * Time.deltaTime);
        
        _navMeshAgent.SetDestination(currentTarget.position); 
    }
    
    public void ChaseTargetAfterShot(Transform target)
    {
        if(chaseAfterDamage || inAttackState) return;
        
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

    public void StopAgent(bool stop)
    {
        if (!_navMeshAgent.isOnNavMesh) return;
        _navMeshAgent.isStopped = stop;
        _navMeshAgent.velocity = Vector3.zero;
    }

    public void StrafeAroundTarget(int strafeDir)
    {
       Vector3 targetDir = currentTarget.position - transform.position;
       var rotation = Quaternion.Euler(0f, enemyProfile.speed * 4f * strafeDir * Time.deltaTime, 0f) * targetDir;
       
       _navMeshAgent.Move(rotation - targetDir);
       transform.rotation = Quaternion.LookRotation(rotation);
    }
}
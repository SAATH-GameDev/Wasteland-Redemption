using UnityEngine;
using UnityEngine.AI;

public class AIController : GameEntity
{
    public Transform currentTarget;

    public EnemyProfile profile;
    
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
    
    private Vector3 _directionToTarget;

    private StateMachine _stateMachine;
    private NavMeshAgent _navMeshAgent;
    private EnemyWeaponController _weaponController;
    private Rigidbody _rigidbody;
    private Animator _animator;
    
    private StatusEffectController statusEffectController = new StatusEffectController();

    private int walkX = Animator.StringToHash("walkX");
    private int walkY = Animator.StringToHash("walkY");
    
    private float targetSearchTimer;

    private Transform GetTarget()
    {
        return _stateMachine.currentState.GetTarget();
    }
    
    private void Awake()
    {
        _stateMachine = GetComponentInChildren<StateMachine>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        _weaponController = GetComponentInChildren<EnemyWeaponController>();
        _animator = GetComponentInChildren<Animator>();
        
        maxHealth = health = profile.health;
        targetSearchTimer = targetSearchInterval;

        statusEffectController.Setup();

        _navMeshAgent.speed = profile.speed + statusEffectController.modValues[(int)StatusEffectProfile.Attribute.SPEED];
    }
    
    override protected void Update()
    {
        currentTarget = GetTarget();

        if(!currentTarget)
            return;
        
        base.Update();

        _directionToTarget = (currentTarget.position - transform.position).normalized;

        _animator.SetFloat(walkX, Vector3.Dot(displayTransform.right, _rigidbody.linearVelocity));
        _animator.SetFloat(walkY, Vector3.Dot(displayTransform.forward, _rigidbody.linearVelocity));
        
        ProcessStatusEffects();
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

        Debug.Log("attacking target");
    }
    
    public void RotateTowardsTarget()
    {
        if(!currentTarget)
            return;
            
        if(Time.timeScale <= 0.0f)
            return;
        
        transform.rotation = Quaternion.LookRotation( _directionToTarget );
    }

    public void MoveTowardsTarget()
    {
        if(!currentTarget)
            return;

        if(Time.timeScale <= 0.0f)
            return;
        
        _navMeshAgent.SetDestination(currentTarget.position);
    }
    
    public void ChaseTargetAfterShot(Transform target)
    {
        if(chaseAfterDamage) return;
        
        chaseAfterDamage = true;
        currentTarget = target;
        //stateMachine.ChangeState(typeof(EnemyChaseState));
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
       var rotation = Quaternion.Euler(0f, (profile.speed + statusEffectController.modValues[(int)StatusEffectProfile.Attribute.SPEED]) * 4f * strafeDir * Time.deltaTime, 0f) * targetDir;
       
       _navMeshAgent.Move(rotation - targetDir);
       transform.rotation = Quaternion.LookRotation(rotation);
    }
    
    public void AddStatusEffect(StatusEffectProfile effect)
    {
        Debug.Log("Adding status effect: " + effect.name);
        statusEffectController.Add(effect);
    }
    
    private void ProcessStatusEffects()
    {
        //>>> Values to change
        statusEffectController.attributeValues[(int)StatusEffectProfile.Attribute.HEALTH] = health;

        //>>> Values to mod
        statusEffectController.attributeValues[(int)StatusEffectProfile.Attribute.SPEED] = profile.speed;
        
        statusEffectController.Update();

        //>>> Update the changed values
        //HEALTH
        int prevHealth = health;
        health = (int)statusEffectController.attributeValues[(int)StatusEffectProfile.Attribute.HEALTH];
        UpdateHealthBar();
        if(prevHealth > health)
            OnHealthDecrement();

        //SPEED
        _navMeshAgent.speed = profile.speed + statusEffectController.modValues[(int)StatusEffectProfile.Attribute.SPEED];
    }

    private void OnDrawGizmosSelected()
    {
        if(!_stateMachine || !_stateMachine.currentState)
            return;
        UnityEditor.Handles.color = Color.white;
        UnityEditor.Handles.Label( transform.position, "State: " + _stateMachine.currentState.GetType().Name );
    }
} 
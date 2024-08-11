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

    private float rotationSpeed = 4.0f;

    private int ammo = 9999999; //infinite

    private Transform GetTarget()
    {
        return _stateMachine.currentState.GetTarget();
    }
    
    private void Awake()
    {
        _stateMachine = GetComponentInChildren<StateMachine>();
        _navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        _weaponController = GetComponentInChildren<EnemyWeaponController>();
        _animator = GetComponentInChildren<Animator>();

        statusEffectController.Setup();

        _navMeshAgent.transform.parent = null;

        maxHealth = health = profile.health;
        _navMeshAgent.speed = profile.speed + statusEffectController.modValues[(int)StatusEffectProfile.Attribute.SPEED];
        _animator.speed = _navMeshAgent.speed * 0.32f;
    }
    
    override protected void Update()
    {
        currentTarget = GetTarget();

        if(!currentTarget)
            return;
        
        base.Update();

        _directionToTarget = (currentTarget.position - transform.position).normalized;

        Vector3 directionToNavAgent = (_navMeshAgent.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.Euler(0.0f, Quaternion.LookRotation(directionToNavAgent).eulerAngles.y, 0.0f);
        _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        Vector3 prevPosition = _rigidbody.position;
        _rigidbody.MovePosition(new Vector3(_navMeshAgent.transform.position.x, _rigidbody.position.y, _navMeshAgent.transform.position.z));
        Vector3 currentSpeed = (_rigidbody.position - prevPosition).normalized;

        _animator.SetFloat(walkX, Vector3.Dot(displayTransform.right, currentSpeed));
        _animator.SetFloat(walkY, Vector3.Dot(displayTransform.forward, currentSpeed));
        
        ProcessStatusEffects();
    }

    private void OnDestroy()
    {
        foreach(EnemyProfile.Loot loot in profile.lootDrops)
            if(Random.value < loot.chance)
                Instantiate(
                    loot.item,
                    transform.position
                        + (Vector3.up * 0.5f)
                        + (Vector3.forward * (Random.value - 0.5f))
                        + (Vector3.right * (Random.value - 0.5f)),
                    Quaternion.identity
                );
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
        _weaponController.HandleShooting(ref ammo);
    }
    
    public void RotateTowardsTarget()
    {
        if(!currentTarget)
            return;
            
        if(Time.timeScale <= 0.0f)
            return;
        
        transform.rotation = Quaternion.Euler(0.0f, Quaternion.LookRotation(_directionToTarget).eulerAngles.y, 0.0f);
    }

    public void MoveTowardsTarget()
    {
        if(!currentTarget)
            return;

        if(Time.timeScale <= 0.0f)
            return;
        
        _navMeshAgent.SetDestination(currentTarget.position);
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
        float prevHealth = health;
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
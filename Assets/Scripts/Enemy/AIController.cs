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

    public StateMachine<AIController> StateMachine { get; private set; }
    
    private Vector3 _directionToTarget;

    public bool chaseAfterContact;
    
    private Rigidbody _rigidbody;
    private DamageOnContact _damageOnContact;
    
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _damageOnContact = GetComponent<DamageOnContact>();
        
        StateMachine = new EnemyStateMachine(this);
        
        StateMachine.AddState(new EnemyIdleState());
        StateMachine.AddState(new EnemyChaseState());    
        
        StateMachine.InitState(typeof(EnemyIdleState));
        

        currentTarget = FindFirstObjectByType<PlayerController>().transform;
        
        maxHealth = health = enemyProfile.health;
    }

    private void OnEnable()
    {
        OnDamage.AddListener(OnContact);
    }

    private void OnDisable()
    {
        OnDamage.RemoveListener(OnContact);
    }

    override protected void Update()
    {
        base.Update();
        StateMachine?.Update();
    }
    
    public void MoveAndRotateTowardsTarget()
    {
        _directionToTarget = (currentTarget.position - transform.position).normalized;
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
    
    private void RotateTowards()
    {
        if(Time.timeScale <= 0.0f) return;
        
        displayTransform.LookAt(transform.position + _directionToTarget);
        displayTransform.rotation = Quaternion.Euler(0.0f, displayTransform.rotation.eulerAngles.y, 0.0f);
    }

    private void MoveTowardsTarget()
    {
        if(Time.timeScale <= 0.0f) return;
        
        Vector3 targetVelocity = _directionToTarget * (enemyProfile.speed);
        transform.position += targetVelocity * Time.deltaTime;
    }
    
    
    private void OnContact()
    {
       chaseAfterContact = true;
       Debug.Log("OnContact");
       StateMachine.ChangeState(typeof(EnemyChaseState));
    }

}


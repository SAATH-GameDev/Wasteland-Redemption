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
    private float targetSearchTimer;

    
    private void Awake()
    {
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
        
        targetSearchTimer -= Time.deltaTime;
        
        if(targetSearchTimer <= 0)
        {
            GetNearestTarget();
            targetSearchTimer = targetSearchInterval;
        }
        
        _directionToTarget = (currentTarget.position - transform.position).normalized;
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
        if(chaseAfterDamage) return;
        
        chaseAfterDamage = true;
        Debug.Log("OnContact");
        StateMachine.ChangeState(typeof(EnemyChaseState));
    }
    
    public void GetNearestTarget()
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


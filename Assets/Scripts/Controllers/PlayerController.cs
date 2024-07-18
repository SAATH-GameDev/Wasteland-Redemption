using UnityEngine;

partial class PlayerController : GameEntity
{
    [Header("Player")]
    public PlayerProfile profile;
    public PlayerCharacterProfile characterProfile;

    [Space]
    public WeaponController weapon;
    public GameObject capsuleMesh;

    [Space]
    public Transform movementTransform;
    public float smoothTime = 0.05f;

    private Rigidbody _rigidbody;
    private Vector3 _currentVelocity;

    static public int count = 0;

    void OnEnable()
    {
        count++;
    }

    void OnDisable()
    {
        count--;
    }

    override protected void Start()
    {
        base.Start();

        if (movementTransform == null)
            movementTransform = transform;

        _rigidbody = GetComponent<Rigidbody>();
        health = profile.health;
    }

    void Update()
    {
        displayTransform.LookAt(transform.position + _look);
        displayTransform.rotation = Quaternion.Euler(0.0f, displayTransform.rotation.eulerAngles.y, 0.0f);
    }

    void FixedUpdate()
    {
        Vector3 targetVelocity = _movement.normalized * (profile.speed * characterProfile.speed);
        _rigidbody.linearVelocity = Vector3.SmoothDamp(_rigidbody.linearVelocity, targetVelocity, ref _currentVelocity, smoothTime);
    }
}

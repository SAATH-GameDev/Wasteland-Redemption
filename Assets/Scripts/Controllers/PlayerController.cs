using UnityEngine;
using UnityEngine.InputSystem;

partial class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Player")]
    public PlayerProfile profile;
    public PlayerCharacterProfile characterProfile;

    [Space]
    [Header("Health")]
    public int maxHealth = 100;
    int currentHealth;
    public int damage = 10; //DEBUGGING FOR NOW...

    [Space]
    [Header("References")]
    public Transform displayTransform;
    public Transform movementTransform;

    private Rigidbody _rigidbody;

    static public int count = 0;

    void OnEnable()
    {
        count++;
    }

    void OnDisable()
    {
        count--;
    }

    void Start()
    {
        if (movementTransform == null)
            movementTransform = transform;

        _rigidbody = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        displayTransform.LookAt(transform.position + _look);
        displayTransform.rotation = Quaternion.Euler(0.0f, displayTransform.rotation.eulerAngles.y, 0.0f);

        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            TakeDamage(damage);
        }
    }

    void FixedUpdate()
    {
        _rigidbody.linearVelocity = _movement.normalized * (profile.speed * characterProfile.speed);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            print("Player has died");
        }
        else
        {
            Debug.Log($"Player took {damage} damage, current health: {currentHealth}");
        }
    }

}

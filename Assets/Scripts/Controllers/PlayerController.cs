using UnityEngine;

partial class PlayerController : MonoBehaviour
{
    public PlayerProfile profile;
    public PlayerCharacterProfile characterProfile;

    [Space]
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
        if(movementTransform == null)
            movementTransform = transform;

        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        displayTransform.LookAt(transform.position + _look);
        displayTransform.rotation = Quaternion.Euler(0.0f, displayTransform.rotation.eulerAngles.y, 0.0f);
    }

    void FixedUpdate()
    {
        _rigidbody.linearVelocity = _movement.normalized * (profile.speed * characterProfile.speed);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerProfile profile;
    public Transform movementTransform;

    private Rigidbody _rigidbody;
    private Vector3 _movement;

    void Start()
    {
        if(movementTransform == null)
            movementTransform = transform;

        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        _rigidbody.linearVelocity = _movement.normalized * profile.speed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 movementInput = context.ReadValue<Vector2>();
        _movement = (movementTransform.right * movementInput.x) + (movementTransform.forward * movementInput.y);
    }
}

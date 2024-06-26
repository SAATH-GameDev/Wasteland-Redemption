using UnityEngine;
using UnityEngine.InputSystem;

partial class PlayerController : MonoBehaviour
{
    private Vector3 _movement;
    private Vector3 _look;

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 movementInput = context.ReadValue<Vector2>();
        _movement = (movementTransform.right * movementInput.x) + (movementTransform.forward * movementInput.y);
    }

    public void Look(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();

        //Dealing with mouse input (position)
        if(lookInput.magnitude > 2.0f)
        {
            lookInput.x /= Screen.width;
            lookInput.y /= Screen.height;
            lookInput -= Vector2.one / 2.0f;
        }

        _look = (movementTransform.right * lookInput.x) + (movementTransform.forward * lookInput.y);
    }
}

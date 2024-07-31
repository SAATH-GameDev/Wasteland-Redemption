using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerController : GameEntity
{
    private Vector3 _movement;
    private Vector3 _look;

    public void Move(InputAction.CallbackContext context)
    {
        if(!movementTransform) return;

        Vector2 movementInput = context.ReadValue<Vector2>();
        _movement = (movementTransform.right * movementInput.x) + (movementTransform.forward * movementInput.y);
    }

    public void Look(InputAction.CallbackContext context)
    {
        if(!movementTransform) return;
        
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

    public void Attack(InputAction.CallbackContext context)
    {
        if(context.started)
            weapon.Attack(true);
        else if(context.canceled)
            weapon.Attack(false);
    }

    public void Reload(InputAction.CallbackContext context)
    {
        if(context.started)
            weapon.Reload();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(!context.started) return;

        if(DialogueManager.Instance.IsActive())
            DialogueManager.Instance.Proceed();
    }

    public void ToggleInventory(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            inventory.UI.SetActive(!inventory.UI.activeSelf);
            inventory.UI.transform.position = GameManager.Instance.WorldToScreenPosition(transform.position, index) + new Vector3(inventory.offset.x * Screen.width, inventory.offset.y * Screen.height, 0.0f);
        }
    }
}

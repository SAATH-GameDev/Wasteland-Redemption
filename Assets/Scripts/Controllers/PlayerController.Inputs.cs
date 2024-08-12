using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerController : GameEntity
{
    private Vector3 _movement;
    private Vector3 _look;

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 movementInput = context.ReadValue<Vector2>();

        if(!GameManager.Instance.ready)
        {
            if(context.performed)
            {
                if(movementInput.x != 0.0f)
                    GameManager.Instance.MoveSetupInX(GameManager.Instance.GetSetupOfIndex(index), movementInput.x < 0);
                else if(movementInput.y != 0.0f)
                    GameManager.Instance.MoveSetupInY(GameManager.Instance.GetSetupOfIndex(index), movementInput.y < 0);
            }
        }
        else if(inventory && inventory.UI && inventory.UI.gameObject.activeSelf)
        {
            _movement = Vector2.zero;
            
            if(context.performed)
                inventory.UpdateSelection(movementInput);
        }
        else if(movementTransform)
        {
            _movement = (movementTransform.right * movementInput.x) + (movementTransform.forward * movementInput.y);
        }
    }

    public void Look(InputAction.CallbackContext context)
    {
        if(!movementTransform)
            return;

        Vector2 lookInput = context.ReadValue<Vector2>();

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
        if(inventory && inventory.UI && inventory.UI.gameObject.activeSelf)
            inventory.DiscardSelected();

        if(context.started)
            weapon.Reload();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(!context.started)
            return;

        if(inventory && inventory.UI && inventory.UI.gameObject.activeSelf)
            inventory.UseSelected();

        if(DialogueManager.Instance.IsActive())
            DialogueManager.Instance.Proceed();
    }

    public void ToggleInventory(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(!GameManager.Instance.ready)
            {
                GameManager.Instance.ReadySetup(GameManager.Instance.GetSetupOfIndex(index));
                
            }
            else
            {
                inventory.UI.SetActive(!inventory.UI.activeSelf);
                inventory.UI.transform.position = GameManager.Instance.WorldToScreenPosition(transform.position, index) + new Vector3(inventory.offset.x * Screen.width, inventory.offset.y * Screen.height, 0.0f);
            }
        }
    }
}

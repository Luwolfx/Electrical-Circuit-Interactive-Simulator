using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName ="PlayerInputReader", menuName ="Player Input Reader")]
public class PlayerInputReader : ScriptableObject, InputSystem_Actions.IPlayerActions
{
    public Action<Vector2> onMoveEvent;
    public Action<Vector2> onLookEvent;
    public Action<bool> onInteractEvent;

    private InputSystem_Actions input;

    private void OnEnable()
    {
        if(input == null)
        {
            input = new InputSystem_Actions();
            input.Player.SetCallbacks(this);
        }
        input.Player.Enable();
    }

    private void OnDisable()
    {
        if(input != null) input.Player.Disable();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.performed) 
            onInteractEvent?.Invoke(true);
        
        if(context.canceled)
            onInteractEvent?.Invoke(false);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        onLookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        onMoveEvent?.Invoke(context.ReadValue<Vector2>());
    }
}

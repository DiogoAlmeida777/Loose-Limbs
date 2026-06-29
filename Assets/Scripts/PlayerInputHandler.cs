using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour, InputSystem.IPlayerActions
{

    public InputSystem InputSystem { get; private set; }
    public bool Jumped { get; private set; } = false; 
    public bool IsSprinting { get; private set; } = false;
    public Vector2 MoveInput {  get; private set; }
    public Vector2 LookInput { get; private set; }

    public bool IsLeftAttacking;

    public bool IsRightAttacking;

    public bool Interacted;

    public event Action Droped;

    //public event Action LeftAttack;
    //public event Action RightAttack;
    //public event Action Interacted;


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        InputSystem = new InputSystem();
        InputSystem.Player.Enable();
        InputSystem.Player.SetCallbacks(this);
    }

    private void OnDisable()
    {
        InputSystem.Player.Disable();
        InputSystem.Player.RemoveCallbacks(this);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Jumped = true;
        }
        else if (context.canceled)
        {
            Jumped = false;
        }
    }


    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsSprinting = true;
        }
        else if (context.canceled)
        {
            IsSprinting = false;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    public void OnLeftArmAttack(InputAction.CallbackContext context)
    {
        if (context.started)
            IsLeftAttacking = true;

        if (context.canceled)
            IsLeftAttacking = false;
    }


    public void OnRightArmAttack(InputAction.CallbackContext context)
    {
        if (context.started)
            IsRightAttacking = true;

        if (context.canceled)
            IsRightAttacking = false;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
            Interacted = true;

        if (context.canceled)
            Interacted = false;
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (context.started)
            Droped?.Invoke();
    }
}

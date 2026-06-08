using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour, InputSystem.IPlayerActions
{

    public InputSystem InputSystem { get; private set; }
    public bool Jumped { get; private set; } = false; 
    public bool IsSprinting { get; private set; } = false;
    public Vector2 MoveInput {  get; private set; }
    public Vector2 LookInput { get; private set; }


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

    // may be usefull if we use Cinemachine Third Person Camera instead of FreeLook.
    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    public void OnLeftArmAttack(InputAction.CallbackContext context)
    {
        //TODO: implement left arm attack
    }

    public void OnRightArmAttack(InputAction.CallbackContext context)
    {
        //TODO: implement right arm attack
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        //TODO: implement interact
    }



}

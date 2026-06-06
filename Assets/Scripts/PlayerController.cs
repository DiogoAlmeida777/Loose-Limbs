using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharStats))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    private CharacterController characterController;
    [SerializeField] private CharStats stats;

    private Vector3 deltaPosition = Vector3.zero;



    private float currentSpeed;

    private static float minGravitySpeed = -1f;
    private float yVelocity = minGravitySpeed;

    private CeillingSensor ceillingSensor;



    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        if (!characterController)
            Debug.Log($"{typeof(CharacterController).Name} not found at {gameObject.name}!");

        ceillingSensor = GetComponentInChildren<CeillingSensor>();
        ceillingSensor.ceillingCollision += OnCeillingCollision;
    }

    private void OnDisable()
    {
        ceillingSensor.ceillingCollision -= OnCeillingCollision;
    }

    // Update is called once per frame
    void Update()
    {
        applyGravity();
        applyMovement();
    }

    private void applyMovement()
    {
        Vector3 XZMotion = transform.TransformDirection(deltaPosition);
        XZMotion = XZMotion * stats.MoveSpeed * Time.deltaTime;

        Vector3 YMotion = Vector3.up * yVelocity * Time.deltaTime;

        Vector3 MoveVector = XZMotion + YMotion;

        characterController.Move(MoveVector);
    }


    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        deltaPosition = new Vector3(input.x, 0, input.y);
    }

    void OnLook(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        transform.Rotate(Vector3.up, input.x * stats.TurnSpeed * Time.deltaTime);
    }

    void OnJump()
    {
        if (!isGrounded()) return;

        yVelocity = stats.JumpForce;
    }

    private void applyGravity()
    {
        if (isGrounded() && yVelocity < 0.0f)
        {
            yVelocity = minGravitySpeed;
        }
        else
        {
            yVelocity += stats.GravityAcc * stats.GravityMultiplier * Time.deltaTime;
        }
    }

    private void OnCeillingCollision()
    {
        yVelocity = minGravitySpeed;
    } 


    public bool isGrounded() => characterController.isGrounded;


}

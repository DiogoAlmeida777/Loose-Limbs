using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharStats stats;
    private PlayerInputHandler inputHandler;
    private CharacterController characterController;

    #region Ceiling Collision Check
    private CeilingSensor ceilingSensor;
    public event Action hitCeiling;
    #endregion

    #region XZ Movement
    [SerializeField] private float minMoveSpeed = 1.0f;
    #endregion

    #region Vertical Movement 
    [SerializeField] private float gravityAcceleration = -9.81f;
    [SerializeField] private float gravityMultiplier = 1.0f;
    private static float minGravitySpeed = -1f;
    private float yVelocity = minGravitySpeed;
    #endregion

    #region Look Direction
    [SerializeField] private Transform cameraTransform; 
    //private Vector3 lookDirection;

    private float yaw;
    private float pitch;
    private float currentYaw;
    private float currentPitch;
    [SerializeField] private float minPitch = -60f;
    [SerializeField] private float maxPitch = 60f;
    [SerializeField] private float turnSpeed = 180f;
    [SerializeField] private float horizontalSensitivity = 0.5f;
    [SerializeField] private float verticalSensitivity = 0.5f;
    [SerializeField] private Transform cameraTarget;
    #endregion

    void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        characterController = GetComponent<CharacterController>();
        if (!characterController)
            Debug.Log($"{typeof(CharacterController).Name} not found at {gameObject.name}!");
        ceilingSensor = GetComponentInChildren<CeilingSensor>();
    }

    private void OnEnable()
    {
        ceilingSensor.ceilingCollision += OnceilingCollision;
    }

    private void OnDisable()
    {
        ceilingSensor.ceilingCollision -= OnceilingCollision;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputHandler.Jumped && IsGrounded())
            yVelocity = stats.JumpForce;

        Gravity();
        Move();
    }

    private void LateUpdate()
    {
        LookAndTurn();
    }

    private void Move()
    {
        Vector2 inputVector = inputHandler.MoveInput;
        Vector3 deltaPosition = new Vector3(inputVector.x, 0, inputVector.y);
        Vector3 XZMotion = transform.TransformDirection(deltaPosition);
        XZMotion *= stats.MoveSpeed * Time.deltaTime;
        Vector3 YMotion = Vector3.up * yVelocity * Time.deltaTime;
        Vector3 MoveVector = XZMotion + YMotion;
        characterController.Move(MoveVector);
    }

    // Function to use in case of free look cinemachine camera.
    private void FreeLookTurn()
    {
        Vector3 lookDirection = cameraTransform.forward;
        lookDirection.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,turnSpeed *Time.deltaTime);
    }

    private void LookAndTurn()
    {
        Vector2 lookInput = inputHandler.LookInput;

        yaw += lookInput.x * horizontalSensitivity;
        pitch -= lookInput.y * verticalSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        currentYaw = Mathf.Lerp(currentYaw, yaw, turnSpeed * Time.deltaTime);
        currentPitch = Mathf.Lerp(currentPitch,pitch,turnSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0f, currentYaw, 0f);
        cameraTarget.localRotation = Quaternion.Euler(currentPitch, 0f, 0f);
    }

    private void Gravity()
    {
        if (IsGrounded() && yVelocity < 0.0f)
        {
            yVelocity = minGravitySpeed;
        }
        else
        {
            yVelocity += gravityAcceleration * gravityMultiplier * Time.deltaTime;
        }
    }

    private void OnceilingCollision()
    {
        yVelocity = minGravitySpeed;
        hitCeiling?.Invoke();
    } 

    public bool IsGrounded() => characterController.isGrounded;


}

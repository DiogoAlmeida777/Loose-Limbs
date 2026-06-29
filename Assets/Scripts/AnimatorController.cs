using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimatorController : MonoBehaviour
{
    public Animator anim;

    #region Dependencies
    public PlayerController playerController;
    public PlayerInputHandler inputHandler;
    public LimbsManager limbsManager;
    #endregion 

    #region Animation Parameters Strings
    public string fwdSpeedString = "fwdSpeed";
    public string sideSpeedString = "sideSpeed";
    public string rotationString = "rotation";
    public string isIdleString = "isIdle";
    public string isSprintingString = "isSprinting";
    public string onGroundString = "onGround";
    public string jumpString = "jump";
    public string isDeadString = "isDead";
    public string hitCeilingString = "hitCeiling";
    public string numOfLegsString = "NumberOfLegs";
    #endregion

    #region Animation Parameters Hashes
    public int fwdSpeedHash { get; private set; }
    public int sideSpeedHash { get; private set; }
    public int rotationHash { get; private set; }
    public int isIdleHash { get; private set; }
    public int isSprintingHash { get; private set; }
    public int onGroundHash { get; private set; }
    public int jumpHash { get; private set; }
    public int isDeadHash { get; private set; }
    public int hitCeilingHash { get; private set; }
    public int numOfLegsHash { get; private set; }
    #endregion

    public int rifleAimingLayer;

    #region State Machine
    public MovementState currentState { get; private set; }

    public IdleState idleState;
    public RunningState walkState;
    public SprintingState sprintingState;
    public JumpingState jumpingState;
    #endregion

    public Vector2 moveInput {  get; private set; }
    [SerializeField] private float damping = .2f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        if (!anim)
            Debug.Log("Animator not found!");

        playerController = GetComponent<PlayerController>();
        inputHandler = GetComponent<PlayerInputHandler>();
        limbsManager = GetComponent<LimbsManager>();

        fwdSpeedHash = Animator.StringToHash(fwdSpeedString);
        sideSpeedHash = Animator.StringToHash(sideSpeedString);
        rotationHash = Animator.StringToHash(rotationString);
        isIdleHash = Animator.StringToHash(isIdleString);
        isSprintingHash = Animator.StringToHash(isSprintingString);
        onGroundHash = Animator.StringToHash(onGroundString);
        jumpHash = Animator.StringToHash(jumpString);
        isDeadHash = Animator.StringToHash(isDeadString);
        hitCeilingHash = Animator.StringToHash(hitCeilingString);
        numOfLegsHash = Animator.StringToHash(numOfLegsString);

        rifleAimingLayer = anim.GetLayerIndex("RifleAiming");
    }

    private void OnEnable()
    {
        playerController.hitCeiling += OnHitCeiling;
        initializeStatemachine();
    }

    private void OnDisable()
    {
        playerController.hitCeiling -= OnHitCeiling;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetInteger(numOfLegsHash, limbsManager.numberOfLegs);
        moveInput = inputHandler.MoveInput;
        anim.SetFloat(fwdSpeedHash, moveInput.y,damping,Time.deltaTime);
        anim.SetFloat(sideSpeedHash,moveInput.x,damping, Time.deltaTime);
        anim.SetFloat(rotationHash, inputHandler.LookInput.x);
        
        currentState.UpdateState(this);

        anim.SetBool(onGroundHash, playerController.IsGrounded());
    }

    private void initializeStatemachine()
    {
        idleState = new IdleState();
        walkState = new RunningState();
        sprintingState = new SprintingState();
        jumpingState = new JumpingState();
        changeState(idleState);
    }

    public void changeState(MovementState state) { 
        currentState = state;
        currentState.EnterState(this);
    }

    public void changeRifleAimingWeight(float weight)
    {
        anim.SetLayerWeight(rifleAimingLayer, weight);
    }

    public void OnAttack(string stateNameHash, string layerName)
    {
        int layer = anim.GetLayerIndex(layerName);
        anim.Play(stateNameHash, layer);
    }

    private void OnHitCeiling()
    {
        anim.SetTrigger(hitCeilingHash);
    }

    public void Death()
    {
        anim.SetBool(isDeadHash, true);
    }
}

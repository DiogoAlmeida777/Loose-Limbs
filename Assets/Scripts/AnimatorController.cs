using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimatorController : MonoBehaviour
{
    public Animator anim;

    public DeltaTransform dt;

    public PlayerController playerController;

    public string animParameter;

    public string animParameter2;

    protected int speedHash;
    protected int speedHash2;



    private void Awake()
    {
        anim = GetComponent<Animator>();
        if (!anim)
            Debug.Log("Animator not found!");

        dt = GetComponent<DeltaTransform>();
        if (!dt)
            Debug.Log("Delta Transform not found!");

        speedHash = Animator.StringToHash(animParameter);
        speedHash2 = Animator.StringToHash(animParameter2);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat(animParameter, dt.fwdSpeed());
        anim.SetFloat(animParameter2, dt.sideSpeed());
        anim.SetFloat("Speed", dt.xzSpeed());
        anim.SetFloat("ySpeed", dt.ySpeed());
        anim.SetFloat("Rotation", dt.rotSpeed().y);
        anim.SetBool("onGround",playerController.isGrounded());
        
    }


    void OnJump()
    {
        anim.SetTrigger("Jump");
        
    }
}

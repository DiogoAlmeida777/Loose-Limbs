using UnityEngine;

public class JumpingState : MovementState
{
    public override void EnterState(AnimatorController animatorController)
    {
        animatorController.anim.SetBool(animatorController.jumpHash, true);
    }

    public override void UpdateState(AnimatorController animatorController)
    {
        if (!animatorController.inputHandler.Jumped && animatorController.playerController.IsGrounded())
        {
            ExitState(animatorController, animatorController.idleState);
        }
    }

    public override void ExitState(AnimatorController animatorController, MovementState state)
    {
        animatorController.anim.SetBool(animatorController.jumpHash, false);
        animatorController.changeState(state);
    }

}

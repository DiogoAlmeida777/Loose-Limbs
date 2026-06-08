using UnityEngine;

public class IdleState : MovementState
{
    public override void EnterState(AnimatorController animatorController)
    {
        animatorController.anim.SetBool(animatorController.isIdleHash, true);
    }

    public override void UpdateState(AnimatorController animatorController)
    {
        if (animatorController.dt.movSpeed().sqrMagnitude > 0.1f)
        {
            ExitState(animatorController, animatorController.walkState);
        }

        if (animatorController.inputHandler.Jumped && animatorController.playerController.IsGrounded())
        {
            ExitState(animatorController, animatorController.jumpingState);
        }

    }

    public override void ExitState(AnimatorController animatorController, MovementState state)
    {
        animatorController.anim.SetBool(animatorController.isIdleHash, false);
        animatorController.changeState(state);
    }

}

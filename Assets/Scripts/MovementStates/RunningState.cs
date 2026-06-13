using UnityEngine;

public class RunningState : MovementState
{
    public override void EnterState(AnimatorController animatorController)
    {

    }

    public override void UpdateState(AnimatorController animatorController)
    {
        if (animatorController.inputHandler.IsSprinting)
        {
            animatorController.changeState(animatorController.sprintingState);
        }

        if (animatorController.moveInput == Vector2.zero)
        {
            animatorController.changeState(animatorController.idleState);
        }

        if (animatorController.inputHandler.Jumped && animatorController.playerController.IsGrounded())
        {
            animatorController.changeState(animatorController.jumpingState);
        }
    }

}

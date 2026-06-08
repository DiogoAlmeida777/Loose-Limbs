using UnityEngine;

public class SprintingState : MovementState
{
    public override void EnterState(AnimatorController animatorController)
    {
        animatorController.anim.SetBool(animatorController.isSprintingHash, true);
    }

    public override void UpdateState(AnimatorController animatorController)
    {
        if (!animatorController.inputHandler.IsSprinting)
        {
            ExitState(animatorController, animatorController.walkState);
        }
        else if (animatorController.inputHandler.Jumped)
        {
            ExitState(animatorController, animatorController.jumpingState);
        }
        else if (animatorController.dt.movSpeed().sqrMagnitude < 0.1f)
        {
            ExitState(animatorController, animatorController.idleState);
        }
    }

    public override void ExitState(AnimatorController animatorController, MovementState state)
    {
        animatorController.anim.SetBool(animatorController.isSprintingHash, false);
        animatorController.changeState(state);
    }
}

using UnityEngine;

public abstract class MovementState
{
    public abstract void EnterState(AnimatorController animatorController);

    public abstract void UpdateState(AnimatorController animatorController);

    public virtual void ExitState(AnimatorController animatorController, MovementState state) { }
    
}

using UnityEngine;

public class AnimationEventDispatcher : MonoBehaviour
{
    private MeleeArm[] arms;

    private void Awake()
    {
        arms = GetComponentsInChildren<MeleeArm>(true);
    }

    public void OnEnableHitbox(string side)
    {
        foreach (var arm in arms)
        {
            if (!arm.isActiveAndEnabled) continue;
            arm.OnEnableHitbox(side);
        }
    }

    public void OnDisableHitbox(string side)
    {
        foreach (var arm in arms)
        {
            if (!arm.isActiveAndEnabled) continue;
            arm.OnDisableHitbox(side);
        }
    }
}
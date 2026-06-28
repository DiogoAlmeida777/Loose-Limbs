using UnityEngine;
using UnityEngine.Events;

public class MeleeArm : Arm
{
    public UnityEvent<string,string> playAttackAnim;


    public override void Attack()
    {
        playAttackAnim?.Invoke(stats.attackAnimStateName, stats.animLayerName);
    }
}

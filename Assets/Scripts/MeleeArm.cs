using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;

public class MeleeArm : Arm
{
    public UnityEvent<string,string> playAttackAnim;


    public override bool Attack()
    {
        if (base.Attack())
        {
            playAttackAnim.Invoke(stats.attackAnimStateName, stats.animLayerName);
            return true;
        }

        return false;


    }
}

using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class Arm : MonoBehaviour
{
    [SerializeField] protected ArmStats stats;
    [SerializeField] protected PlayerInputHandler playerInputHandler;
    protected float nextAttackTime;

    public bool CanAttack => Time.time > nextAttackTime;

    protected virtual void Update()
    {
        if (playerInputHandler.IsLeftAttacking && stats.bodySide == BodySide.Left)
            if (CanAttack)
            {
                Attack();
                nextAttackTime = Time.time + stats.attackCooldown;
                return;
            }


        if (playerInputHandler.IsRightAttacking && stats.bodySide == BodySide.Right)
            if (CanAttack)
            {
                Attack();
                nextAttackTime = Time.time + stats.attackCooldown;
                return;
            }
    }

    public bool canGrab()
    {
        return stats.canGrab;
    }

    public abstract void Attack();

}

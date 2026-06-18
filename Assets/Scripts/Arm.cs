using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class Arm : MonoBehaviour
{
    [SerializeField] protected ArmStats stats;
    [SerializeField] private PlayerInputHandler playerInputHandler;
    protected float nextAttackTime;

    public bool CanAttack => Time.time > nextAttackTime;

    protected virtual void Update()
    {
        if (playerInputHandler.IsLeftAttacking && stats.bodySide == BodySide.Left)
            if (CanAttack)
            {
                Attack();
                nextAttackTime = Time.time + stats.attackCooldown;
            }


        if (playerInputHandler.IsRightAttacking && stats.bodySide == BodySide.Right)
            if (CanAttack)
            {
                Attack();
                nextAttackTime = Time.time + stats.attackCooldown;
            }
    }

    public abstract void Attack();

}

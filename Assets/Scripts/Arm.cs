using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class Arm : MonoBehaviour
{
    [SerializeField] protected ArmStats stats;
    [SerializeField] private PlayerInputHandler playerInputHandler;
    private float nextAttackTime;

    protected virtual void Update()
    {
        if (playerInputHandler.IsLeftAttacking && stats.bodySide == BodySide.Left)
            Attack();
        
        if (playerInputHandler.IsRightAttacking && stats.bodySide == BodySide.Right) 
            Attack();
    }

    public virtual bool Attack() {

        if (Time.time < nextAttackTime)
        {
            return false;
        }
            

        nextAttackTime = Time.time + stats.attackCooldown;
        return true;
    }


}

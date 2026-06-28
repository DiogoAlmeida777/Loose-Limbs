using UnityEngine;

public class GrabbingArm : MeleeArm
{
    public Gun gun;

    protected override void Update()
    {
        if (gun)
        {
            if (playerInputHandler.IsLeftAttacking && stats.bodySide == BodySide.Left)
            {
                if(gun.getGunType() == GunType.Pistol)
                {
                    gun.pullTrigger();
                }
                return;
            }

            if (playerInputHandler.IsRightAttacking && stats.bodySide == BodySide.Right)
            {
                gun.pullTrigger();
                return;
            }
        }
        else
        {
            base.Update();
        }
    }

    private void OnDisable()
    {
        detachGun();
    }

    public override void Attack()
    {
        if (gun)
            gun.pullTrigger();
        else
            base.Attack();
    }

    public void attachGun(Gun grabbedGun) { 
        gun = grabbedGun;
    }

    public void detachGun()
    {
        gun = null;
    }
}

using UnityEngine;

public class RangedArm : Arm

{
    [SerializeField] private GameObject projectilePrefab;
    public override bool Attack()
    {
        GameObject projectile = Instantiate(projectilePrefab);
        return base.Attack();
    }
}

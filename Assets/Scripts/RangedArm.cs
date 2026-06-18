using Unity.VisualScripting;
using UnityEngine;

public class RangedArm : Arm

{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform handPos;
    [SerializeField] private Transform aimPos;

    private void Start()
    {
        aimPos = GameObject.FindWithTag("AimPosition").transform;
    }
    public override void Attack()
    {
       Quaternion rotation = Quaternion.LookRotation(aimPos.position-handPos.position);
       GameObject projectile = Instantiate(projectilePrefab,handPos.position,rotation);
    }
}

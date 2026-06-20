using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RangedArm : Arm

{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform handPos;
    [SerializeField] private Transform aimPos;
    public UnityEvent<float,BodySide> changeRigWeight;

    private void Start()
    {
        aimPos = GameObject.FindWithTag("AimPosition").transform;
    }

    private void OnEnable()
    {
        changeRigWeight?.Invoke(1.0f,stats.bodySide);
    }
    public override void Attack()
    {
       Quaternion rotation = Quaternion.LookRotation(aimPos.position-handPos.position);
       GameObject projectile = Instantiate(projectilePrefab,handPos.position,rotation);
    }

    private void OnDisable()
    {
        changeRigWeight?.Invoke(0f,stats.bodySide);
    }
}

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RangedArm : Arm

{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform handPos;
    [SerializeField] private Transform aimPos;
    public UnityEvent<BodySide,float> OnRigConfigChange;

    private void Start()
    {
        aimPos = GameObject.FindWithTag("AimPosition").transform;
    }

    private void OnEnable()
    {
        OnRigConfigChange?.Invoke(stats.bodySide,1.0f);
    }
    public override void Attack()
    {
       Quaternion rotation = Quaternion.LookRotation(aimPos.position-handPos.position);
       GameObject projectile = Instantiate(projectilePrefab,handPos.position,rotation);
    }

    private void OnDisable()
    {
        OnRigConfigChange?.Invoke(stats.bodySide,0f);
    }
}

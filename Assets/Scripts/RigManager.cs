using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigManager : MonoBehaviour
{

    [SerializeField] private Rig rig;
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private TwoBoneIKConstraint rightHandIK;
    [SerializeField] private MultiAimConstraint leftHandAim;
    [SerializeField] private MultiAimConstraint rightHandAim;

    [SerializeField] private Transform leftHandtarget;
    [SerializeField] private Transform aimPos;

    private void Awake()
    {
        aimPos = GameObject.FindWithTag("AimPosition").transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = GetComponent<Rig>();
        if (rig)
            enableRig();
    }

    public void setArmConfig(BodySide side, float weight)
    {
        if (side == BodySide.Left)
        {
            leftHandIK.weight = weight;
            leftHandAim.weight = weight;
            //leftHandtarget.SetParent(aimPos);
        }
        else if (side == BodySide.Right)
        {
            rightHandIK.weight = weight;
            rightHandAim.weight = weight;
        }
    }

    public void setArmsRestPos()
    {
        leftHandIK.weight = 0f;
        leftHandAim.weight = 0f;
        rightHandIK.weight = 0f;
        rightHandAim.weight = 0f;
    }

    public void setHoldingTwoHandsWeaponConfig(Transform handlePos)
    {
        rightHandAim.weight = 1.0f;
        rightHandIK.weight = 0f;
        leftHandIK.weight = 1.0f;
        leftHandAim.weight = 0f;
        leftHandtarget.SetParent(handlePos);
    }

    public void setupHandIKWeight(float newWeight, BodySide side)
    {
        if (side == BodySide.Left)
            leftHandIK.weight = newWeight;
        else
            rightHandIK.weight = newWeight;
    }

    public void setupHandAimWeight(float newWeight, BodySide side)
    {
        if (side == BodySide.Left)
            leftHandAim.weight = newWeight;
        else
            rightHandAim.weight = newWeight;
    }

    public void enableRig()
    {
        rig.weight = 1.0f;
    }

    public void disableRig()
    {
        rig.weight = 0f;
    }
}

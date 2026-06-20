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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = GetComponent<Rig>();
        if (rig)
            enableRig();
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

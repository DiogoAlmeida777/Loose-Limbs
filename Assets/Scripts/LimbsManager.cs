using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class LimbsManager : MonoBehaviour
{
    #region Limbs
    [SerializeField] private GameObject[] R_Arms;
    [SerializeField] private GameObject[] L_Arms;
    [SerializeField] private GameObject[] R_Legs;
    [SerializeField] private GameObject[] L_Legs;
    #endregion

    #region Active Limbs
    public GameObject currentLeftArm { get; private set; }
    public GameObject currentRightArm { get; private set; }
    public GameObject currentLeftLeg { get; private set; }
    public GameObject currentRightLeg { get; private set; }
    #endregion

    #region Leg Statistics
    private LegStats leftLegStats;
    private LegStats rightLegStats;
    #endregion

    #region Limb's Hurtboxes
    [SerializeField] private Hurtbox[] leftArmHurtboxes;
    [SerializeField] private Hurtbox[] leftLegHurtboxes;
    [SerializeField] private Hurtbox[] rightArmHurtboxes;
    [SerializeField] private Hurtbox[] rightLegHurtboxes;
    #endregion


    public UnityEvent<BodySide, bool, GameObject> OnLimbChanged = new UnityEvent<BodySide, bool, GameObject>();

    public int numberOfLegs { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLeftArm = null;
        currentLeftLeg = null;
        currentRightArm = null;
        currentRightLeg = null;
        leftLegStats = null;
        rightLegStats = null;
        numberOfLegs = 0;
    }


    private void getArm(ArmStats arm)
    {
        if (arm.bodySide == BodySide.Left)
        {
            if (currentLeftArm != null)
            {
                currentLeftArm.SetActive(false);
            }
            int index = (int)arm.type;
            currentLeftArm = L_Arms[index];
            currentLeftArm.SetActive(true);
            foreach (Hurtbox hurtbox in leftArmHurtboxes)
                hurtbox.setHealth(currentLeftArm.GetComponent<Health>());

            OnLimbChanged?.Invoke(BodySide.Left, true, currentLeftArm);
        }
        else if (arm.bodySide == BodySide.Right)
        {
            if (currentRightArm != null)
            {
                currentRightArm.SetActive(false);
            }
            int index = (int)arm.type;
            currentRightArm = R_Arms[index];
            currentRightArm.SetActive(true);
            foreach (Hurtbox hurtbox in rightArmHurtboxes)
                hurtbox.setHealth(currentRightArm.GetComponent<Health>());

            OnLimbChanged?.Invoke(BodySide.Right, true, currentRightArm);
        }

    }

    private void getLeg(LegStats leg)
    {
        if (leg.bodySide == BodySide.Left)
        {
            if (currentLeftLeg != null)
            {
                currentLeftLeg.SetActive(false);
            }
            else
                numberOfLegs++;

            int index = (int)leg.type;
            currentLeftLeg = L_Legs[index];
            currentLeftLeg.SetActive(true);
            leftLegStats = leg;
            foreach (Hurtbox hurtbox in leftLegHurtboxes)
                hurtbox.setHealth(currentLeftLeg.GetComponent<Health>());

            OnLimbChanged?.Invoke(BodySide.Left, false, currentLeftLeg);
        }
        else if (leg.bodySide == BodySide.Right)
        {
            if (currentRightLeg != null)
            {
                currentRightLeg.SetActive(false);
            }
            else
                numberOfLegs++;

            int index = (int)leg.type;
            currentRightLeg = R_Legs[index];
            currentRightLeg.SetActive(true);
            rightLegStats = leg;
            foreach (Hurtbox hurtbox in rightLegHurtboxes)
                hurtbox.setHealth(currentRightLeg.GetComponent<Health>());

            OnLimbChanged?.Invoke(BodySide.Right, false, currentRightLeg);
        }
    }

    public void getLimb(LimbStats limb)
    {
        if (limb is LegStats leg)
            getLeg(leg);

        if (limb is ArmStats arm)
            getArm(arm);
    }

    private void loseArm(BodySide bs)
    {
        if (bs == BodySide.Left)
        {
            currentLeftArm = null;
            foreach (Hurtbox hurtbox in leftArmHurtboxes)
                hurtbox.setHealth(null);
        }
        else if (bs == BodySide.Right)
        {
            currentRightArm = null;
            foreach (Hurtbox hurtbox in rightArmHurtboxes)
                hurtbox.setHealth(null);
        }

        OnLimbChanged?.Invoke(bs, true, null);
    }

    private void loseLeg(BodySide bs)
    {
        if (bs == BodySide.Left)
        {
            currentLeftLeg = null;
            leftLegStats = null;
            foreach (Hurtbox hurtbox in leftLegHurtboxes)
                hurtbox.setHealth(null);
        }
        else if (bs == BodySide.Right)
        {
            currentRightLeg = null;
            rightLegStats = null;
            foreach (Hurtbox hurtbox in rightLegHurtboxes)
                hurtbox.setHealth(null);
        }
        numberOfLegs -= 1;
        OnLimbChanged?.Invoke(bs, false, null);
    }


    public void loseLimb(LimbStats limb)
    {
        BodySide bodySide = limb.bodySide;
        if (limb is LegStats)
            loseLeg(bodySide);

        if (limb is ArmStats)
            loseArm(bodySide);
    }

    private bool armCanGrab(GameObject armGO)
    {
        if (!armGO) return false;
        Arm arm = armGO.GetComponent<Arm>();
        if (!arm) return false;
        if (!arm.canGrab()) return false;
        return true;
    }

    public int numberOfArmsCanGrab()
    {
        int numberOfArms = 0;

        if (armCanGrab(currentLeftArm))
            numberOfArms++;

        if (armCanGrab(currentRightArm))
            numberOfArms++;

        return numberOfArms;
    }

    public bool LeftArmCanGrab => armCanGrab(currentLeftArm);
    public bool RightArmCanGrab => armCanGrab(currentRightArm);


    public float MoveSpeed =>
        (leftLegStats != null ? leftLegStats.moveSpeed : 0) +
        (rightLegStats != null ? rightLegStats.moveSpeed : 0);

    public float SprintBuff =>
        1 +
        (leftLegStats != null ? leftLegStats.sprintMultiplier : 0) +
        (rightLegStats != null ? rightLegStats.sprintMultiplier : 0);

    public float JumpForce =>
        (leftLegStats != null ? leftLegStats.jumpForce : 0) +
        (rightLegStats != null ? rightLegStats.jumpForce : 0);

    public float Stamina =>
        (leftLegStats != null ? leftLegStats.stamina : 0) +
        (rightLegStats != null ? rightLegStats.stamina : 0);

}

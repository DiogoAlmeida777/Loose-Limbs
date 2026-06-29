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

    [SerializeField] private LimbDropper limbDropper;

    public int numberOfLegs { get; private set; }

    public UnityEvent<BodySide> OnGrabbingArmLoss; 

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


    private void getArm(ArmStats arm, float armHealth)
    {
        if (arm.bodySide == BodySide.Left)
        {
            if (currentLeftArm != null)
            {
                if (LeftArmCanGrab)
                    OnGrabbingArmLoss?.Invoke(BodySide.Left);
                currentLeftArm.SetActive(false);
                limbDropper.drop(
                    currentLeftArm.GetComponent<Arm>().Stats,
                    currentLeftArm.GetComponent<LimbHealth>().currentHealth,
                    currentLeftArm.transform.position,
                    currentLeftArm.transform.rotation
                    );

            }
            int index = (int)arm.type;
            currentLeftArm = L_Arms[index];
            currentLeftArm.SetActive(true);
            LimbHealth health = currentLeftArm.GetComponent<LimbHealth>();
            health.setCurrentHealth(armHealth);
            foreach (Hurtbox hurtbox in leftArmHurtboxes)
                hurtbox.setHealth(health);
        }
        else if (arm.bodySide == BodySide.Right)
        {
            if (currentRightArm != null)
            {
                if (RightArmCanGrab)
                    OnGrabbingArmLoss?.Invoke(BodySide.Right);
                currentRightArm.SetActive(false);
                limbDropper.drop(
                    currentRightArm.GetComponent<Arm>().Stats,
                    currentRightArm.GetComponent<LimbHealth>().currentHealth,
                    currentRightArm.transform.position,
                    currentRightArm.transform.rotation
                    );

            }
            int index = (int)arm.type;
            currentRightArm = R_Arms[index];
            currentRightArm.SetActive(true);
            LimbHealth health = currentRightArm.GetComponent<LimbHealth>();
            health.setCurrentHealth(armHealth);
            foreach (Hurtbox hurtbox in rightArmHurtboxes)
                hurtbox.setHealth(health);
        }

    }

    private void getLeg(LegStats leg, float legHealth)
    {
        if (leg.bodySide == BodySide.Left)
        {
            if (currentLeftLeg != null)
            {
                currentLeftLeg.SetActive(false);
                limbDropper.drop(
                    leftLegStats,
                    currentLeftLeg.GetComponent<LimbHealth>().currentHealth,
                    currentLeftLeg.transform.position,
                    currentLeftLeg.transform.rotation);
            }
            else
                numberOfLegs++;

            int index = (int)leg.type;
            currentLeftLeg = L_Legs[index];
            currentLeftLeg.SetActive(true);
            leftLegStats = leg;
            LimbHealth health = currentLeftLeg.GetComponent<LimbHealth>();
            health.setCurrentHealth(legHealth);
            foreach (Hurtbox hurtbox in leftLegHurtboxes)
                hurtbox.setHealth(health);
        }
        else if (leg.bodySide == BodySide.Right)
        {
            if (currentRightLeg != null)
            {
                currentRightLeg.SetActive(false);
                limbDropper.drop(
                    rightLegStats,
                    currentRightLeg.GetComponent<LimbHealth>().currentHealth,
                    currentRightLeg.transform.position,
                    currentRightLeg.transform.rotation);
            }
            else
                numberOfLegs++;

            int index = (int)leg.type;
            currentRightLeg = R_Legs[index];
            currentRightLeg.SetActive(true);
            rightLegStats = leg;
            LimbHealth health = currentRightLeg.GetComponent<LimbHealth>();
            health.setCurrentHealth(legHealth);
            foreach (Hurtbox hurtbox in rightLegHurtboxes)
                hurtbox.setHealth(health);
        }
    }

    public void getLimb(LimbStats limb, float limbHealth)
    {
        if (limb is LegStats leg)
            getLeg(leg, limbHealth);

        if (limb is ArmStats arm)
            getArm(arm, limbHealth);
    }

    private void loseArm(BodySide bs)
    {
        if (bs == BodySide.Left)
        {
            if(LeftArmCanGrab)
                OnGrabbingArmLoss?.Invoke(BodySide.Left);
            currentLeftArm = null;
            foreach (Hurtbox hurtbox in leftArmHurtboxes)
                hurtbox.setHealth(null);
        }
        else if (bs == BodySide.Right)
        {
            if (RightArmCanGrab)
                OnGrabbingArmLoss?.Invoke(BodySide.Right);
            currentRightArm = null;
            foreach (Hurtbox hurtbox in rightArmHurtboxes)
                hurtbox.setHealth(null);
        }
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
        (leftLegStats != null? leftLegStats.moveSpeed : 0) + 
        (rightLegStats != null? rightLegStats.moveSpeed : 0);

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

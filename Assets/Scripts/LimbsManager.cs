using System;
using UnityEngine;

public class LimbsManager : MonoBehaviour
{
    [SerializeField] private GameObject[] R_Arms;
    [SerializeField] private GameObject[] L_Arms;
    [SerializeField] private GameObject[] R_Legs;
    [SerializeField] private GameObject[] L_Legs;

    private GameObject currentLeftArm;
    private GameObject currentRightArm;
    private GameObject currentLeftLeg;
    private GameObject currentRightLeg;

    private LegStats leftLegStats;
    private LegStats rightLegStats;

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

    private void Update()
    {
        Debug.Log(numberOfLegs);
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
            currentLeftArm = null;
        else if (bs == BodySide.Right)
            currentRightArm = null;
    }

    private void loseLeg(BodySide bs)
    {
        if (bs == BodySide.Left)
        {
            currentLeftLeg = null;
            leftLegStats = null;
        }
        else if (bs == BodySide.Right)
        {
            currentRightLeg = null;
            rightLegStats = null;
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

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

    [SerializeField] private LegStats leftLegStats;
    [SerializeField] private LegStats rightLegStats;

    public int numberOfLegs { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void loseLimb()
    {

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
        if (leg.bodySide == BodySide.Right)
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

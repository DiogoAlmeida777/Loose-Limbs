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
    [SerializeField] private ArmStats leftArmStats;
    [SerializeField] private ArmStats rightArmStats;

    private int numberOfLegs;

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
            leftArmStats = arm;
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
            rightArmStats = arm;
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
        if(limb is LegStats leg)
            getLeg(leg);

        if(limb is ArmStats arm)
            getArm(arm);
    }





    

    
}

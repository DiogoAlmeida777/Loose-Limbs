using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LimbsManager : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<LimbType, GameObject> R_Arms;
    //[SerializeField] private GameObject[] R_Arms;
    [SerializeField] private GameObject[] L_Arms;
    [SerializeField] private GameObject[] R_Legs;
    [SerializeField] private GameObject[] L_Legs;

    private GameObject currentLeftArm;
    private GameObject currentRightArm;
    private GameObject currentLeftLeg;
    private GameObject currentRightLeg;

    private int numberOfLegs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}

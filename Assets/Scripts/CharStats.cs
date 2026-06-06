using UnityEngine;
using System;

public class CharStats : MonoBehaviour
{
    [SerializeField] private float moveSpeed  = 5.0f;
    [SerializeField] private float sprintSpeed = 10.0f;
    [SerializeField] private float moveBuff = 1.0f;
    [SerializeField] private float minSpeed  = 1.0f;

    [SerializeField] private float turnSpeed  = 180f;

    [SerializeField] private float jumpForce  = 5f;

    [SerializeField] private float gravityAcceleration = -9.81f;
    [SerializeField] private float gravityMultiplier = 1.0f;

    [SerializeField] private int numberOfLegs = 2;





    public float MoveSpeed => Math.Max(moveSpeed * moveBuff, minSpeed);
    public float SprintSpeed => sprintSpeed * moveBuff;
    public float TurnSpeed => turnSpeed;
    public float JumpForce => jumpForce;
    public float GravityAcc => gravityAcceleration;
    public float GravityMultiplier => gravityMultiplier;


    

}

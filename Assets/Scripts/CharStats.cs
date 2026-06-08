using UnityEngine;
using System;

public class CharStats : MonoBehaviour
{
    // TO BE REPLACED IN THE FUTURE

    [SerializeField] private float moveSpeed  = 5.0f;
    [SerializeField] private float sprintSpeed = 10.0f;
    [SerializeField] private float moveBuff = 1.0f;
    [SerializeField] private float jumpForce  = 5f;
    [SerializeField] private int numberOfLegs = 2;


    public float MoveSpeed => moveSpeed * moveBuff;
    public float SprintSpeed => sprintSpeed * moveBuff;
    public float JumpForce => jumpForce;

    

}

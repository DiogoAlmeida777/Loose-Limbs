using UnityEngine;

[CreateAssetMenu(fileName = "LegStats", menuName = "Scriptable Objects/LimbStats/LegStats")]
public class LegStats : LimbStats
{
    public float moveSpeed;
    public float sprintMultiplier;
    public float stamina;
    public float jumpForce;
}

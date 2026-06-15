using UnityEngine;

[CreateAssetMenu(fileName = "ArmStats", menuName = "Scriptable Objects/LimbStats/ArmStats")]
public class ArmStats : LimbStats
{
    public float meleeDamage;
    public float attackCooldown;
    public bool canGrab;
    public AnimationClip attackAnim;
}

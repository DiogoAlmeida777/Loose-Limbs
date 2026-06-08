using UnityEngine;

[CreateAssetMenu(fileName = "ArmStats", menuName = "Scriptable Objects/ArmStats")]
public class ArmStats : ScriptableObject
{
    public BodySide bodySide;
    public LimbType type;
    public float health;
    public float meleeDamage;
    public bool canGrab;
    public AnimationClip attackAnim;
}

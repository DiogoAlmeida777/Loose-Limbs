using UnityEngine;

[CreateAssetMenu(fileName = "LegStats", menuName = "Scriptable Objects/LegStats")]
public class LegStats : ScriptableObject
{
    public BodySide bodySide = BodySide.Left;
    public LimbType type = LimbType.Human;
    public float health = 50;
    public float moveSpeedMultiplier = 1.0f;
    public float stamina;
    public float jumpForce = 8;
}

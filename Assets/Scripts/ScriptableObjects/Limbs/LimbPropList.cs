using UnityEngine;

[CreateAssetMenu(fileName = "LimbPropList", menuName = "Scriptable Objects/LimbPropList")]
public class LimbPropList : ScriptableObject
{
    public GameObject[] leftLegPropPrefabs;
    public GameObject[] leftArmPropPrefabs;
    public GameObject[] rightLegPropPrefabs;
    public GameObject[] rightArmPropPrefabs;
}

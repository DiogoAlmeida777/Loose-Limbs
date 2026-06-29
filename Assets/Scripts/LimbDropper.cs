using UnityEngine;

public class LimbDropper : MonoBehaviour
{

    [SerializeField] private LimbPropList limbProps;
    

    public void drop(LimbStats stats, float remaininghealth, Vector3 dropPosition, Quaternion dropRotation)
    {
        GameObject prefab = findPrefab(stats);

        GameObject droppedLimb = Instantiate(prefab, dropPosition, dropRotation);

        droppedLimb.GetComponent<LimbProp>().Initialize(remaininghealth);
        
    }

    private GameObject findPrefab(LimbStats stats)
    {
        int index = (int)stats.type;

        return stats switch
        {
            ArmStats arm => arm.bodySide == BodySide.Left
                ? limbProps.leftArmPropPrefabs[index]
                : limbProps.rightArmPropPrefabs[index],

            LegStats leg => leg.bodySide == BodySide.Left
                ? limbProps.leftLegPropPrefabs[index]
                : limbProps.rightLegPropPrefabs[index],

            _ => null
        };
    }
}

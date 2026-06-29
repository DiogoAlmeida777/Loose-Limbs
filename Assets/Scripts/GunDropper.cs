using UnityEngine;

public class GunDropper : MonoBehaviour
{
    [SerializeField] private GunPropList gunPrefabs;

    public void drop(GunType gunType, int remainingAmmo, Vector3 dropPosition, Quaternion dropRotation)
    {
        int index = (int) gunType;
        GameObject prefab = gunPrefabs.guns[index];

        GameObject droppedGun = Instantiate(prefab, dropPosition, dropRotation);

        droppedGun.GetComponent<Gun>().Initialize(remainingAmmo);
    }
    
}

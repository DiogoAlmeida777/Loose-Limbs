using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct WeaponSlot
{
    public GunType gunType;
    public GameObject[] slot;
}

public class GunsManager : MonoBehaviour
{
    private PlayerInputHandler inputHandler;

    [SerializeField] private WeaponSlot[] weapons;

    private WeaponSlot currentWeapon;

    private bool isArmed = false;

    [SerializeField] private GunDropper gunDropper;

    public UnityEvent<BodySide, float> OnPistolRigConfig;

    public UnityEvent<Transform> OnTwoHandedWeaponRigConfig;

    public UnityEvent<float> OnChangeRifleAimState;

    public UnityEvent OnRestArms;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    private void OnEnable()
    {
        inputHandler.Dropped += tossWeapon;
    }

    private void OnDisable()
    {
        inputHandler.Dropped -= tossWeapon;
    }


    public bool tryEquipWeapon(Gun pickupGun, LimbsManager limbsManager) {

        GunType pickupGunType = pickupGun.getGunType();
        int slotIndex = (int)pickupGunType;

        if (slotIndex >= weapons.Length) return false;

        WeaponSlot targetSlot = weapons[slotIndex];

        if (targetSlot.slot.Length == 0 ) return false;

        // FOR WEAPONS THAT ONLY NEED ATLEAST ONE HAND (PISTOLS)
        if (pickupGunType == GunType.Pistol)
        {
            
            bool leftArmAvailable = limbsManager.LeftArmCanGrab;
            bool rightArmAvailable = limbsManager.RightArmCanGrab;

            Gun rightPistol = targetSlot.slot[0].GetComponent<Gun>();
            Gun leftPistol = targetSlot.slot[1].GetComponent<Gun>();

            if (isArmed && currentWeapon.gunType == GunType.Pistol)
            {
                
                bool isHoldingRightPistol = rightPistol != null && targetSlot.slot[0].activeSelf;
                bool isHoldingLeftPistol = leftPistol != null && targetSlot.slot[1].activeSelf;

                // CASE 1: is already armed with two pistols
                if (isHoldingRightPistol && isHoldingLeftPistol)
                {
                    addAmmoToDualWieldedWeapons(rightPistol, leftPistol, pickupGun.CurrentAmmo);
                    return true;
                }
                // CASE 2: right hand is holding a pistol
                if (isHoldingRightPistol && !isHoldingLeftPistol)
                {
                    if (leftArmAvailable)
                    {
                        targetSlot.slot[1].SetActive(true);
                        leftPistol.addAmmo(pickupGun.CurrentAmmo);
                        GrabbingArm leftArm = limbsManager.currentLeftArm.GetComponent<GrabbingArm>();
                        leftArm.attachGun(leftPistol);
                        OnPistolRigConfig?.Invoke(BodySide.Left,1.0f);
                        return true;
                    }
                    else
                    {
                        rightPistol.addAmmo(pickupGun.CurrentAmmo);
                        return true;
                    }
                }
                // Case 3: left hand is holding a pistol
                if(!isHoldingRightPistol && isHoldingLeftPistol)
                {
                    if (rightArmAvailable)
                    {
                        targetSlot.slot[0].SetActive(true);
                        rightPistol.addAmmo(pickupGun.CurrentAmmo);
                        GrabbingArm rightArm = limbsManager.currentRightArm.GetComponent<GrabbingArm>();
                        rightArm.attachGun(rightPistol);
                        OnPistolRigConfig?.Invoke(BodySide.Right,1.0f);
                        return true;
                    }
                    else
                    {
                        leftPistol.addAmmo(pickupGun.CurrentAmmo);
                        return true;
                    }
                }
            }

            // CASE 4: is unarmed or armed with another weapon type
            if (!leftArmAvailable && !rightArmAvailable) return false;

            if (isArmed)
                dropAllWeapons();

            currentWeapon = targetSlot;
            isArmed = true;

            if (rightArmAvailable)
            {
                targetSlot.slot[0].SetActive(true);
                targetSlot.slot[0].GetComponent<Gun>().addAmmo(pickupGun.CurrentAmmo);
                GrabbingArm rightArm = limbsManager.currentRightArm.GetComponent<GrabbingArm>();
                rightArm.attachGun(rightPistol);
                OnPistolRigConfig?.Invoke(BodySide.Right,1.0f);
            }
            else if (leftArmAvailable)
            {
                targetSlot.slot[1].SetActive(true);
                targetSlot.slot[1].GetComponent<Gun>().addAmmo(pickupGun.CurrentAmmo);
                GrabbingArm leftArm = limbsManager.currentLeftArm.GetComponent<GrabbingArm>();
                leftArm.attachGun(leftPistol);
                OnPistolRigConfig?.Invoke(BodySide.Left,1.0f);
            }

            return true;

        }
        // FOR WEAPONS THAT NEED 2 HANDS (RIFLES AND SNIPERS)
        else
        {
            
            if (limbsManager.numberOfArmsCanGrab() < 2) return false;

            //CASE 1: is already armed with the same weapon type
            if (isArmed && currentWeapon.gunType == pickupGunType)
            {
                Gun equipedGun = currentWeapon.slot[0].GetComponent<Gun>();
                equipedGun.addAmmo(pickupGun.CurrentAmmo);
                return true;
            }

            // CASE 2: is unarmed or armed with different weapon type
            if (isArmed)
                dropAllWeapons();

            currentWeapon = targetSlot;
            isArmed = true;
            targetSlot.slot[0].SetActive(true);
            Gun newWeapon = targetSlot.slot[0].GetComponent<Gun>();
            newWeapon.addAmmo(pickupGun.CurrentAmmo);
            GrabbingArm rightArm = limbsManager.currentRightArm.GetComponent<GrabbingArm>();
            GrabbingArm leftArm = limbsManager.currentLeftArm.GetComponent<GrabbingArm>();
            rightArm.attachGun(newWeapon);
            leftArm.attachGun(newWeapon);
            OnTwoHandedWeaponRigConfig?.Invoke(newWeapon.HandlePoint);
            OnChangeRifleAimState?.Invoke(1.0f);
            return true;
        }

    }

    private void addAmmoToDualWieldedWeapons(Gun primary, Gun secondary, int amount)
    {
        int primaryRemainingCapacity = primary.MaxAmmo - primary.CurrentAmmo;


        if (amount <= primaryRemainingCapacity) {
            primary.addAmmo(amount);
        }
        else
        {
            primary.addAmmo(primaryRemainingCapacity);
            int ammoRemainder = amount - primaryRemainingCapacity;
            secondary.addAmmo(ammoRemainder);
        }
           
    }

    private void getDisarmed()
    {
        foreach (GameObject weaponGO in currentWeapon.slot)
            if (weaponGO) weaponGO.SetActive(false);
        isArmed = false;
        OnRestArms?.Invoke();
        OnChangeRifleAimState?.Invoke(0f);
    }

    private void getDisarmed(int i)
    {
        currentWeapon.slot[i].SetActive(false);
        isArmed = false;
        foreach (GameObject weaponGO in currentWeapon.slot)
            if (weaponGO.activeSelf)
            {
                isArmed = true;
                break;
            }

        if (i == 1)
        {
            OnPistolRigConfig?.Invoke(BodySide.Left, 0f);
        }
        else
        {
            OnPistolRigConfig?.Invoke(BodySide.Right, 0f);
        }
    }

    public void tossWeapon()
    {
        if (!isArmed) return;

        if (currentWeapon.gunType != GunType.Pistol)
        {
            dropTwoHandsGun();
            return;
        }

        if (currentWeapon.slot[1].activeSelf)
        {
            dropPistol(1);
        }
        else if (currentWeapon.slot[0].activeSelf)
        {
            dropPistol(0);
        }
    }


    private void dropAllWeapons()
    {
        if (!isArmed) return;

        if (currentWeapon.gunType == GunType.Pistol)
        {
            if (currentWeapon.slot[1].activeSelf)
                dropPistol(1);
            if (currentWeapon.slot[0].activeSelf)
                dropPistol(0);
        }
        else
        {
            dropTwoHandsGun();
        }
    }

    private void dropTwoHandsGun()
    {
        Gun gun = currentWeapon.slot[0].GetComponent<Gun>();

        gunDropper.drop(
            currentWeapon.gunType,
            gun.CurrentAmmo,
            currentWeapon.slot[0].transform.position,
            currentWeapon.slot[0].transform.rotation);

        getDisarmed();
    }

    private void dropPistol(int i)
    {
        Gun gun = currentWeapon.slot[i].GetComponent<Gun>();

        gunDropper.drop(
            GunType.Pistol,
            gun.CurrentAmmo,
            currentWeapon.slot[i].transform.position,
            currentWeapon.slot[i].transform.rotation);

        getDisarmed(i);
    }

    public void removeWeaponFromArm(BodySide armSide)
    {
        if (!isArmed) return;
        if (currentWeapon.gunType == GunType.Pistol) {
            if (armSide == BodySide.Left) dropPistol(1);
            else dropPistol(0);
        }
        else
        {
            dropTwoHandsGun();
        }
    }


}

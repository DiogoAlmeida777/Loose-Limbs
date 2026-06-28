using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Gun : MonoBehaviour, IInteractable
{
    [SerializeField] private GunType type;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private float fireRate;
    [SerializeField] private int currAmmo;
    [SerializeField] private int maxAmmo;
    [SerializeField] private Transform shootPos;
    [SerializeField] private Transform aimPos;

    public Transform HandlePoint => gameObject.transform.Find("handlePos");
    public int CurrentAmmo => currAmmo;
    public int MaxAmmo => maxAmmo;

    private bool canShoot = true;

    private void chamberNextRound() { canShoot = true; }

    private void Awake()
    {
        aimPos = GameObject.FindWithTag("AimPosition").transform;
    }

    void Start()
    {
        currAmmo = maxAmmo;
    }

    private void OnDisable()
    {
        currAmmo = 0;
    }

    public void pullTrigger()
    {
        if (canShoot) {
            fire();
            canShoot = false;
            Invoke("chamberNextRound", fireRate);
        }
    }

    private void fire()
    {
        if (currAmmo == 0) return;
        Quaternion shootDir;
        if (aimPos)
            shootDir = Quaternion.LookRotation(aimPos.position - shootPos.position);
        else
            shootDir = shootPos.rotation;
        Instantiate(bulletPrefab, shootPos.position, shootDir);
        muzzleFlash.Play();
        currAmmo--;
    }

    public void addAmmo(int ammo)
    {
        currAmmo = Mathf.Min(CurrentAmmo + ammo, maxAmmo);
    }

    public void Interact(GameObject interactor)
    {
        LimbsManager limbManager = interactor.GetComponent<LimbsManager>();
        if (!limbManager) return;
        GunsManager gunsManager = interactor.GetComponent<GunsManager>();
        if (!gunsManager) return;

        if (gunsManager.tryEquipWeapon(this, limbManager))
        {
            Destroy(gameObject);
        }
    }

    public GunType getGunType()
    {
        return type;
    }
}

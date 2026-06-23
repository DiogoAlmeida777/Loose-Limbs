using System;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate;
    [SerializeField] private int maxAmmo;
    [SerializeField] private Transform shootPos;
    private int currAmmo;

    private float nextShotTimer;

    void Start()
    {
        currAmmo = maxAmmo;
    }

    public void Fire()
    {
        if (currAmmo == 0) return;
        if (nextShotTimer < fireRate) return;

        Instantiate(bulletPrefab, shootPos.position, Quaternion.identity);
        nextShotTimer = 0;
        currAmmo--;
    }

    public void addAmmo(int ammo)
    {
        currAmmo = Mathf.Min(currAmmo + ammo, maxAmmo);
    }

    // Update is called once per frame
    void Update()
    {
        nextShotTimer += Time.deltaTime;
    }
}

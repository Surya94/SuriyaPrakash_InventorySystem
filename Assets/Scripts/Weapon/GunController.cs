using System;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public WeaponDataSO weaponData;
    public Transform projectileSpawnPoint;

    public int currentAmmo = 30;
    private float lastShootTime;
    private float reloadStartTime;
    private bool isReloading;
    public AudioSource audioSource;
    private Vector3 originalPosition;
    private bool isRecoiling;
    private float recoilStartTime;

    public List<GameObject> bulletHolePool;
    public List<GameObject> hitEffectPool;
    private void Start()
    {
        originalPosition = transform.localPosition;

    }

    private void Update()
    {
        if (GameManager.Instance.IsinPauseMenu)
            return;

        if (isReloading)
        {
            float normalizedTime = (Time.time - reloadStartTime) / weaponData.reloadTime;
            if (normalizedTime >= 1f)
            {
                FinishReloading();
            }
        }
        else
        {
            if (weaponData.automatic && Input.GetButton("Fire1") && Time.time >= lastShootTime + weaponData.fireRate)
            {
                Shoot();
            }
            else if (!weaponData.automatic && Input.GetButtonDown("Fire1") && Time.time >= lastShootTime + weaponData.fireRate)
            {
                Shoot();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                StartReloading();
            }

            if (isRecoiling)
            {
                float normalizedTime = (Time.time - recoilStartTime) / weaponData.recoilDuration;
                float recoilDistance = Mathf.Lerp(0f, weaponData.recoilForce, normalizedTime);
                transform.localPosition = originalPosition + Vector3.back * recoilDistance;

                if (normalizedTime >= 1f)
                {
                    isRecoiling = false;
                }
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * weaponData.recoilResetSpeed);
            }

        }
    }

    void Shoot()
    {
        if (currentAmmo > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, GenerateBulletSpread(), out hit))
            {
                var hitEffect = GetUnusedHitEffect();
                hitEffect.SetActive(true);
                hitEffect.transform.position = hit.point;
                hitEffect.transform.rotation = Quaternion.LookRotation(hit.normal);
                var bullethole = GetUnusedBullethole();
                bullethole.SetActive(true);
                bullethole.transform.position = hit.point;
                bullethole.transform.rotation = Quaternion.LookRotation(hit.normal);
            }
            GameObject muzzle = Instantiate(weaponData.muzzleFlash, projectileSpawnPoint.position, transform.rotation);
            muzzle.transform.SetParent(transform);
            Destroy(muzzle,3f);
            currentAmmo--;
            lastShootTime = Time.time;
            audioSource.PlayOneShot(weaponData.fireSound);
            var ammoData = InventoryManager.Instance.GetItemType(weaponData.ammoType);
            if (ammoData != null)
            {
                SignalManager.Instance?.DispatchSignal(new OnUpdateAmmo() { remainingBullet = currentAmmo, availableInInventory = ammoData.quantity });
            }
            else
            {
                SignalManager.Instance?.DispatchSignal(new OnUpdateAmmo() { remainingBullet = currentAmmo, availableInInventory = 0 });
            }
            ApplyRecoil();
        }
        else
        {
            StartReloading();
        }
    }

    Vector3 GenerateBulletSpread()
    {
        float spreadAngle = UnityEngine.Random.Range(0f, weaponData.recoilForce * 15);
        Vector3 spreadDirection = Quaternion.Euler(UnityEngine.Random.Range(-spreadAngle, spreadAngle), UnityEngine.Random.Range(-spreadAngle, spreadAngle), 0f) * Camera.main.transform.forward;
        return spreadDirection;
    }


    private void ApplyRecoil()
    {
        if (!isRecoiling)
        {
            isRecoiling = true;
            recoilStartTime = Time.time;
        }
    }
    void Reload()
    {
        var ammoData = InventoryManager.Instance.GetItemType(weaponData.ammoType);
        if (ammoData != null)
        {
            if (ammoData.quantity > weaponData.maxAmmo)
            {
                ammoData.quantity -= (weaponData.maxAmmo - currentAmmo);
                currentAmmo = weaponData.maxAmmo;
            }
            else
            {
                currentAmmo = ammoData.quantity;
                ammoData.quantity = 0;
                InventoryManager.Instance.RemoveItem(weaponData.ammoType);
            }
            audioSource.PlayOneShot(weaponData.emptyFiresound);
            SignalManager.Instance?.DispatchSignal(new OnUpdateAmmo() { remainingBullet = currentAmmo, availableInInventory = ammoData.quantity });
        }
        else
        {
            SignalManager.Instance?.DispatchSignal(new OnUpdateAmmo() { remainingBullet = currentAmmo, availableInInventory = 0 });
        }
        lastShootTime = Time.time;
    }

    void StartReloading()
    {
        if (currentAmmo < weaponData.maxAmmo && !isReloading)
        {
            isReloading = true;
            reloadStartTime = Time.time;
            audioSource.PlayOneShot(weaponData.reloadSound);
        }
    }

    void FinishReloading()
    {
        isReloading = false;
        Reload();
    }

    public GameObject GetUnusedBullethole()
    {
        if (bulletHolePool == null)
            bulletHolePool = new List<GameObject>();
        foreach (var item in bulletHolePool)
        {
            if (!item.gameObject.activeSelf)
                return item;
        }
        var obj =  Instantiate(weaponData.bulletHolePrefab);
        bulletHolePool.Add(obj);
        return obj;
    }

    public GameObject GetUnusedHitEffect()
    {
        if (hitEffectPool == null)
            hitEffectPool = new List<GameObject>();
        foreach (var item in hitEffectPool)
        {
            if (!item.gameObject.activeSelf)
                return item;
        }
        var obj = Instantiate(weaponData.hitEffectPrefab);
        hitEffectPool.Add(obj);
        return obj;
    }

}

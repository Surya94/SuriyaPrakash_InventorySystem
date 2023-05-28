using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "ScriptableObjects/WeaponDataSO", order = 2)]

public class WeaponDataSO : ScriptableObject
{
    public ItemType ammoType;
    public GameObject hitEffectPrefab;
    public GameObject bulletHolePrefab;
    public GameObject muzzleFlash;
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public AudioClip emptyFiresound;
    public int maxAmmo = 30;
    public float reloadTime = 2f;
    public float fireRate = 0.2f;
    public bool automatic;
    public float damage = 10f;
    public float recoilForce = 0.5f;
    public float recoilDuration = 0.1f;
    public float recoilResetSpeed = 5f;

    public CollectableData collectableData;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image dotImg;
    public Image equippedWeaponIcon;
    public Text weaponBulletInfo;
    public GameObject inventoryObj;
    public void OnEnable()
    {
        SignalManager.Instance?.AddListener<OnLookAtInteractable>(OnLookAtItem);
        SignalManager.Instance?.AddListener<OnUpdateAmmo>(OnShotFire);
        SignalManager.Instance?.AddListener<OnToggleInventory>(OpenInventory);
        SignalManager.Instance.AddListener<OnEquipItem>(OnEquippedItem);
        SignalManager.Instance.AddListener<OnPickedItem>(OnPickItem);
        SignalManager.Instance.AddListener<OnDroppedItem>(OnDropItem);
    }
    public void OnDisable()
    {
        SignalManager.Instance?.RemoveListener<OnLookAtInteractable>(OnLookAtItem);
        SignalManager.Instance?.RemoveListener<OnUpdateAmmo>(OnShotFire);
        SignalManager.Instance?.RemoveListener<OnToggleInventory>(OpenInventory);
        SignalManager.Instance.RemoveListener<OnEquipItem>(OnEquippedItem);
        SignalManager.Instance.RemoveListener<OnPickedItem>(OnPickItem);
        SignalManager.Instance.RemoveListener<OnDroppedItem>(OnDropItem);
    }

    private void OnEquippedItem(OnEquipItem signalData)
    {
        UpdateUI(signalData.collectableData);
    }

    private void OnDropItem(OnDroppedItem signalData)
    {
        if (signalData.collectableData.stacKType == StacKType.UniqueItem)
        {
            weaponBulletInfo.text = "";
            equippedWeaponIcon.sprite = null;
        }
        else
        {
            var currentWeapon = WeaponHandler.Instance.currentWeapon;
            if (currentWeapon == null)
            {
                weaponBulletInfo.text = "";
                equippedWeaponIcon.sprite = null;
            }
            else
            {
                var inBag = InventoryManager.Instance.GetItemType(currentWeapon.weaponData.ammoType);
                weaponBulletInfo.text = currentWeapon.currentAmmo + "/" + (inBag != null ? inBag.quantity.ToString() : "0");
            }
        }
    }

    private void OnPickItem(OnPickedItem signalData)
    {
        UpdateUI(signalData.collectableData);
    }

    private void UpdateUI(CollectableData collectableData)
    {
        if (collectableData.stacKType == StacKType.UniqueItem)
        {
            equippedWeaponIcon.sprite = ItemResourceHandler.Instance.ResourceHandler.GetItemResource(collectableData.itemType).itemIcon;

        }
        var currentWeapon = WeaponHandler.Instance.currentWeapon;
        if (currentWeapon == null)
        {
            weaponBulletInfo.text = "";
            equippedWeaponIcon.sprite = null;
        }
        else
        {
            var inBag = InventoryManager.Instance.GetItemType(currentWeapon.weaponData.ammoType);
            weaponBulletInfo.text = currentWeapon.currentAmmo + "/" + (inBag != null ? inBag.quantity.ToString() : "0");
        }
    }
    private void OpenInventory(OnToggleInventory signalData)
    {
        inventoryObj.SetActive(signalData.val);
    }

    private void OnShotFire(OnUpdateAmmo signalData)
    {
        weaponBulletInfo.text = signalData.remainingBullet + "/" + signalData.availableInInventory;
    }

    private void OnLookAtItem(OnLookAtInteractable signalData)
    {
        dotImg.color = signalData.isLookingAt ? Color.green : Color.red;
    }
}

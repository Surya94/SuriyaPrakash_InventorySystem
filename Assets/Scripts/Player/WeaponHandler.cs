using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponHandler : Singleton<WeaponHandler>
{
    public List<Weapons> allWeapons;
    public GunController currentWeapon;

    public void OnEnable()
    {
        SignalManager.Instance.AddListener<OnPickedItem>(OnPickItem);
        SignalManager.Instance.AddListener<OnDroppedItem>(OnDropItem);
    }

    public void OnDisable()
    {
        SignalManager.Instance.RemoveListener<OnPickedItem>(OnPickItem);
        SignalManager.Instance.RemoveListener<OnDroppedItem>(OnDropItem);

    }

    private void OnDropItem(OnDroppedItem signalData)
    {
        if (signalData.collectableData.stacKType == StacKType.UniqueItem)
        {
            currentWeapon = null;
            foreach (var item in allWeapons)
            {
                item.weaponObject.gameObject.SetActive(false);
            }
        }
    }

    private void OnPickItem(OnPickedItem signalData)
    {
        if (signalData.collectableData.stacKType == StacKType.UniqueItem)
        {
            var weaponType = allWeapons.Select(x => x.itemType).ToList();
            foreach (var item in allWeapons)
            {
                if (item.itemType == signalData.collectableData.itemType)
                {
                    item.weaponObject.gameObject.SetActive(true);
                    currentWeapon = item.weaponObject;
                    currentWeapon.currentAmmo = signalData.collectableData.ammoLeft;
                }
                else
                    item.weaponObject.gameObject.SetActive(false);

            }
            SignalManager.Instance.DispatchSignal(new OnEquipItem() { collectableData = signalData.collectableData });
        }
    }

    public void DropItem()
    {
        if (currentWeapon == null)
            return;

        var selectedType = ItemResourceHandler.Instance.ResourceHandler.GetItemResource(currentWeapon.weaponData.collectableData.itemType);
        var newDorpItem = Instantiate(selectedType.PickableObj.gameObject, Camera.main.transform.position, Quaternion.identity);
        newDorpItem.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 10, ForceMode.Impulse);
        newDorpItem.GetComponent<IInteractable>().CollectableData = new CollectableData()
        {
            itemName = currentWeapon.weaponData.collectableData.itemName,
            itemDesc = currentWeapon.weaponData.collectableData.itemDesc,
            itemType = currentWeapon.weaponData.collectableData.itemType,
            itemInteractTxt = currentWeapon.weaponData.collectableData.itemInteractTxt,
            quantity = currentWeapon.weaponData.collectableData.quantity,
            stacKType = currentWeapon.weaponData.collectableData.stacKType,
            ammoLeft = currentWeapon.currentAmmo,
        };
        InventoryManager.Instance.RemoveItem(currentWeapon.weaponData.collectableData.itemType);
        SignalManager.Instance.DispatchSignal(new OnDroppedItem() { collectableData = currentWeapon.weaponData.collectableData });
    }
}
[System.Serializable]
public class Weapons
{
    public ItemType itemType;
    public GunController weaponObject;
}

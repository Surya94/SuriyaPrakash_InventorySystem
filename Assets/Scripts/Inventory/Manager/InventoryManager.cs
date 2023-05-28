using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public Dictionary<ItemType, CollectableData> inventoryData;
    public void AddItem(CollectableData data)
    {
        if (inventoryData == null)
            inventoryData = new Dictionary<ItemType, CollectableData>();

        if (inventoryData.ContainsKey(data.itemType))
        {
            if (data.stacKType == StacKType.Stackable)
            {
                inventoryData[data.itemType].quantity += data.quantity;
            }
            else
            {
                WeaponHandler.Instance.DropItem();
                inventoryData[data.itemType] = data;
            }
        }
        else
        {
            if (data.stacKType == StacKType.UniqueItem)
            {
                WeaponHandler.Instance.DropItem();
            }
            inventoryData[data.itemType] = data;
        }
    }

    public CollectableData GetItemType(ItemType itemType)
    {
        if(inventoryData==null)
            return null;

        if (inventoryData.ContainsKey(itemType))
        {
            return inventoryData[itemType];
        }
        return null;
    }

    public void RemoveItem(ItemType itemType)
    {
        if (inventoryData.ContainsKey(itemType))
        {
            inventoryData.Remove(itemType);
        }
    }
    public void ForceCheckData()
    {
        foreach (var item in inventoryData)
        {
            if (item.Value.quantity <= 0)
            {
                inventoryData.Remove(item.Key);
            }
        }
    }
   
}

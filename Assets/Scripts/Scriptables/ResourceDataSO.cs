using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceDataSO", menuName = "ScriptableObjects/ResourceDataSO", order = 1)]
public class ResourceDataSO : ScriptableObject
{
    public List<ItemResource> itemResources;

    public ItemResource GetItemResource(ItemType item)
    {
        if (itemResources == null)
            return null;
        return itemResources.Find(x => x.itemType == item);
    }
}
[System.Serializable]
public class ItemResource
{
    public ItemType itemType;
    public Sprite itemIcon;
    public Interactables PickableObj;
    
}

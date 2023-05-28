using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour, IInteractable
{
    [SerializeField]
    private CollectableData collectableData;

    public bool isPicked;
    public CollectableData CollectableData
    {
        get => collectableData;
        set => collectableData = value;
    }

    public virtual void OnInteract()
    {
        if (isPicked)
            return;

        isPicked = true;
        InventoryManager.Instance.AddItem(collectableData);
        SignalManager.Instance.DispatchSignal(new OnPickedItem() {collectableData= CollectableData });
        Destroy(gameObject);
    }
}

public enum StacKType { UniqueItem, Stackable }
public enum ItemType { Consumable, PistolAmmo, RifleAmmo, MedKit, Pistol, Rifle }

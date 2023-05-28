using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Text quantity;
    public CollectableData collectableData;

    public void Init(CollectableData data)
    {
        collectableData = data;
        GenerateUI();
    }

    private void GenerateUI()
    {
        if (collectableData == null)
            return;

        var data = ItemResourceHandler.Instance.ResourceHandler.GetItemResource(collectableData.itemType);
        if (data != null)
        {
            icon.sprite = data.itemIcon;
        }
        quantity.gameObject.SetActive(collectableData.quantity > 0);
        quantity.text = "x" + collectableData.quantity.ToString();
    }

    public void OnClick()
    {
        SignalManager.Instance?.DispatchSignal(new OnSelectItem() { slotData = this });
    }
}

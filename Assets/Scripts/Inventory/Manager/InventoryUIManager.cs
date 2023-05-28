using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public Image previewIcon;
    public Text previewTitle;
    public Text previewDesc;
    public GameObject previewPanel;
    public Button useBtn;
    public Button dropBtn;
    public Transform container;
    public InventorySlot slotObj;
    public List<InventorySlot> allSlotObjs;
    public InventorySlot SelectcedObj;
    public void OnEnable()
    {
        SignalManager.Instance?.AddListener<OnSelectItem>(OnSelectedItem);
        var data = InventoryManager.Instance.inventoryData;

        if (data == null)
            return;
        previewPanel.SetActive(false);
        foreach (var item in allSlotObjs)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in data)
        {
            var slot = GetUnusedSlot();
            slot.gameObject.SetActive(true);
            slot.Init(item.Value);
        }

    }

    public void OnDisable()
    {
        SignalManager.Instance?.RemoveListener<OnSelectItem>(OnSelectedItem);
        foreach (var item in allSlotObjs)
        {
            item.gameObject.SetActive(false);
        }
    }

    private void OnSelectedItem(OnSelectItem signalData)
    {
        SelectcedObj = signalData.slotData;
        SetPreview();
    }

    private void SetPreview()
    {
        previewPanel.SetActive(true);
        previewIcon.sprite = SelectcedObj.icon.sprite;
        previewTitle.text = SelectcedObj.collectableData.itemName;
        previewDesc.text = SelectcedObj.collectableData.itemDesc;
        useBtn.gameObject.SetActive(SelectcedObj.collectableData.itemType == ItemType.Consumable);

    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)|| Input.GetKeyUp(KeyCode.I))
        {
            SignalManager.Instance.DispatchSignal(new OnToggleInventory() { val = false });
        }
    }

    public InventorySlot GetUnusedSlot()
    {
        if (allSlotObjs == null)
            allSlotObjs = new List<InventorySlot>();

        foreach (var item in allSlotObjs)
        {
            if (!item.gameObject.activeSelf)
                return item;
        }
        var obj = Instantiate(slotObj.gameObject, container);
        var slotData = obj.GetComponent<InventorySlot>();
        allSlotObjs.Add(slotData);
        return slotData;
    }

    public void DropItem()
    {
        if (SelectcedObj.collectableData.stacKType == StacKType.UniqueItem)
        {
            WeaponHandler.Instance.DropItem();
            SelectcedObj.gameObject.SetActive(false);
            previewPanel.gameObject.SetActive(false);
            return;
        }
        var selectedType = ItemResourceHandler.Instance.ResourceHandler.GetItemResource(SelectcedObj.collectableData.itemType);
        var newDorpItem = Instantiate(selectedType.PickableObj.gameObject, Camera.main.transform.position, Quaternion.identity);
        newDorpItem.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 10, ForceMode.Impulse);
        newDorpItem.GetComponent<IInteractable>().CollectableData = new CollectableData()
        {
            itemName = SelectcedObj.collectableData.itemName,
            itemDesc = SelectcedObj.collectableData.itemDesc,
            itemType = SelectcedObj.collectableData.itemType,
            itemInteractTxt = SelectcedObj.collectableData.itemInteractTxt,
            quantity = SelectcedObj.collectableData.quantity,
            stacKType = SelectcedObj.collectableData.stacKType
        };
        InventoryManager.Instance.RemoveItem(SelectcedObj.collectableData.itemType);
        SelectcedObj.gameObject.SetActive(false);
        previewPanel.gameObject.SetActive(false);
        SignalManager.Instance.DispatchSignal(new OnDroppedItem() { collectableData = SelectcedObj.collectableData });
    }
}

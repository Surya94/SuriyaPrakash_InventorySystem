using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpHintManager : MonoBehaviour
{
    public const string pickUpHint = "Press E to {0}";
    public GameObject objectToEnable;
    public Text hitText;
    public void OnEnable()
    {
        SignalManager.Instance?.AddListener<OnLookAtInteractable>(OnLookAtItem);
    }
    public void OnDisable()
    {
        SignalManager.Instance?.RemoveListener<OnLookAtInteractable>(OnLookAtItem);
    }

    private void OnLookAtItem(OnLookAtInteractable signalData)
    {
        if (objectToEnable != null)
            objectToEnable.SetActive(signalData.isLookingAt);
        if (hitText != null &&  signalData.collectableData!=null)
            hitText.text = string.Format(pickUpHint, signalData.collectableData.itemInteractTxt);
    }
}

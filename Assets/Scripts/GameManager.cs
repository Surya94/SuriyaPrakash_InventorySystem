using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool IsinPauseMenu;

    public void OnEnable()
    {
        SignalManager.Instance?.AddListener<OnToggleInventory>(OpenInventory);
    }
    public void OnDisable()
    {
        SignalManager.Instance?.RemoveListener<OnToggleInventory>(OpenInventory);
    }

    private void OpenInventory(OnToggleInventory signalData)
    {
        IsinPauseMenu = signalData.val;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            SignalManager.Instance.DispatchSignal(new OnToggleInventory() { val = true });
        }
    }
}
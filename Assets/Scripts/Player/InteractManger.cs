using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManger : MonoBehaviour
{
    public int MaxRange;
    public LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, MaxRange, layerMask))
        {
            var item = hit.transform.GetComponent<IInteractable>();

            SignalManager.Instance?.DispatchSignal(new OnLookAtInteractable()
            {
                isLookingAt = true,
                collectableData=item?.CollectableData
            });

            if (Input.GetKeyUp(KeyCode.E))
            {

                if (item != null)
                    item.OnInteract();
            }
        }
        else
        {
            SignalManager.Instance?.DispatchSignal(new OnLookAtInteractable() { isLookingAt = false });
        }
    }
}

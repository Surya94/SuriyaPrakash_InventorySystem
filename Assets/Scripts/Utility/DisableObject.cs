using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObject : MonoBehaviour
{
    public float waitTime = 2f;
    private void OnEnable()
    {
        Invoke("DisableObj", waitTime);
    }

    private void DisableObj()
    {
        gameObject.SetActive(false);
    }
}

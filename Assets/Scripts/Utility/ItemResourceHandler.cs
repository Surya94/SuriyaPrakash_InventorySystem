using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemResourceHandler : Singleton<ItemResourceHandler>
{
    private ResourceDataSO resourceHandler;
    public ResourceDataSO ResourceHandler
    {
        get
        {
            if (resourceHandler == null)
                resourceHandler = Resources.Load<ResourceDataSO>("ResourceDataSO");
            return resourceHandler;
        }
    }
}

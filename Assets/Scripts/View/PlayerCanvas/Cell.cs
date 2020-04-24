using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;

public class Cell : MonoBehaviour
{
    public Fabric fabric = null;
    PlayerCanvas parent;

    private void Start()
    {
        parent = GetComponentInParent<PlayerCanvas>();
    }

    private void AdaptMode()
    {
        switch (parent.Mode)
        {
            case ProductionMode.PROD:
                break;
            case ProductionMode.BUY:
                break;
            case ProductionMode.UPGRADE:
                break;
        }
    }

    private void Update()
    {
        AdaptMode();
    }

}

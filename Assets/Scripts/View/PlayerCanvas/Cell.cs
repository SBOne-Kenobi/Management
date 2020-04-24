using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;

public class Cell : MonoBehaviour
{
    public Fabric fabric = null;
    PlayerCanvas parent;

    public GameObject Production;
    public GameObject Upgrade;
    public GameObject Buy;

    private void Start()
    {
        parent = GetComponentInParent<PlayerCanvas>();
    }

    private void AdaptMode()
    {
        switch (parent.Mode)
        {
            case ProductionMode.PROD:
                Production.SetActive(true);
                Upgrade.SetActive(false);
                Buy.SetActive(false);
                break;
            case ProductionMode.BUY:
                Production.SetActive(false);
                Upgrade.SetActive(false);
                Buy.SetActive(true);
                break;
            case ProductionMode.UPGRADE:
                Production.SetActive(false);
                Upgrade.SetActive(true);
                Buy.SetActive(false);
                break;
        }
    }

    private void AdaptFabric()
    {
        
    }

    private void Update()
    {
        AdaptMode();
        if (Production.activeSelf)
            AdaptFabric();
    }

}

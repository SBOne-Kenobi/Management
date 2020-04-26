using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;

public class ProdFabric : MonoBehaviour
{
    public Fabric Fabric => GetComponentInParent<Cell>().Fabric;
    public GameObject Building;
    public GameObject Simple;
    public GameObject Upgrade;
    public GameObject ProductionControl;

    private void Awake()
    {
        ProductionControl.SetActive(true);
    }

    private void Update()
    {
        if (Fabric == null)
        {
            Building.SetActive(false);
            Simple.SetActive(false);
            Upgrade.SetActive(false);
        }
        else if (Fabric.BuildTime > 0)
        {
            Building.SetActive(true);
            Simple.SetActive(false);
            Upgrade.SetActive(false);
        }
        else if (Fabric is AutoFabric)
        {
            Building.SetActive(false);
            Simple.SetActive(false);
            Upgrade.SetActive(true);
        }
        else if (Fabric is SimpleFabric)
        {
            Building.SetActive(false);
            Simple.SetActive(true);
            Upgrade.SetActive(false);
        }
    }
}

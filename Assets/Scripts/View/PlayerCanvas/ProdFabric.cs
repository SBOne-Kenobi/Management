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
    public GameObject Progress;

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
            Progress.SetActive(false);
        }
        else if (Fabric.BuildTime > 0)
        {
            Building.SetActive(true);
            Simple.SetActive(false);
            Upgrade.SetActive(false);
            Progress.SetActive(true);
            Progress.GetComponent<Progress>().UpdateCur(Fabric.StartBuildTime - Fabric.BuildTime, Fabric.StartBuildTime);
        }
        else if (Fabric is AutomatedFabric)
        {
            Building.SetActive(false);
            Simple.SetActive(false);
            Progress.SetActive(false);
            Upgrade.SetActive(true);
        }
        else if (Fabric is SimpleFabric)
        {
            Building.SetActive(false);
            Simple.SetActive(true);
            Upgrade.SetActive(false);
            if ((Fabric as SimpleFabric).UpgradeTime > 0)
            {
                Progress.SetActive(true);
                Progress.GetComponent<Progress>().UpdateCur(
                        (Fabric as SimpleFabric).StartUpgradeTime - (Fabric as SimpleFabric).UpgradeTime,
                        (Fabric as SimpleFabric).StartUpgradeTime
                    );
            } else
            {
                Progress.SetActive(false);
            }
        }
    }
}

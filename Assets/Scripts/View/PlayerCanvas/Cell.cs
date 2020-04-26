using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;

public class Cell : ProdElement
{
    public Director director;
    public int pos;
    public Fabric Fabric
    {
        get
        {
            if (director == null)
                return null;
            return director.Fabrics[pos];
        }
    }
    public GameObject Production, Upgrade, Buy;

    protected override void SetBuyMode(bool owner)
    {
        Production.SetActive(false);
        Upgrade.SetActive(false);
        Buy.SetActive(true);
    }

    protected override void SetProdMode(bool owner)
    {
        Production.SetActive(true);
        Upgrade.SetActive(false);
        Buy.SetActive(false);
    }

    protected override void SetUpgradeMode(bool owner)
    {
        Production.SetActive(false);
        Upgrade.SetActive(Fabric != null);
        Buy.SetActive(false);
    }
}

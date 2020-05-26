using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Management;

public class ProdController : ProdElement
{
    Fabric Fabric => GetComponentInParent<ProdFabric>().Fabric;
    Transform Number;

    protected override bool NeededState()
    {
        var par = GetComponentInParent<PlayerCanvas>().Controller.game;
        return par != null && par.State.CurrentState == Management.GameState.Production;
    }

    void Start()
    {
        Number = transform.Find("Number");
    }

    public void Add()
    {
        if (Fabric != null)
        {
            Fabric.Owner.AddMaterial(Fabric.Owner.GetIndex(Fabric));
        }
    }

    public void Reduce()
    {
        if (Fabric != null)
        {
            Fabric.Owner.RemoveMaterial(Fabric.Owner.GetIndex(Fabric));
        }
    }

    protected override void SetProdMode(bool owner)
    {
        gameObject.SetActive(owner && Fabric != null && Fabric.BuildTime == 0);
    }

    protected override void SetBuyMode(bool owner)
    {
        gameObject.SetActive(false);
    }

    protected override void SetUpgradeMode(bool owner)
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Fabric != null)
            Number.GetComponent<Text>().text = Fabric.CurrentMat.ToString();
        else
            Number.GetComponent<Text>().text = "-";
    }
}

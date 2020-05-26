using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ProdElement : MonoBehaviour
{
    protected abstract void SetProdMode(bool owner);
    protected abstract void SetBuyMode(bool owner);
    protected abstract void SetUpgradeMode(bool owner);

    virtual protected bool NeededState()
    {
        return true;
    }

    void Awake()
    {
        var par = GetComponentInParent<PlayerCanvas>();
        par.prods.Add(this);
        AdaptMode(par.Mode, par.IsOwner);
    }

    public void AdaptMode(ProductionMode mode, bool owner)
    {
        if (!NeededState())
        {
            gameObject.SetActive(false);
            return;
        }
        switch(mode)
        {
            case ProductionMode.PROD:
                SetProdMode(owner);
                break;
            case ProductionMode.BUY:
                SetBuyMode(owner);
                break;
            case ProductionMode.UPGRADE:
                SetUpgradeMode(owner);
                break;
        }
    }

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : ProdElement
{
    protected override bool NeededState()
    {
        var par = GetComponentInParent<PlayerCanvas>().Controller.game;
        return par != null && par.State.CurrentState == Management.GameState.BuildUpgrade;
    }

    protected override void SetBuyMode(bool owner)
    {
        gameObject.SetActive(owner);
    }

    protected override void SetProdMode(bool owner)
    {
        gameObject.SetActive(owner);
    }

    protected override void SetUpgradeMode(bool owner)
    {
        gameObject.SetActive(owner);
    }
}

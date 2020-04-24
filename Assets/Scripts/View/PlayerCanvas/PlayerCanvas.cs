using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProductionMode
{
    PROD = 0,
    BUY = 1,
    UPGRADE = 2,
}

public class PlayerCanvas : MonoBehaviour
{
    public Player player;
    private Switcher switcher;
    public bool IsOwner => switcher.GetPlayer() == player;
    public ProductionMode Mode { get; private set; }

    private void Awake()
    {
        switcher = FindObjectOfType<Switcher>();
        ProdMode();
    }

    public void ProdMode()
    {
        Mode = ProductionMode.PROD;
    }

    public void BuyMode()
    {
        Mode = ProductionMode.BUY;
    }

    public void UpgradeMode()
    {
        Mode = ProductionMode.UPGRADE;
    }
}

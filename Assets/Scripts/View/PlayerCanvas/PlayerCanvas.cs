using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum ProductionMode
{
    PRODUCTION = 0,
    BUY = 1,
    UPGRADE = 2,
}

public class PlayerCanvas : MonoBehaviour
{
    public PlayerControl player;
    public bool IsOwner {
        get
        {
            if (player != null && player.PhotonView != null)
                return player.PhotonView.IsMine;
            return false;
        }
    }
    public ProductionMode Mode { get; private set; } = ProductionMode.PRODUCTION;
    public List<ProdElement> prods = new List<ProdElement>();
    public Controller Controller { get; private set; }

    private void Awake()
    {
        ProdMode();
        Controller = FindObjectOfType<Controller>();
    }

    private void Update()
    {
        AdaptMode();
    }

    private void AdaptMode()
    {
        foreach (ProdElement elem in prods)
            elem.AdaptMode(Mode, IsOwner);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        ProdMode();
    }

    public void ProdMode()
    {  
        Mode = ProductionMode.PRODUCTION;
    }

    public void BuyMode()
    {
        if (Mode == ProductionMode.BUY)
            ProdMode();
        else
            Mode = ProductionMode.BUY;
    }

    public void UpgradeMode()
    {
        if (Mode == ProductionMode.UPGRADE)
            ProdMode();
        else
            Mode = ProductionMode.UPGRADE;
    }
}

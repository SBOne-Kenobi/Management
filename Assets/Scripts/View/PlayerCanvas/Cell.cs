using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun;

public class Cell : ProdElement
{
    public PlayerControl player;
    public Director director;
    private Controller controller;
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

    private void Start()
    {
        controller = FindObjectOfType<Controller>();
    }

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
        Buy.SetActive(false);
        bool tof = Fabric != null && Fabric.BuildTime == 0 && (Fabric is SimpleFabric);
        if (tof)
        {
            tof = ((Fabric as SimpleFabric).UpgradeTime == -1) && 
                (director.Money >= (Fabric as SimpleFabric).UpgradePrice);
        }
        Upgrade.SetActive(tof);
    }

    void SendMessage(byte id)
    {
        controller.SendFabricState(id, player.PhotonView.Owner.ActorNumber, pos, director.Money);
    }

    public void BuyFabric()
    {
        if (director != null)
        {
            SendMessage(21);
            director.BuyFabric(pos);
        }
    }

    public void SellFabric()
    {
        if (director != null)
        {
            SendMessage(22);
            director.SellFabric(pos);
        }
    }

    public void UpgradeFabric()
    {
        if (director != null)
        {
            SendMessage(23);
            director.UpgradeFabric(pos);
        }
    }
}

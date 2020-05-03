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
    public ProductionMode Mode { get; private set; } = ProductionMode.PROD;
    public List<ProdElement> prods = new List<ProdElement>();
    public Controller Controller { get; private set; }

    private void Awake()
    {
        switcher = FindObjectOfType<Switcher>();
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
        Mode = ProductionMode.PROD;
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

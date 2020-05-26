using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ToolBar : PlayerInfo
{
    private Transform ReadyButton;

    [SerializeField]
    private Controller Controller;
    [SerializeField]
    private GameObject FixCosts;

    override protected void Awake()
    {
        base.Awake();
        ReadyButton = transform.Find("ReadyButton");
    }

    private void Start()
    {
        player = Controller.MinePlayer;
    }

    override protected void Update()
    {
        base.Update();
        ReadyButton.GetComponent<Button>().interactable = player.Mutable;
        FixCosts.GetComponent<Text>().text = "Fix costs: " + player.Director.FixCosts;
    }

    public void GetReady()
    {
        player.GetReady();
    }
}

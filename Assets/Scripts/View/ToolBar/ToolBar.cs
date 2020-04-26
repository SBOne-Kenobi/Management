using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBar : PlayerInfo
{
    public Switcher switcher;
    private Transform Timer;
    private Transform ReadyButton;

    override protected void Awake()
    {
        base.Awake();
        Timer = transform.Find("Timer");
        ReadyButton = transform.Find("ReadyButton");
    }

    override protected void Update()
    {
        player = switcher.GetPlayer();
        base.Update();
        ReadyButton.GetComponent<Button>().interactable = player.Mutable;
    }

    public void GetReady()
    {
        switcher.GetPlayer().GetReady();
    }
}

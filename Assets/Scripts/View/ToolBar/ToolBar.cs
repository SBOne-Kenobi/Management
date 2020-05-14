using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ToolBar : PlayerInfo
{
    private Transform Timer;
    private Transform ReadyButton;

    [SerializeField]
    private Controller controller;

    override protected void Awake()
    {
        base.Awake();
        Timer = transform.Find("Timer");
        ReadyButton = transform.Find("ReadyButton");
    }

    private void Start()
    {
        player = controller.MinePlayer;
    }

    override protected void Update()
    {
        base.Update();
        ReadyButton.GetComponent<Button>().interactable = player.Mutable;
    }

    public void GetReady()
    {
        player.GetReady();
    }
}

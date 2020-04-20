using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBar : MonoBehaviour
{

    public Switcher switcher;
    private Transform Prod;
    private Transform Mat;
    private Transform Mon;
    private Transform Timer;
    private Transform ReadyButton;

    private void Awake()
    {
        Prod = transform.Find("Prod/ProdVal");
        Timer = transform.Find("Timer");
        Mat = transform.Find("Mat/MatVal");
        Mon = transform.Find("Mon/MonVal");
        ReadyButton = transform.Find("ReadyButton");
    }

    // Update is called once per frame
    void Update()
    {
        Prod.GetComponent<Text>().text = switcher.GetPlayer().Director.Product.ToString();
        Mat.GetComponent<Text>().text = switcher.GetPlayer().Director.Materials.ToString();
        Mon.GetComponent<Text>().text = switcher.GetPlayer().Director.Money.ToString();
        ReadyButton.GetComponent<Button>().interactable = switcher.GetPlayer().Mutable;
    }

    public void GetReady()
    {
        switcher.GetPlayer().GetReady();
    }
}

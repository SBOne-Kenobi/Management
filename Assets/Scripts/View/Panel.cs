using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Panel : MonoBehaviour
{
    private Transform State;
    private Transform Mat;
    private Transform Prod;
    private Transform MinMat;
    private Transform MaxProd;
    private Transform Month;
    private Controller controller;

    private void Awake()
    {
        State = transform.Find("State");
        Mat = transform.Find("Mat");
        Prod = transform.Find("Prod");
        MinMat = transform.Find("MinMat");
        MaxProd = transform.Find("MaxProd");
        Month = transform.Find("Month");
        controller = FindObjectOfType<Controller>();
    }

    private void Update()
    {
        State.GetComponent<Text>().text = "Current state: " + controller.game.ToString();
        Mat.GetComponent<Text>().text = "M: " + (Math.Floor(controller.game.State.Bank.GetInfo.UMat * controller.game.Alive)).ToString();
        Prod.GetComponent<Text>().text = "P: " + (Math.Floor(controller.game.State.Bank.GetInfo.UProd * controller.game.Alive)).ToString();
        MinMat.GetComponent<Text>().text = "≥$: " + controller.game.State.Bank.GetInfo.MinPrice.ToString();
        MaxProd.GetComponent<Text>().text = "≤$: " + controller.game.State.Bank.GetInfo.MaxPrice.ToString();
        Month.GetComponent<Text>().text = "Month: " + controller.game.State.CurrentMonth.ToString();
    }
}

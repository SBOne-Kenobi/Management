﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Management;

public class Request : MonoBehaviour
{

    private Controller controller;
    public Switcher switcher;
    public GameObject input;
    public GameObject button;

    private void Awake()
    {
        controller = FindObjectOfType<Controller>();
    }

    void Start()
    {
        input.SetActive(false);
        button.SetActive(true);
    }

    public void FixPrice()
    {
        Transform amount = input.transform.Find("InputAmount");
        Transform price = input.transform.Find("InputPrice");
        int pr, am;
        if (price.GetComponent<InputField>().text.Length > 0 && price.GetComponent<InputField>().text[0] == '-')
        {
            pr = 0;
        }
        else
        {
            pr = Convert.ToInt32("0" + price.GetComponent<InputField>().text);
        }
        if (amount.GetComponent<InputField>().text.Length > 0 && amount.GetComponent<InputField>().text[0] == '-')
        {
            am = 0;
        }
        else
        {
            am = Convert.ToInt32("0" + amount.GetComponent<InputField>().text);
        }

        if (controller.game.State.CurrentState == GameState.ProdRequest)
        {
            am = Math.Min(am, switcher.GetPlayer().Director.Product);
        }
        else if (controller.game.State.CurrentState == GameState.MatRequest)
        {
            pr = Math.Min(pr, switcher.GetPlayer().Director.Money / Math.Max(am, 1));
        }
        price.GetComponent<InputField>().text = pr.ToString();
        amount.GetComponent<InputField>().text = am.ToString();
    }

    private bool TimeToDo
    {
        get
        {
            return (controller.game.State.CurrentState == GameState.MatRequest ||
                controller.game.State.CurrentState == GameState.ProdRequest);
        }
    }
    void Update()
    {
        if (!switcher.GetPlayer().IsReady && TimeToDo)
        {
            button.SetActive(false);
            input.SetActive(true);
        }
        else
        {
            if (input.activeSelf)
            {
                input.transform.Find("InputAmount").GetComponent<InputField>().text = "";
                input.transform.Find("InputPrice").GetComponent<InputField>().text = "";
            }
            input.SetActive(false);
            button.SetActive(true);
        }
    }

    public void MakeRequest()
    {
        if (TimeToDo)
        {
            Transform amount = input.transform.Find("InputAmount");
            Transform price = input.transform.Find("InputPrice");
            int pr = Convert.ToInt32("0" + price.GetComponent<InputField>().text);
            int am = Convert.ToInt32("0" + amount.GetComponent<InputField>().text);
            if (controller.game.State.CurrentState == GameState.MatRequest)
            {
                controller.AddRequestOfMat(pr, am, switcher.GetPlayer());
            }
            else
            {
                controller.AddRequestOfProd(pr, am, switcher.GetPlayer());
            }
        }
    }
}

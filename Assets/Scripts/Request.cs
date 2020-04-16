using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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
    }

    void Update()
    {
        if (!switcher.GetPlayer().IsReady && (controller.game.State == 1 || controller.game.State == 3))
        {
            button.SetActive(false);
            input.SetActive(true);
        }
        else
        {
            if (controller.game.State == 1 || controller.game.State == 3)
            {
                MakeRequest();
            }
            input.SetActive(false);
            button.SetActive(true);
        }
    }

    public void MakeRequest()
    {
        Transform amount = input.transform.Find("InputAmount");
        Transform price = input.transform.Find("InputPrice");
        if (controller.game.State == 1)
        {
            controller.AddRequestOfMat(Convert.ToInt32(price.GetComponent<InputField>().text),
                Convert.ToInt32(amount.GetComponent<InputField>().text), switcher.GetPlayer());
        }
        else
        {
            controller.AddRequestOfProd(Convert.ToInt32(price.GetComponent<InputField>().text),
                Convert.ToInt32(amount.GetComponent<InputField>().text), switcher.GetPlayer());
        }
    }
}

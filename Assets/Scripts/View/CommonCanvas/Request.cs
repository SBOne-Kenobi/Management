using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Management;
using System.Linq;

public class Request : MonoBehaviour
{

    private Controller controller;
    public GameObject input;
    private PlayerControl Player;

    private void Awake()
    {
        controller = FindObjectOfType<Controller>();
        Player = controller.Players.First(p => p.PhotonView.IsMine);
    }

    void Start()
    {
        input.SetActive(false);
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
            am = Math.Min(am, Player.Director.Product);
        }
        else if (controller.game.State.CurrentState == GameState.MatRequest)
        {
            pr = Math.Min(pr, Player.Director.Money / Math.Max(am, 1));
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
        if (!Player.IsReady && TimeToDo)
        {
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
                controller.AddRequestOfMat(pr, am, Player);
            }
            else
            {
                controller.AddRequestOfProd(pr, am, Player);
            }
        }
    }
}

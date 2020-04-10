using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    public Controller controller;

    private void Update()
    {
        GetComponent<Text>().text = "Month " + controller.game.Month.ToString() + "\n" +
            "Price level: " + controller.game.Bank.PriceLevel.ToString();
    }
}

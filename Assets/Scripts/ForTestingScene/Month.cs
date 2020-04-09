using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Month : MonoBehaviour
{
    public Controller controller;

    private void Update()
    {
        GetComponent<Text>().text = "Month " + controller.game.Month.ToString();
    }
}

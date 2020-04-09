using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class State : MonoBehaviour
{

    public Controller controller;

    // Start is called before the first frame update
    void Start()
    {
        controller.InitGame();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = "State: " + controller.game.State.ToString();
        if (!controller.IsReadyToGoNext)
            GetComponent<Text>().text = GetComponent<Text>().text + "\n Wait Players";
    }
}

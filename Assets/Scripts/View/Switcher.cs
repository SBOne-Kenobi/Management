using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Switcher : MonoBehaviour
{

    public GameObject turnLabel;
    private Controller controller;
    private int turn;

    public Player GetPlayer()
    {
        return controller.players[turn];
    }

    private void Awake()
    {
        controller = FindObjectOfType<Controller>();
        turn = 0;
    }

    public void Switch()
    {
        turn = (turn + 1) % controller.players.Count;
    }

    // Update is called once per frame
    void Update()
    {
        turnLabel.GetComponent<Text>().text = GetPlayer().Name + " turn";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyButton : MonoBehaviour
{
    public Player player;

    // Update is called once per frame
    void Update()
    {
        GetComponent<Button>().interactable = player.Mutable;
    }
}

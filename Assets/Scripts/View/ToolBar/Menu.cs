using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject MenuBuntton;

    public void Clicked()
    {
        Application.Quit();
    }
}

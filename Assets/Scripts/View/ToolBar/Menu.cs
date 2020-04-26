using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject MenuBuntton;
    public GameObject ReadyButton;
    public GameObject MenuCanvas;
    public GameObject CommonCanvas;
    public static bool isPaused = false;

    public void Clicked()
    {
        //CommonCanvas.SetActive(false);
        ReadyButton.GetComponent<Button>().interactable = false;
        MenuBuntton.GetComponent<Button>().interactable = false;
        MenuCanvas.SetActive(true);

        Time.timeScale = 0f;

        isPaused = true;
    }

    public void Resume()
    {
        MenuCanvas.SetActive(false);
        ReadyButton.GetComponent<Button>().interactable = true;
        MenuBuntton.GetComponent<Button>().interactable = true;
        //CommonCanvas.SetActive(true);

        Time.timeScale = 1f;

        isPaused = false;
    }

    public void Exit()
    {
        Application.Quit();
    }
}

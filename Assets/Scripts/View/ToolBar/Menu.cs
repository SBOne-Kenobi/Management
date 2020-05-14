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
    public static bool isPaused = false;
    [SerializeField]
    private float TimeDelay = 1f;

    public SwitchCanvas Switcher;

    private GameObject CanvasHolder = null; 

    public void Clicked()
    {
        if (Switcher.CurrentCanvas == null)
            CanvasHolder = Switcher.CommonCanvas;
        else
            CanvasHolder = Switcher.CurrentCanvas;
        CanvasHolder.SetActive(false);
        ReadyButton.GetComponent<Button>().interactable = false;
        MenuBuntton.GetComponent<Button>().interactable = false;
        MenuCanvas.SetActive(true);

        Time.timeScale = TimeDelay;

        isPaused = true;
    }

    public void Resume()
    {
        MenuCanvas.SetActive(false);
        ReadyButton.GetComponent<Button>().interactable = true;
        MenuBuntton.GetComponent<Button>().interactable = true;
        CanvasHolder.SetActive(true);
        CanvasHolder = null;
        Time.timeScale = 1f;

        isPaused = false;
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        Photon.Pun.PhotonNetwork.LeaveRoom();
    }
}

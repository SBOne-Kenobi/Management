using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCanvas : MonoBehaviour
{
    public GameObject CommonCanvas;
    private GameObject CurrentCanvas = null;

    public void GoCommon()
    {
        if (CurrentCanvas != null)
        {
            CurrentCanvas.SetActive(false);
            CurrentCanvas = null;
        }
        CommonCanvas.SetActive(true);
    }

    public void GoCanvas(GameObject canvas)
    {
        CurrentCanvas = canvas;
        CommonCanvas.SetActive(false);
        CurrentCanvas.GetComponent<PlayerCanvas>().Activate();
    }
}

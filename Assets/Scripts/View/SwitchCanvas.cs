using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCanvas : MonoBehaviour
{
    public GameObject CommonCanvas;
    public GameObject CurrentCanvas { get; private set; } = null;

    public void GoCommon()
    {
        if (CurrentCanvas != null)
        {
            CurrentCanvas.SetActive(false);
            CurrentCanvas = null;
        }
        CommonCanvas.GetComponent<Canvas>().enabled = true;
        CommonCanvas.GetComponent<GraphicRaycaster>().enabled = true;
        //CommonCanvas.SetActive(true);
    }

    public void GoCanvas(GameObject canvas)
    {
        CurrentCanvas = canvas;
        //CommonCanvas.SetActive(false);
        CommonCanvas.GetComponent<Canvas>().enabled = false;
        CommonCanvas.GetComponent<GraphicRaycaster>().enabled = false;
        CurrentCanvas.GetComponent<PlayerCanvas>().Activate();
    }
}

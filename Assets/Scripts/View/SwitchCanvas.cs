using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCanvas : MonoBehaviour
{
    public Canvas CommonCanvas;
    private Canvas CurrentCanvas = null;

    public void GoCommon()
    {
        if (CurrentCanvas != null)
        {
            CurrentCanvas.enabled = false;
            CurrentCanvas = null;
        }
        CommonCanvas.enabled = true;
    }

    public void GoCanvas(Canvas canvas)
    {
        CurrentCanvas = canvas;
        CommonCanvas.enabled = false;
        CurrentCanvas.enabled = true;
    }
}

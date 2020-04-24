using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    SwitchCanvas switchCanvas;

    private void Start()
    {
        switchCanvas = FindObjectOfType<SwitchCanvas>();
    }

    public void SwitchBack()
    {
        switchCanvas.GoCommon();
    }
}

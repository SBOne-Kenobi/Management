using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Progress : MonoBehaviour
{
    public void UpdateCur(int cur, int max)
    {
        GetComponent<Slider>().maxValue = max;
        GetComponent<Slider>().value = cur;
    }

    public void SetNewProgress(int max)
    {
        UpdateCur(0, max);
    }

    public void UpdateCur(int cur)
    {
        GetComponent<Slider>().value = cur;
    }
}

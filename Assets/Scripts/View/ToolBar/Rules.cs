using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rules : MonoBehaviour
{
    [SerializeField]
    private GameObject Menu;

    public void Open()
    {
        Menu.SetActive(false);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        Menu.SetActive(true);
        gameObject.SetActive(false);
    }
}

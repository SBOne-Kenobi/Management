﻿using System.Collections;
using UnityEngine;
using Management;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Director Director { get; private set; } = new Director();
    public int Order = 0;
    public bool IsReady { get; private set; } = false;
    public bool Mutable { get; private set; } = false;
    public string Name = "Default";

    public void GetReady()
    {
        if (Mutable)
            IsReady = true;
    }

    public void Update()
    {
        if (Director.IsBankrupt)
            Debug.Log(Name + " Bankrupt!");
    }   

    public IEnumerator WaitForReady()
    {
        Mutable = true;
        IsReady = false;
        while (!IsReady)
        {
            //ждем изменения состояния ready 
            yield return null;
        }
        Mutable = false;
    }
}

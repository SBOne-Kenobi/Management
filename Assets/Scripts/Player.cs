using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;

public class Player : MonoBehaviour
{
    public Director director = new Director();
    public bool IsReady = true;

    public void GetReady()
    {
        IsReady = true;
    }

    public void Update()
    {
        
    }

    public IEnumerator WaitForReady()
    {
        IsReady = false;
        while (!IsReady)
        {
            //ждем изменения состояния ready 
            yield return null;
        }
    }
}

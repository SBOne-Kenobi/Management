using System.Collections;
using UnityEngine;
using Management;

public class Player : MonoBehaviour
{
    public Director director = new Director();
    public int Order = 0;
    public bool IsReady = false;
    public bool Mutable = false;

    public void GetReady()
    {
        if (Mutable)
            IsReady = true;
    }

    public void Update()
    {
        
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

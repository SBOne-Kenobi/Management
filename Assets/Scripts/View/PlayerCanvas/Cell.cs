using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;

public class Cell : MonoBehaviour
{
    public Fabric fabric = null;
    PlayerCanvas parent;

    private void Start()
    {
        parent = GetComponentInParent<PlayerCanvas>();
    }

    private void AdaptMode()
    {
        
    }

    private void Update()
    {
        
    }

}

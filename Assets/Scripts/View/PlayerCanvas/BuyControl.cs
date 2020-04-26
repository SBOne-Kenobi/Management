using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyControl : MonoBehaviour
{
    public GameObject Buy;
    public GameObject Cell;
    private void Update()
    {
        bool tof = GetComponentInParent<Cell>().Fabric == null;
        Buy.SetActive(tof);
        Cell.SetActive(!tof);
    }
}

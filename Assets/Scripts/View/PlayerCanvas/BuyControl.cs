using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyControl : MonoBehaviour
{
    public GameObject Buy;
    public GameObject Sell;

    private void Update()
    {
        bool tof = GetComponentInParent<Cell>().Fabric == null;
        Buy.SetActive(tof);
        Sell.SetActive(!tof);
    }
}

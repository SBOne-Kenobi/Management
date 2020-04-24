using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProdElement : MonoBehaviour
{
    PlayerCanvas parent;

    virtual protected void Start()
    {
        parent = GetComponentInParent<PlayerCanvas>();
        enabled = parent.IsOwner;
    }
}

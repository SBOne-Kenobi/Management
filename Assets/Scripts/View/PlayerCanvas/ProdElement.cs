using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProdElement : MonoBehaviour
{
    Player player;
    PlayerCanvas parent;

    private void Start()
    {
        var c = GetComponentsInParent<Canvas>();
        parent = c[c.Length - 1].GetComponent<PlayerCanvas>();
        player = parent.player;
        enabled = parent.IsOwner;
    }
}

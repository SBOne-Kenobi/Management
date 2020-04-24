using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;

public class FabricField : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = GetComponentInParent<PlayerCanvas>().player;
        int cnt = 0;
        foreach (Cell cell in GetComponentsInChildren<Cell>())
        {
            cell.fabric = player.Director.Fabrics[cnt];
            cnt++;
        }
    }
}

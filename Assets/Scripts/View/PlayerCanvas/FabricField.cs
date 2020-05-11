using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;

public class FabricField : MonoBehaviour
{
    PlayerControl player;

    private void Start()
    {
        player = GetComponentInParent<PlayerCanvas>().player;
        int cnt = 0;
        foreach (Cell cell in GetComponentsInChildren<Cell>())
        {
            cell.player = player;
            cell.director = player.Director;
            cell.pos = cnt;
            cnt++;
        }
    }
}

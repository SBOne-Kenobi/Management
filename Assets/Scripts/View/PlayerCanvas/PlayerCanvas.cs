using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    public Player player;
    private Switcher switcher;
    public bool IsOwner => switcher.GetPlayer() == player;

    private void Awake()
    {
        switcher = FindObjectOfType<Switcher>();
    }
}

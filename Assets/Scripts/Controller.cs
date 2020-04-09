using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Management;

public class Controller : MonoBehaviour
{

    public Management.Management game = null;
    public List<Player> players = new List<Player>();
    public bool IsReadyToGoNext = false;

    public void InitGame()
    {
        List<Director> directors = new List<Director>();
        foreach(Player player in players)
            directors.Add(player.director);
        game = new Management.Management(directors);
        IsReadyToGoNext = false;
        StartCoroutine(WaitForAllReady(GoNext));
        Debug.Log("Start Game");
    }

    public void GoNext()
    {
        IsReadyToGoNext = true;
        Debug.Log("Game ready to continue");
    }

    public IEnumerator WaitForAllReady(Action action)
    {
        foreach (Player player in players)
            StartCoroutine(player.WaitForReady());

        Debug.Log("Start players wait");
        bool isReady = false;
        while (!isReady)
        {
            isReady = true;
            players.ForEach(p => { if (!p.IsReady) isReady = false; });
            if (!isReady)
                yield return null;
        }
        Debug.Log("All players ready");
        action();
    }

    public void Update()
    {
        if (game == null)
            return;
        if (IsReadyToGoNext)
        {
            game.NextState();
            IsReadyToGoNext = false;
            if (game.State == 0)
            {
                //changed fabrics, became bankrupts...
                IsReadyToGoNext = true;
            } else if (game.State == 1)
            {
                //update DemandOffer
                StartCoroutine(WaitForAllReady(GoNext));
            } else if (game.State == 2)
            {
                //sum up results of requests of mat.
                StartCoroutine(WaitForAllReady(GoNext));
            } else if (game.State == 3)
            {
                //sum up results of building fabrics.
                StartCoroutine(WaitForAllReady(GoNext));
            } else if (game.State == 4)
            {
                //sum up results of requests of prod.
                IsReadyToGoNext = true;
            }
        }
    }


}

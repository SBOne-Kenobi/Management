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
    public bool AllPlayersReady = false;

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
        game.NextState();
        Debug.Log("Game ready to continue");
    }

    public IEnumerator WaitForAllReady(Action action)
    {
        AllPlayersReady = false;
        foreach (Player player in players)
        {
            if (!player.director.IsBankrupt)
                StartCoroutine(player.WaitForReady());
        }

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
        AllPlayersReady = true;
        action();
    }

    public IEnumerator WaitForFewSecs(Action action)
    {
        Debug.Log("Waiting 1 sec.");
        yield return new WaitForSeconds(1f);
        action();
    }

    public void Update()
    {
        if (game == null)
            return;
        if (IsReadyToGoNext && AllPlayersReady)
        {
            IsReadyToGoNext = false;
            if (game.State == 0)
            {
                //changed fabrics, became bankrupts...
                StartCoroutine(WaitForFewSecs(GoNext));
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
                //sum up results of production.
                StartCoroutine(WaitForAllReady(GoNext));
            } else if (game.State == 4)
            {
                //sum up results of requests of prod.
                StartCoroutine(WaitForFewSecs(GoNext));
            }
        }
    }


}

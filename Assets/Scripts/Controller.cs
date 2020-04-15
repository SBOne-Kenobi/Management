using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Management;

public class Controller : MonoBehaviour
{

    public Management.Management game = null;
    public List<Player> players { get; private set; } = new List<Player>();
    public bool IsReadyToGoNext { get; private set; } = false;
    public bool AllPlayersReady { get; private set; } = false;

    public void AddPlayer(Player player)
    {
        if (players.Contains(player))
            return;
        player.Order = players.Count;
        players.Add(player);
    }

    public int GetPriority(Player player)
    {
        int res = (player.Order - game.DirectorsOrder + players.Count) % players.Count;
        return res;
    }

    public void AddRequestOfMat(int price, int mat, Player player)
    {
        game._requests_of_mat.Add(new Demand(price, mat, GetPriority(player)));
    }

    public void AddRequestOfProd(int price, int prod, Player player)
    {
        game._requests_of_prod.Add(new Offer(price, prod, GetPriority(player)));
    }

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

    public IEnumerator WaitForTime(Action action, float time)
    {
        Debug.Log("Waiting time");
        yield return new WaitForSeconds(time);
        action();
    }

    public void Start()
    {
        InitGame();
    }

    public void EndGame()
    {
        game = null;
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
                if (game.Alive <= 1)
                {
                    EndGame();
                    return;
                }
                StartCoroutine(WaitForTime(GoNext, 1f));
            } else if (game.State == 1)
            {
                //update DemandOffer
                StartCoroutine(WaitForAllReady(GoNext));
            } else if (game.State == 2)
            {
                //sum up results of requests of mat.
                game._requests_of_mat.Clear();
                StartCoroutine(WaitForAllReady(GoNext));
            } else if (game.State == 3)
            {
                //sum up results of production.
                StartCoroutine(WaitForAllReady(GoNext));
            } else if (game.State == 4)
            {
                //sum up results of requests of prod.
                game._requests_of_prod.Clear();
                StartCoroutine(WaitForTime(GoNext, 1f));
            }
        }
    }


}

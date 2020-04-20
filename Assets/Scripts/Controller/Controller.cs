using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Management;

public class Controller : MonoBehaviour
{

    public Management.Management game = null;
    public List<Player> Players { get; private set; } = new List<Player>();
    public bool IsReadyToGoNext { get; private set; } = false;
    public bool AllPlayersReady { get; private set; } = false;

    public void AddPlayer(Player player)
    {
        if (Players.Contains(player))
            return;
        player.Order = Players.Count;
        Players.Add(player);
    }

    public void AddRequestOfMat(int price, int mat, Player player)
    {
        game.State.AddRequestOfMat(price, mat, player.Director);
    }

    public void AddRequestOfProd(int price, int prod, Player player)
    {
        game.State.AddRequestOfProd(price, prod, player.Director);
    }

    public void InitGame()
    {
        List<Director> directors = new List<Director>();
        foreach(Player player in Players)
            directors.Add(player.Director);
        game = new Management.Management(directors);
        IsReadyToGoNext = false;
        StartCoroutine(WaitForAllReady(GoNext));
    }

    public void GoNext()
    {
        IsReadyToGoNext = true;
        game.NextState();
    }

    public IEnumerator WaitForAllReady(Action action)
    {
        AllPlayersReady = false;
        foreach (Player player in Players)
        {
            if (!player.Director.IsBankrupt)
                StartCoroutine(player.WaitForReady());
        }
        
        bool isReady = false;
        while (!isReady)
        {
            isReady = true;
            Players.ForEach(p => { if (!p.IsReady) isReady = false; });
            if (!isReady)
                yield return null;
        }
        AllPlayersReady = true;
        yield return null;
        action();
    }

    public IEnumerator WaitForTime(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    public void Start()
    {
        InitGame();
    }

    public void EndGame()
    {
        Debug.Log("End");
        //Application.Quit();
    }

    public void Update()
    {
        if (game == null)
            return;
        if (IsReadyToGoNext && AllPlayersReady)
        {
            IsReadyToGoNext = false;
            //begin new state
            if (game.State.CurrentState == GameState.FixCosts)
            {
                //changed fabrics, became bankrupts...
                if (game.Alive <= 1)
                {
                    EndGame();
                    return;
                }
                StartCoroutine(WaitForAllReady(GoNext));
            } else if (game.State.CurrentState == GameState.UpdateMarket)
            {
                //update DemandOffer
                StartCoroutine(WaitForTime(GoNext, 1f));
            } else if (game.State.CurrentState == GameState.MatRequest)
            {
                //start wait for requests
                StartCoroutine(WaitForAllReady(GoNext));
            } else if (game.State.CurrentState == GameState.Production)
            {
                //wait for production
                StartCoroutine(WaitForAllReady(GoNext));
            } else if (game.State.CurrentState == GameState.ProdRequest)
            {
                //wait for requests
                StartCoroutine(WaitForAllReady(GoNext));
            }
        }
    }


}

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Management;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class Controller : MonoBehaviour, IOnEventCallback
{
    public Management.Management game = null;
    public List<PlayerControl> Players { get; private set; } = new List<PlayerControl>();
    public bool IsReadyToGoNext { get; private set; } = false;
    public bool AllPlayersReady { get; private set; } = false;

    public SendOptions SendOptions { get; } = new SendOptions { Reliability = true };
    public RaiseEventOptions RaiseEventOptions { get; } = new RaiseEventOptions { Receivers = ReceiverGroup.Others };

    public void AddPlayer(PlayerControl player)
    {
        if (Players.Contains(player))
            return;
        player.Order = Players.Count;
        Players.Add(player);
    }

    public void AddRequestOfMat(int price, int mat, PlayerControl player)
    {
        game.State.AddRequestOfMat(price, mat, player.Director);
    }

    public void AddRequestOfProd(int price, int prod, PlayerControl player)
    {
        game.State.AddRequestOfProd(price, prod, player.Director);
    }

    public void InitGame()
    {
        List<Director> directors = new List<Director>();
        foreach(PlayerControl player in Players)
            directors.Add(player.Director);
        game = new Management.Management(directors);
        IsReadyToGoNext = false;
        StartCoroutine(WaitForAllReady(GoNext));
    }
    
    public void GoNext()
    {
        IsReadyToGoNext = true;
        game.NextState();
        PhotonNetwork.RaiseEvent(01, game.State.Bank.PriceLevel, RaiseEventOptions, SendOptions);
    }

    public IEnumerator WaitForAllReady(Action action)
    {
        AllPlayersReady = false;
        foreach (PlayerControl player in Players)
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

    public void EndGame()
    {
        Debug.Log("End");
        PhotonNetwork.LeaveRoom();
        //Application.Quit();
    }

    public void Update()
    {
        if (game == null || !PhotonNetwork.IsMasterClient)
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
                    PhotonNetwork.RaiseEvent(2, null, RaiseEventOptions, SendOptions);
                    EndGame();
                    return;
                }
                StartCoroutine(WaitForTime(GoNext, 1f));
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
            } else if (game.State.CurrentState == GameState.BuildUpgrade)
            {
                //wait for upgade
                StartCoroutine(WaitForAllReady(GoNext));
            }
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case 1:
                game.NextState();
                game.State.Bank.SetNewPriceLevel((int)photonEvent.CustomData, game.Alive);
                break;
            case 2:
                EndGame();
                break;
        }
    }

    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Management;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Linq;

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
        Players.Add(player);
    }

    public void AddRequestOfMat(int price, int mat, PlayerControl player)
    {
        Debug.Log("mat " + player.PhotonView.Owner.NickName);
        game.State.AddRequestOfMat(price, mat, player.Director);
        if (player.PhotonView.IsMine)
            PhotonNetwork.RaiseEvent(4, new int[] { price, mat, player.PhotonView.Owner.ActorNumber }, RaiseEventOptions, SendOptions);
    }

    public void AddRequestOfProd(int price, int prod, PlayerControl player)
    {
        Debug.Log("prod " + player.PhotonView.Owner.NickName);
        game.State.AddRequestOfProd(price, prod, player.Director);
        if (player.PhotonView.IsMine)
            PhotonNetwork.RaiseEvent(5, new int[] { price, prod, player.PhotonView.Owner.ActorNumber }, RaiseEventOptions, SendOptions);
    }

    public void ExecuteGame()
    {
        List<Director> directors = new List<Director>();
        foreach (PlayerControl player in Players.OrderBy(p => p.Order))
            directors.Add(player.Director);
        game = new Management.Management(directors);
        GoNext();
    }

    int PrevNum = 1;
    int GetNext(int seed)
    {
        PrevNum = (int)(((long)PrevNum * seed + 17377) % (int)(1e9 + 7));
        return PrevNum;
    }

    void SetOrders(int seed)
    {
        PrevNum = 173771;
        foreach (PlayerControl player in Players.OrderBy(p => p.PhotonView.Owner.ActorNumber))
        {
            player.Order = GetNext(seed);
        }
    }

    public void InitGame()
    {
        int seed = UnityEngine.Random.Range(0, 1000);
        PhotonNetwork.RaiseEvent(3, seed, RaiseEventOptions, SendOptions);
        SetOrders(seed);
        ExecuteGame();
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

    void CheckEnd()
    {
        if (game.Alive <= 1)
        {
            PhotonNetwork.RaiseEvent(2, null, RaiseEventOptions, SendOptions);
            EndGame();
            //return true;
        }
        //return false;
    }

    public void EndGame()
    {
        Debug.Log("End");
        StopAllCoroutines();
        PhotonNetwork.Disconnect(); 
        //Application.Quit();
    }

    public void Update()
    {
        if (game == null || !PhotonNetwork.IsMasterClient)
            return;
        CheckEnd();
        if (IsReadyToGoNext && AllPlayersReady)
        {
            IsReadyToGoNext = false;
            //begin new state
            if (game.State.CurrentState == GameState.FixCosts)
            {
                //changed fabrics, became bankrupts...
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
            case 3:
                SetOrders((int)photonEvent.CustomData);
                ExecuteGame();
                break;
            case 4:
                int[] info4 = (int[])photonEvent.CustomData;
                PlayerControl player4 = Players.First(p => p.PhotonView.Owner.ActorNumber == info4[2]);
                AddRequestOfMat(info4[0], info4[1], player4);
                break;
            case 5:
                int[] info5 = (int[])photonEvent.CustomData;
                PlayerControl player5 = Players.First(p => p.PhotonView.Owner.ActorNumber == info5[2]);
                AddRequestOfProd(info5[0], info5[1], player5);
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

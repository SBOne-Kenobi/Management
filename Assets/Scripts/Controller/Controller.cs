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

    public PlayerControl MinePlayer;

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
        MinePlayer = Players.First(p => p.PhotonView.IsMine);
        List<Director> directors = new List<Director>();
        Players.Sort((a, b) => { return a.Order.CompareTo(b.Order); });

        GetComponent<SpawnerPlayers>().SpawnPlayers(Players);

        foreach (PlayerControl player in Players)
            directors.Add(player.Director);
        game = new Management.Management(directors);
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(WaitForAllReady(GoNextServer));
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
    
    public void GoNextServer()
    {
        IsReadyToGoNext = true;
        game.NextState();
        PhotonNetwork.RaiseEvent(01, game.State.Bank.PriceLevel, RaiseEventOptions, SendOptions);
    }

    public void GoNextClient(int price_level)
    {
        IsReadyToGoNext = true;
        AllPlayersReady = true;
        game.NextState();
        game.State.Bank.SetNewPriceLevel(price_level, game.Alive);
    }

    public IEnumerator WaitForAllReady(Action action)
    {
        AllPlayersReady = false;
        PhotonNetwork.RaiseEvent(06, null, RaiseEventOptions, SendOptions);

        if (!MinePlayer.Director.IsBankrupt)
            StartCoroutine(MinePlayer.WaitForReady());
        

        bool isReady = false;

        //waiting for recieve all players
        while (!isReady)
        {
            isReady = true;
            Players.ForEach(p => { if (p.IsReady && !p.Director.IsBankrupt) isReady = false; });
            if (!isReady)
                yield return null;
        }
        PhotonNetwork.RaiseEvent(07, null, RaiseEventOptions, SendOptions);
        if (!MinePlayer.Director.IsBankrupt)
            MinePlayer.Mutable = true;

        isReady = false;
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
        if (game == null)
            return;
        if (game.Alive <= 1)
            StartCoroutine(EndGame());
    }

    

    IEnumerator EndGame()
    {
        Debug.Log("End");
        foreach (PlayerControl player in Players)
            if (!player.PhotonView.IsMine)
                PhotonNetwork.CloseConnection(player.PhotonView.Owner);
        while (Players.Count(p => !p.PhotonView.IsMine) > 0)
            yield return null;
        PhotonNetwork.Disconnect();
    }

    public void Update()
    {
        if (game == null)
            return;
        if (IsReadyToGoNext && AllPlayersReady)
        {
            IsReadyToGoNext = false;
            //begin new state
            if (PhotonNetwork.IsMasterClient)
            {
                CheckEnd();
                if (game.State.CurrentState == GameState.FixCosts)
                {
                    //changed fabrics, became bankrupts...
                    StartCoroutine(WaitForTime(GoNextServer, 1f));
                }
                else if (game.State.CurrentState == GameState.UpdateMarket)
                {
                    //update DemandOffer
                    StartCoroutine(WaitForTime(GoNextServer, 1f));
                }
                else if (game.State.CurrentState == GameState.MatRequest)
                {
                    //start wait for requests
                    StartCoroutine(WaitForAllReady(GoNextServer));
                }
                else if (game.State.CurrentState == GameState.Production)
                {
                    //wait for production
                    StartCoroutine(WaitForAllReady(GoNextServer));
                }
                else if (game.State.CurrentState == GameState.ProdRequest)
                {
                    //wait for requests
                    StartCoroutine(WaitForAllReady(GoNextServer));
                }
                else if (game.State.CurrentState == GameState.BuildUpgrade)
                {
                    //wait for upgade
                    StartCoroutine(WaitForAllReady(GoNextServer));
                }
            }
        }
    }

    public void SendFabricState(byte id, int actorNumber, int pos, int money)
    {
        PhotonNetwork.RaiseEvent(id, new int[] { actorNumber, pos, money }, RaiseEventOptions, SendOptions);
    }

    void UpdateFabricState(byte id, int actorNumber, int pos, int money)
    {
        Director director = Players.First(p => p.PhotonView.Owner.ActorNumber == actorNumber).Director;
        director.Money = money;
        switch(id)
        {
            case 21: //Buy
                director.BuyFabric(pos);
                break;
            case 22: //Sell
                director.SellFabric(pos);
                break;
            case 23: //Upgrade
                director.UpgradeFabric(pos);
                break;
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case 1:
                GoNextClient((int)photonEvent.CustomData);
                break;
            case 2: //unused
                EndGame();
                break;
            case 3:
                SetOrders((int)photonEvent.CustomData);
                ExecuteGame();
                break;
            case 4:
                int[] info = (int[])photonEvent.CustomData;
                PlayerControl player = Players.First(p => p.PhotonView.Owner.ActorNumber == info[2]);
                AddRequestOfMat(info[0], info[1], player);
                break;
            case 5:
                info = (int[])photonEvent.CustomData;
                player = Players.First(p => p.PhotonView.Owner.ActorNumber == info[2]);
                AddRequestOfProd(info[0], info[1], player);
                break;
            case 6:
                AllPlayersReady = false;
                if (!MinePlayer.Director.IsBankrupt)
                    StartCoroutine(MinePlayer.WaitForReady());
                break;
            case 7:
                if (!MinePlayer.Director.IsBankrupt)
                    MinePlayer.Mutable = true;
                break;
        }
        if (photonEvent.Code >= 21 && photonEvent.Code < 30)
        {
            int[] info = (int[])photonEvent.CustomData;
            int actorNumber = info[0];
            int pos = info[1];
            int money = info[2];

            UpdateFabricState(photonEvent.Code, actorNumber, pos, money);
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

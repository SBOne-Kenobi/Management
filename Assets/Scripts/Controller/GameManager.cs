using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{

    public List<PlayerControl> Players => GetComponent<Controller>().Players;
    
    public GameObject Game;

    [SerializeField]
    private GameObject Lobby;

    [SerializeField]
    private GameObject PlayerPrefab;

    [SerializeField]
    private ListPlayer ListPlayer;

    [SerializeField]
    private GameObject PlayerItemPrefab;

    [SerializeField]
    private Button ReadyButton;

    [SerializeField]
    private Button StartButton;

    private void Start()
    {
        //создать нового игрока
        PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector3(0,0,0), Quaternion.identity);
        var item = PhotonNetwork.Instantiate(PlayerItemPrefab.name, new Vector3(0,0,0), Quaternion.identity);
        ReadyButton.onClick.AddListener(item.GetComponent<PlayerItem>().GetReady);
        StartButton.onClick.AddListener(StartGame);
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            bool AllReady = true && (PhotonNetwork.CurrentRoom.MaxPlayers == ListPlayer.List.Count);
            foreach (PlayerItem item in ListPlayer.List)
                AllReady = AllReady && item.IsReady;
            StartButton.gameObject.SetActive(AllReady);
        } else
        {
            StartButton.gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        Game.SetActive(true);
        Lobby.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
        {
            GetComponent<Controller>().InitGame();
            PhotonNetwork.RaiseEvent(0, null, 
                new RaiseEventOptions { Receivers = ReceiverGroup.Others }, 
                new SendOptions { Reliability = true });
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " entered room");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (!Game.activeSelf)
        {
            PlayerControl player = Players.First(p => p.PhotonView.Owner == null);
            Players.Remove(player);
            PlayerItem item = ListPlayer.List.First(p => p.PhotonView.Owner == null);
            ListPlayer.Remove(item);
            PhotonNetwork.DestroyPlayerObjects(otherPlayer);
        }
        Debug.Log(otherPlayer.NickName + " leave room");
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case 0:
                StartGame();
                break;
        }
    }
}

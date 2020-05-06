using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    
    public List<Player> Players { get; private set; } = new List<Player>();

    private Player Player;

    [SerializeField]
    private GameObject PlayerPrefab;

    [SerializeField]
    private GameObject Game;

    public void Start()
    {
        //Vector3 pos = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        //PhotonNetwork.Instantiate(PlayerPrefab.name, pos, Quaternion.identity);
        Player = new Player();
        Players.Add(Player);
    }

    public void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        Game.SetActive(true);
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
        Debug.Log(otherPlayer.NickName + " leave room");
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }
}

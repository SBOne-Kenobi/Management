using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text LogText;

    [SerializeField]
    private InputField NickName;

    [SerializeField]
    private List<Button> Buttons;

    
    // Start is called before the first frame update
    void Start()
    {
        foreach (Button button in Buttons)
            button.interactable = false;

        string Nick = PlayerPrefs.GetString("NickName", "Player#" + Random.Range(1000, 9999));
        PhotonNetwork.NickName = Nick;
        NickName.text = Nick;

        Log("Players name is set to " + PhotonNetwork.NickName);

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "0";
        PhotonNetwork.ConnectUsingSettings();
    }

    void Log(string s)
    {
        Debug.Log(s);
        LogText.text += "\n";
        LogText.text += s;
    }

    public override void OnConnectedToMaster()
    {
        Log("Connected to Master");
        foreach (Button button in Buttons)
            button.interactable = true;
    }

    public void CreateRoom()
    {
        PhotonNetwork.NickName = NickName.text;
        PlayerPrefs.SetString("NickName", NickName.text);
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2, IsOpen = true, CleanupCacheOnLeave = false });
    }

    public void JoinRoom()
    {
        PhotonNetwork.NickName = NickName.text;
        PlayerPrefs.SetString("NickName", NickName.text);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Log("Joined the room");
        PhotonNetwork.LoadLevel("GameScene");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Log("Fail create room");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Log("Fail to connect random room");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Log("Fail to connect room");
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour, IPunObservable
{
    public bool IsReady { get; private set; } = false;
    private Image Image;
    private Transform Name;
    public PhotonView PhotonView { get; private set; }

    private ListPlayer ListPlayer;

    void Start()
    {
        PhotonView = GetComponent<PhotonView>();
        Image = GetComponent<Image>();
        Name = transform.Find("Name");
        Name.GetComponent<Text>().text = GetComponent<PhotonView>().Owner.NickName;
        ListPlayer = FindObjectOfType<ListPlayer>();
        ListPlayer.Add(this);
    }

    void Update()
    {
        if (IsReady)
            Image.color = Color.green;
        else
            Image.color = Color.red;
    }

    public void GetReady()
    {
        IsReady = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(IsReady);
        }
        else
        {
            IsReady = (bool) stream.ReceiveNext();
        }
    }
}

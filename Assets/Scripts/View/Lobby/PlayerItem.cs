﻿using Photon.Pun;
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

    [SerializeField]
    private GameObject Icon;
    
    public Controller Controller;

    public int IconNum { get; private set; } = -1;

    public void SetIcon(int id)
    {
        if (IconNum == id || Controller == null)
            return;
        
        IconNum = id;
        var texture = Controller.Icons.Images[id];
        Icon.GetComponent<Image>().sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
    }

    void Start()
    {
        Controller = FindObjectOfType<Controller>();
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
            Image.color = new Color(0, 1f, 0, 0.7f);
        else
            Image.color = new Color(1f, 0.03f, 0, 0.7f);
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
            stream.SendNext(IconNum);
        }
        else
        {
            IsReady = (bool) stream.ReceiveNext();
            SetIcon((int)stream.ReceiveNext());
        }
    }
}

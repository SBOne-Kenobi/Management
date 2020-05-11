using System.Collections;
using UnityEngine;
using Management;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerControl : MonoBehaviour, IPunObservable
{
    public Director Director { get; private set; } = new Director();
    public PhotonView PhotonView { get; private set; }
    public int Order = 0;
    public bool IsReady { get; private set; } = true;
    public bool Mutable { get; set; } = false;
    public string Name = "Default";
    private Controller controller;

    public void Start()
    {
        var parent = FindObjectOfType<GameManager>().Game.transform.Find("CommonCanvas");
        transform.SetParent(parent);

        PhotonView = GetComponent<PhotonView>();
        Name = PhotonView.Owner.NickName;
        if (!PhotonView.IsMine)
            GetComponent<Image>().color = Color.red;

        controller = FindObjectOfType<Controller>();
        controller.AddPlayer(this);
    }

    public void GetReady()
    {
        if (Mutable)
            IsReady = true;
    }

    public IEnumerator WaitForReady()
    {
        IsReady = false;
        while (!IsReady)
        {
            //ждем изменения состояния ready 
            yield return null;
        }
        Mutable = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //sync IsReady, Mutable
        //+ Money, product, materials
        if (stream.IsWriting)
        {
            stream.SendNext(IsReady);
            stream.SendNext(Mutable);
            stream.SendNext(Director.Money);
            stream.SendNext(Director.Product);
            stream.SendNext(Director.Materials);
        }
        else
        {
            IsReady = (bool) stream.ReceiveNext();
            Mutable = (bool) stream.ReceiveNext();
            Director.Money = (int) stream.ReceiveNext();
            Director.Product = (int) stream.ReceiveNext();
            Director.Materials = (int) stream.ReceiveNext();
        }
    }
}
